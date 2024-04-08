using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "newBiome", menuName = "New Biome")]
public class Biome : ScriptableObject
{
    public AnimationCurve noiseScaleCurve;
    public AnimationCurve islandDencityCurve;
    public AnimationCurve islandChanceCurve;
    public AnimationCurve islandGrassCurve;
    public Sprite questImage;
}
