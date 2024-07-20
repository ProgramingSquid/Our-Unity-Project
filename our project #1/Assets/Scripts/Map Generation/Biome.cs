using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "New Biome")]
public class Biome : ScriptableObject
{
    [Serializable]
    public struct Properties
    {
        [SerializeReference]
        public GenerationBase.OffsetCalculation offsetCalculationProperties;
    }

    public Properties properties;

    public RandomValue<float> maxHumidityValue;
    public RandomValue<float> minHumidityValue;

    public RandomValue<float> maxTemperatureValue;
    public RandomValue<float> minTemperatureValue;
    public Dimension dimension;
    //Name...
    //description...
    //ect...
    private void OnValidate()
    {
        properties.offsetCalculationProperties = dimension.GenerationBase.offset;
    }
}
