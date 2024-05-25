using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "EnemyType", menuName = "EnemyType")]
public class EnemyTypeSO : ScriptableObject
{

    //The base class that all enemy classes inheret from
    /*(All enemies must have...)*/

    [LabelText("Name")]
    public string enemyName;

    public EnemyStat<float> maxHealth;
    [AssetsOnly] public GameObject prefab;
    [Space(5)]


    [InlineEditor, ShowIf("hasSubTypes")]
    public List<EnemyTypeSO> SubTypes;
    [DisplayAsString] public bool hasSubTypes = true;

    [InlineEditor]
    [ShowIf("hasDifficultyVeriants")] public List<EnemyTypeSO> difficultyVeriants;
    [DisplayAsString] public bool hasDifficultyVeriants = true;
    public List<EnemyStat<float>> ExtraStatistics;

    #region Spawning priority
    [Tooltip(
        "The base value to control the lickly hood of" +
        "the enemy spawning. A negitive value makes it more rare," +
        "A value above zero makes it more common" +
        "Zero has no effect"
    )]
    [Title("Enemy Spawning Priority")]
    public float baseSpawningPriority;

    [Tooltip("How much this Enemy influences thier wave's total spawningPriority when spawned as a wave")]
    public float spawningPriorityInfluence;
    [Space(10)]

    [BoxGroup("activeEnemies", false)]
    public EnemySpawningPriorityEffectingParamater activeEnemies;
    [BoxGroup("activeEnemies", false)]
    public float activeDistance;


    [BoxGroup("killedEnemies", false)]
    public EnemySpawningPriorityEffectingParamater killedEnemies;
    [BoxGroup("killedEnemies", false)]
    public float recentlyKilledTime;


    [BoxGroup("lowHeathEnemies", false)]
    public EnemySpawningPriorityEffectingParamater lowHeathEnemies;
    [BoxGroup("lowHeathEnemies", false)]
    [LabelText("healthCutOff")] public float healthCutOff_Low;


    [BoxGroup("highHeathEnemies", false)]
    public EnemySpawningPriorityEffectingParamater highHeathEnemies;
    [BoxGroup("highHeathEnemies", false)]
    [LabelText("healthCutOff")] public float healthCutOff_High;


    [BoxGroup("spawnGroupSameEnemies", false)]
    public EnemySpawningPriorityEffectingParamater spawnGroupSameEnemies;

    [BoxGroup("spawnGroupTotalEnemies", false)]
    public EnemySpawningPriorityEffectingParamater spawnGroupTotalEnemies;
    #endregion

   


    private void OnValidate()
    {
        ValidateSubTypes();
    }

    void ValidateSubTypes()
    {
        foreach (EnemyTypeSO item in SubTypes)
        {
            item.hasSubTypes = false;
        }
        foreach (EnemyTypeSO item in difficultyVeriants)
        {
            item.hasSubTypes = false;
            item.hasDifficultyVeriants = false;
        }

        if (!hasDifficultyVeriants)
        {
            difficultyVeriants = null;
        }
        if (!hasSubTypes)
        {
            SubTypes = null;
        }
    }

    public void UpdateParameters()
    {
        
    }

    List<PropertyInfo> GetPropertiesOfType<T>(object obj)
    {
        var properties = new List<PropertyInfo>();
        PropertyInfo[] allProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in allProperties)
        {
            if (property.PropertyType == typeof(T))
            {
                properties.Add(property);
            }
        }
        return properties;
    }
}

    [Serializable]
public class EnemyStat<T>
{
    public string tag;
    public T defualtValue;

    public bool isScallingStat;
    
    public bool shouldScale;
    [ShowIf("isScallingStat")]  public T value;
    [ShowIf("isScallingStat")] public T maxValue;
    [ShowIf("isScallingStat")] T minValue;
    [ShowIf("isScallingStat")] float scallingSpeed;
    [ShowIf("isScallingStat")] LeanTweenType scallingMode;
}


