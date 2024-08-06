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
        public float resolution;

        public bool shadeSmooth;
        [AssetsOnly]public GameObject prefab;

        #region Contructors

        public Properties(float resolution,
            bool shadeSmooth, GameObject prefab)
        {
            this.resolution = resolution;
            this.shadeSmooth = shadeSmooth;
            this.prefab = prefab;
        }
        public Properties(Properties properties)
        {
            this.resolution = properties.resolution;
            this.shadeSmooth = properties.shadeSmooth;
            this.prefab = properties.prefab;
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


    public override GameObject Generate(Vector3 GameObjectPos)
    {

        // Create a new GameObject for this feature
        GameObject featureObject = GameObject.Instantiate(properties.prefab, GameObjectPos, Quaternion.identity, parent);

        // Add a MeshFilter and MeshRenderer to the GameObject
        MeshFilter meshFilter = new MeshFilter();
        if(!featureObject.TryGetComponent<MeshFilter>(out meshFilter)) { meshFilter = featureObject.AddComponent<MeshFilter>(); }

        MeshRenderer meshRenderer = new MeshRenderer();
        if (!featureObject.TryGetComponent<MeshRenderer>(out meshRenderer)) { meshRenderer = featureObject.AddComponent<MeshRenderer>(); }
        
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Assign the mesh to the MeshFilter and any colliders:
        meshFilter.mesh = mesh;
        if (featureObject.TryGetComponent<MeshCollider>(out MeshCollider collider)) { collider.sharedMesh = mesh; }

        // Calculate the number of vertices along each dimension
        int xCount = Mathf.RoundToInt((float)xSize * properties.resolution);
        int zCount = Mathf.RoundToInt((float)zSize * properties.resolution);

        // Create an array for the vertices
        Vector3[] vertices = new Vector3[xCount * zCount];

        // Set the position of each vertex
        for (int z = 0, i = 0; z < zCount; z++)
        {
            for (int x = 0; x < xCount; x++)
            {
                var pos = new Vector3(x * ((float)xSize / xCount), 0, z * ((float)zSize / zCount));
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

#if UNITY_EDITOR
        var debug = featureObject.AddComponent<DataDebugHelper>();
        debug.stringData = baseGenerator.ToString();
#endif

        return featureObject;
    }
    public override Vector3 CalculatePosition(Vector2Int gridPos)
    {
        Vector2 offset = new Vector2(xSize / 2, zSize / 2);
        Vector2 position = new Vector2(gridPos.x * xSize, gridPos.y * zSize);
        Vector3 total = new Vector3(position.x - offset.x - 1, 0, position.y - offset.x - 1);

        return total;
    }


    
}

public class CalculatePerlinNoise : BaseGenerator
{
    //To Do: Add Support for using NoiseMask struct
    public List<NoiseLayerGroup> noiseBlendingGroups;
    public Vector2 globalOffset;
    [ReadOnly]public Vector2 chunkOffset;
    public int seed;
    public float globalAmplitude;

    public override void OffsetChunkValues(Vector3 objectPos, Vector2Int gridPos, Vector2 chunkSize)
    {
        chunkOffset = new Vector2(gridPos.x * chunkSize.x, gridPos.y * chunkSize.y);
    }

    public override Vector3 Calculate(Vector3 pos)
    {
        float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        float final = 0;
        foreach (NoiseLayerGroup group in noiseBlendingGroups)
        {
            float groupValue = 0;
            foreach (NoiseLayer layer in group.layers)
            {
                var noiseGenerator = new SimplexNoise(seed);

                float x = (pos.x + layer.offset.x + globalOffset.x + chunkOffset.x) * layer.scale / 10;
                float y = (pos.z + layer.offset.y + globalOffset.y + chunkOffset.y) * layer.scale / 10;

                float baseValue = Remap(noiseGenerator.Calculate(x, y), -1, 1, 0, 1);

                float falloffValue = Mathf.Pow(baseValue, layer.falloff);

                float clampValue = Mathf.Clamp(falloffValue, layer.clampRange.x, layer.clampRange.y);

                float value = clampValue * layer.amplitude * globalAmplitude;

                switch (layer.blendingType)
                {
                    case noiseBlendingType.Add:
                        groupValue += value;
                        break;
                    case noiseBlendingType.Subtract:
                        groupValue -= value;
                        break;
                    case noiseBlendingType.Multiply:
                        groupValue *= value;
                        break;
                    case noiseBlendingType.Divide:
                        groupValue /= value;
                        break;
                }
            }
            switch (group.blendingType)
            {
                case noiseBlendingType.Add:
                    final += groupValue;
                    break;
                case noiseBlendingType.Subtract:
                    final -= groupValue;
                    break;
                case noiseBlendingType.Multiply:
                    final *= groupValue;
                    break;
                case noiseBlendingType.Divide:
                    final /= groupValue;
                    break;
            }
        }
        return new Vector3(0, final, 0);
    }

    public override string ToString()
    {
        string output =
            "globalOffset: " + globalOffset.ToString() + ", " +
            "chunkOffset: " + chunkOffset.ToString() + ", " +
            "globalAmplitude: " + globalAmplitude;


        return output;
    }
}
[Serializable]
public enum noiseBlendingType
{
    Add,
    Subtract,
    Multiply,
    Divide
}
[System.Serializable]
public struct NoiseLayerGroup
{
    public List<NoiseLayer> layers;
    public bool noralizeAmplitudes;
    public noiseBlendingType blendingType;
}

[System.Serializable]
public struct NoiseLayer
{
    public float scale; // The frequency of the noise
    public float amplitude; // The amplitude of the noise
    [MinMaxSlider(0, 1)]
    public Vector2 clampRange;
    public float falloff;
    public Vector2 offset;
    public noiseBlendingType blendingType;
}