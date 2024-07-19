using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A script that manages map generation. It controls and handles the logic for biome blending,
///  and the logic of generating GenerationFeatures.
/// </summary>
public static class MapManager
{
    public static List<Biome> biomes = new List<Biome>();
    [SerializeReference]
    public static GenerationBase.OffsetCalculation baseTerrainCalculation;

    public static void GenerateMap()
    {
        var noise = new TerrainNoise(biomes[0].GenerationProperties.TerrainNoiseProperties);
        noise.Generate();
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

    public static float GetHumidity(Vector2 Pos)
    {
        //Use forms of Noise to calculate a tilling pseudo random plane of numbers representing humidity.
        throw new NotImplementedException();
    }
    public static float GetTemperature(Vector2 Pos)
    {
        //Use forms of Noise to calculate a tilling pseudo random plane of numbers representing temperature
        //(The forms of noise may be different then humidity's calculation).

        throw new NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "New Biome", menuName = "New Biome")]
public class Biome : ScriptableObject
{
    [Serializable]
    public struct GenerationFeatureProperties
    {
        public TerrainNoise.Properties TerrainNoiseProperties;
    }


    public GenerationFeatureProperties GenerationProperties;

    public RandomValue<float> maxHumidityValue;
    public RandomValue<float> minHumidityValue;

    public RandomValue<float> maxTemperatureValue;
    public RandomValue<float> minTemperatureValue;



    //Name...
    //description...
    //ect...
}


public abstract class GenerationFeature
{
    public abstract void Generate();
}

public abstract class GenerationBase
{
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