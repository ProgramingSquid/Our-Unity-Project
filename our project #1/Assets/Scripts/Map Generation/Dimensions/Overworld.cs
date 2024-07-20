using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Overworld", menuName = "Dimension/New Over World")]
public class Overworld : Dimension
{
    private void OnValidate()
    {
        foreach (var biome in biomes)
        {
            biome.dimension = this;
            
        }
    }

    public override float GetHumidity(Vector2 Pos)
    {
        throw new System.NotImplementedException();
    }

    public override float GetTemperature(Vector2 Pos)
    {
        throw new System.NotImplementedException();
    }
}
