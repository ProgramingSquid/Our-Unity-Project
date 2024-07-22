using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GenerationBase;
public class XZPlaneTerrainBase : GenerationBase
{
    [Serializable]
    public struct Properties
    {
        public int xSize;
        public int zSize;
        public float resolution;

        public bool shadeSmooth;
        public Material defaultMaterial;

        #region Contructors

        public Properties(int xSize, int zSize, float resolution, List<NoiseLayer> noiseLayers,
            bool shadeSmooth, Transform parent, Material defaultMaterial)
        {
            this.xSize = xSize;
            this.zSize = zSize;
            this.resolution = resolution;
            this.shadeSmooth = shadeSmooth;
            this.defaultMaterial = defaultMaterial;
        }
        public Properties(Properties properties)
        {
            this.xSize = properties.xSize;
            this.zSize = properties.zSize;
            this.resolution = properties.resolution;
            this.shadeSmooth = properties.shadeSmooth;
            this.defaultMaterial = properties.defaultMaterial;
        }

        #endregion
    }

    public Properties properties;
    Mesh mesh;
    GameObject gameObject;
    MeshFilter filter;
    MeshRenderer renderer;

    public XZPlaneTerrainBase(Properties properties, BaseGenerator baseGenerator)
    {
        this.properties = properties;
        this.baseGenerator = baseGenerator;
        this.parent = null;
    }
    public XZPlaneTerrainBase(Properties properties,BaseGenerator baseGenerator, Transform parent)
    {
        this.properties = properties;
        this.baseGenerator = baseGenerator;
        this.parent = parent;
    }


    public override void Generate(Vector3 GameObjectPos)
    {

        // Create a new GameObject for this feature
        GameObject featureObject = new GameObject("Base");
        featureObject.transform.parent = parent;
        featureObject.transform.position = GameObjectPos;
        featureObject.transform.localRotation = Quaternion.identity;

        // Add a MeshFilter and MeshRenderer to the GameObject
        MeshFilter meshFilter = featureObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = featureObject.AddComponent<MeshRenderer>();

        // Assign the default material to the MeshRenderer
        meshRenderer.material = properties.defaultMaterial;
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Calculate the number of vertices along each dimension
        int xCount = Mathf.RoundToInt((float)properties.xSize * properties.resolution);
        int zCount = Mathf.RoundToInt((float)properties.zSize * properties.resolution);

        // Create an array for the vertices
        Vector3[] vertices = new Vector3[xCount * zCount];

        // Set the position of each vertex
        for (int z = 0, i = 0; z < zCount; z++)
        {
            for (int x = 0; x < xCount; x++)
            {
                var pos = new Vector3(x * ((float)properties.xSize / xCount), 0, z * ((float)properties.zSize / zCount));
                vertices[i] = pos + baseGenerator.Calculate(new(x,0,z));
                i++;
            }
        }

        // Assign the vertices to the mesh
        mesh.vertices = vertices;

        // Create an array for the triangles
        int[] triangles = new int[(xCount - 1) * (zCount - 1) * 6];

        // Set the vertices for each triangle
        var vert = 0;
        var tri = 0;
        for (int z = 0; z < zCount - 1; z++)
        {
            for (int x = 0; x < xCount - 1; x++)
            {
                triangles[tri] = vert;
                triangles[tri + 1] = vert + xCount;
                triangles[tri + 2] = vert + 1;

                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + xCount;
                triangles[tri + 5] = vert + xCount + 1;
                vert++;
                tri += 6;
            }
            vert++;
        }

        // Assign the triangles to the mesh
        mesh.triangles = triangles;

        //Calculate Uvs
        var uvs = new Vector2[vertices.Length];

        for (int z = 0, i = 0; z < zCount; z++)
        {
            for (int x = 0; x < xCount; x++)
            {
                uvs[i] = new Vector2((float)x / xCount, (float)z / zCount);
                i++;
            }
        }

        //Assign the uvs to the mesh
        mesh.uv = uvs;

        //Calculate Normals
        if (properties.shadeSmooth)
        {
            // Recalculate the normals for the mesh for smooth shading
            mesh.RecalculateNormals();
        }
        else
        {
            // Calculate the normals for flat shading
            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 normal = Vector3.Cross(
                    vertices[mesh.triangles[i + 1]] - vertices[mesh.triangles[i]],
                    vertices[mesh.triangles[i + 2]] - vertices[mesh.triangles[i]]
                ).normalized;
                normals[mesh.triangles[i]] = normal;
                normals[mesh.triangles[i + 1]] = normal;
                normals[mesh.triangles[i + 2]] = normal;
            }
            mesh.normals = normals;
        }
    }
    public override Vector3 CalculatePosition(Vector2Int gridPos)
    {
        //To Do:
        //Account for plane mesh being generated with an baseGenerator.
        return base.CalculatePosition(gridPos);
    }


    
}

public class CalculatePerlinNoise : BaseGenerator
{
    //To Do: Add Support for using NoiseMask struct
    public List<NoiseMask> noise;
    public Vector2 globalOffset;
    public float globalAmplitude;

    public override Vector3 Calculate(Vector3 pos)
    {
        float y = 0;
        foreach (NoiseMask mask in noise)
        {
            float total = 0;
            float maskingValue = 0;
            float amplitudeSum = 0;

            foreach (var layer in mask.mask)
            {
                if (mask.useMask == false) { maskingValue = 1; break; }

                float x = (pos.x + globalOffset.x) * layer.scale / 10;
                float z = (pos.z + globalOffset.y) * layer.scale / 10;

                float value = Mathf.PerlinNoise(x + layer.offset.x, z + layer.offset.y);
                maskingValue += value * layer.amplitude * mask.intensity;
                amplitudeSum += layer.amplitude;
            }

            // Normalize maskingValue by dividing by the sum of amplitudes
            if (amplitudeSum > 0)
            {
                maskingValue /= amplitudeSum;
            }

            // Apply thresholdRange
            if (maskingValue < mask.thresholdRange.x)
            {
                maskingValue = 0;
            }
            else if (maskingValue > mask.thresholdRange.y)
            {
                maskingValue = 1;
            }

            // Apply clampRange
            maskingValue = Mathf.Clamp(maskingValue, mask.clampRange.x, mask.clampRange.y);

            foreach (var layer in mask.noiseLayers)
            {
                float x = (pos.x + globalOffset.x) * layer.scale / 10;
                float z = (pos.z + globalOffset.y) * layer.scale / 10;
                float value = Mathf.Clamp01(Mathf.PerlinNoise(x + layer.offset.x, z + layer.offset.y)) * layer.amplitude;

                total += value;
            }

            // Apply falloff to the normalized maskingValue
            y += total * Mathf.Pow(maskingValue, mask.falloff) * globalAmplitude;
        }

        // To Do: Find a way to ensure seamless transitions between chunks
        return new Vector3(0, y, 0);
    }
}

    [System.Serializable]
public  struct NoiseMask
{
    public bool useMask;
    [ShowIf("useMask")] public List<NoiseLayer> mask;
    [MinMaxSlider(0,1)]
    [ShowIf("useMask")] public Vector2 thresholdRange;
    [MinMaxSlider(0,1)]
    [ShowIf("useMask")] public Vector2 clampRange;
    [ShowIf("useMask")] public float falloff;
    [Range(0.01f,1)]
    [ShowIf("useMask")] public float intensity;
    public List<NoiseLayer> noiseLayers;
}
[System.Serializable]
public struct NoiseLayer
{
    public float scale; // The frequency of the noise
    public float amplitude; // The amplitude of the noise
    public Vector2 offset;
}