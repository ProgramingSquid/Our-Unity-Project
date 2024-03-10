using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewBiomeQuest", menuName = "Biomes/Biome Quests/Create New Biom Quest")]
public class BiomeQuest : ScriptableObject
{
    [Range(0, 1f), Tooltip("A value to influence the probability this quest may apear in a game, 0 = unlickly 1 = lickly.")]
    public float rarityPiority = .1f;
    [Tooltip("Biome that the quest is for."), Expandable]
    public Biome biome;
    public BiomeQuestCondition condition;
    #region dirrection Bias
    [Tooltip("A bias for what dirrection the quest should lead the player once compleated (use 0,0 for no bias)")]
    [Foldout("Dirrection Bias")] public Vector2 dirrectionBias;
    [Foldout("Dirrection Bias")] public AnimationCurve directionBiasFalloffCurve;
    #endregion
    #region distance Bias 
    
    [Tooltip("A bias for how far the quest should lead the player once compleated (use 0,0 for no bias)")]
    [Foldout("Distance Bias")] public float distanceBias;
    [Foldout("Distance Bias")] public AnimationCurve distanceBiasFalloffCurve;
    #endregion


    public UnityEvent<float> OnEquip = new UnityEvent<float>();
    public UnityEvent<float> OnCompleation = new UnityEvent<float>();
    public UnityEvent<float> OnSpawn = new UnityEvent<float>();

    public bool isConditionMet()
    {
        return condition.isConditionMet();
    }
}
public class BiomeQuestCondition : ScriptableObject
{
    public virtual bool isConditionMet()
    {
        return true;
    }
}
