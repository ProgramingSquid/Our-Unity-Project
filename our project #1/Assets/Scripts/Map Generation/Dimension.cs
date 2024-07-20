using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dimension : ScriptableObject
{
    [SerializeReference]
    public GenerationBase GenerationBase;

    [InlineEditor]
    public List<Biome> biomes;

    //Name...
    //description...
    //ect...

    public abstract float GetHumidity(Vector2 Pos);
    public abstract float GetTemperature(Vector2 Pos);
}
