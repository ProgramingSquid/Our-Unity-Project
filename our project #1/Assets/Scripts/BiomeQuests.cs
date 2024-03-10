using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewBiomeQuest", menuName = "Create New Biom Quest")]
public class BiomeQuest : ScriptableObject
{
    [Range(0, 1f), Tooltip("A value to influence the probability this quest may apear in a game, 0 = unlickly 1 = lickly.")] 
    public float rarityPiority = .1f;
    [Tooltip("Biome that the quest is for."), Expandable]
    public Biome biome;
    [Tooltip("A bias for what dirrection the quest should lead you once compleated (use 0,0 for no bias)")]
    public Vector2 dirrectionBias;
    [ShowIf("dirrectionBias != Vector2.zero")] public AnimationCurve directionBiasFalloff;
    
}



