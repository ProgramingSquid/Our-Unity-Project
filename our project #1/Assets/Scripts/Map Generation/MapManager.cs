using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
///  A script that manages map generation. It controls and handles the logic for biome blending,
///  and the logic of generating GenerationFeatures.
/// </summary>
public static class MapManager
{
    [ReadOnly] public static GameSettings gameSettings;

    [SerializeReference]
    public static Dimension dimension;
    public static Transform target;
    [ReadOnly] public static Vector2Int playerChunkPos = new();
    [ReadOnly] public static Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    public static void UpdateChunks()
    {
        playerChunkPos = new Vector2Int(
            Mathf.FloorToInt(target.position.x / dimension.GenerationBase.xSize),
            Mathf.FloorToInt(target.position.z / dimension.GenerationBase.zSize)
        );

        List<Vector2Int> chunksToDisable = new List<Vector2Int>();

        foreach (var chunk in chunks)
        {
            if (Vector2Int.Distance(chunk.Key, playerChunkPos) > gameSettings.renderDistance)
            {
                chunksToDisable.Add(chunk.Key);
            }
        }

        foreach (var chunkPos in chunksToDisable)
        {
            chunks[chunkPos].gameObject.SetActive(false);
        }

        for (int x = -gameSettings.renderDistance; x <= gameSettings.renderDistance; x++)
        {
            for (int z = -gameSettings.renderDistance; z <= gameSettings.renderDistance; z++)
            {
                Vector2Int gridPos = new Vector2Int(playerChunkPos.x + x, playerChunkPos.y + z);

                if (!chunks.ContainsKey(gridPos))
                {
                    GenerateChunk(gridPos);
                }
                else
                {
                    chunks[gridPos].gameObject.SetActive(true);
                }
            }
        }
    }

    private static void GenerateChunk(Vector2Int gridPos)
    {
        Vector3 pos = dimension.GenerationBase.CalculatePosition(gridPos);

        Vector2 chunkSize = new(dimension.GenerationBase.xSize, dimension.GenerationBase.zSize);

        foreach (var pass in dimension.GenerationBase.vertexPasses) 
        {
            pass.CalculateChunkValues(pos, gridPos, chunkSize);
        }

        Chunk newChunk = dimension.GenerationBase.Generate(pos);
        
        chunks.Add(gridPos, newChunk);
    }

    public static void GenerateMapValues()
    {
        //To Do:
        /*
            * UpdateChunks Seeds, 
            * Randomize RandomnessValues, 
            * ect...
        */
    }

    public static void SetGameSettings()
    {
        gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/GameSettings.asset");
    }
}

public class Chunk
{
    public GameObject gameObject;
    public Mesh mesh;

    public Chunk(GameObject gameObject, Mesh mesh)
    {
        this.gameObject = gameObject;
        this.mesh = mesh;
    }
}

public abstract class GenerationFeature : GenerationPipelinePass
{
    public abstract void Generate();
}
public abstract class GenerationPass : GenerationPipelinePass
{
    public abstract void Generate(Mesh chunkMesh);
}

public abstract class GenerationBase
{
    public Transform parent;
    public int xSize;
    public int zSize;

    [SerializeReference]
    public List<VertexPass> vertexPasses;
    public abstract Chunk Generate(Vector3 GameObjectPos);
    /// <summary>
    /// An optional method which can be overridden for additional control over how Generated Chunk GameObjects' 
    /// positions are generated relative to a grid position.
    /// </summary>
    /// <param name="gridPos">
    /// A position vector representing the position of the chunk cell in grid cardiants </param>
    /// <returns>A Vector3 representing where the GameObject's position should be </returns>
    public virtual Vector3 CalculatePosition(Vector2Int gridPos)
    {
        return (Vector2)gridPos;
    }

    /// <summary>
    /// A class used to define how vertex offsets on a base mesh should be calculated, allowing for control over biome 
    /// blending base terrain generation.
    /// </summary>
    public abstract class VertexPass : GenerationPipelinePass
    {
        public abstract Vector3 Calculate(Vector3 pos);
    }
}
public abstract class GenerationPipelinePass
{
    public virtual void CalculateChunkValues(Vector3 objectPos, Vector2Int gridPos, Vector2 chunkSize)
    {
    }
}