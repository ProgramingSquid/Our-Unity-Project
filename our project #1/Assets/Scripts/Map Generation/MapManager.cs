using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
///  A script that manages map generation. It controls and handles the logic for biome blending,
///  and the logic of generating GenerationFeatures.
/// </summary>
public static class MapManager
{
    [SerializeReference]
    public static Dimension dimension;
    public static void GenerateMap()
    {
        dimension.GenerationBase.Generate(dimension.GenerationBase.parent.position);
        //To Do: Generate Generation Features with appropriate settings.
    }


    public static void GenerateMapValues()
    {
        //To Do:
        /*
            * Generate Seeds, 
            * Randomize RandomnessValues, 
            * ect...
        */
    }
}

public abstract class GenerationFeature
{
    public abstract void Generate();
}

public abstract class GenerationBase
{
    public Transform parent;

    [SerializeReference]
    public OffsetCalculation offset;
    public abstract void Generate(Vector3 GameObjectPos);
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
    public abstract class OffsetCalculation
    {
        public abstract Vector3 Calculate(Vector3 pos);
    }
}