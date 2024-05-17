using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName ="EnemyType", menuName ="EnemyType")]
public class EnemySO : ScriptableObject
{

    //The base class that all enemy classes inheret from
    /*(All enemies must have...)*/

    [LabelText("Name")]
    public string enemyName;
    
    public EnemyStat<float> maxHealth;
    [AssetsOnly] public GameObject prefab;

    [Space(5),
    Tooltip(
        "A value to controll the lickly hood of" +
        "the enemy to spawn. A negitive value makes it rare," +
        "A value above zero makes it more common" +
        "Zero has no effect"
    )]
    public float bias;

    [InlineEditor, ShowIf("hasSubTypes")]
    public List<EnemySO> SubTypes;
    [DisplayAsString] public bool hasSubTypes = true;

    [InlineEditor]
    [ShowIf("hasDifficultyVeriants")] public List<EnemySO> difficultyVeriants;
    [DisplayAsString] public bool hasDifficultyVeriants = true;
    
    private void OnValidate()
    {
        Validate();
    }

    void Validate()
    {
        foreach (EnemySO item in SubTypes)
        {
            item.hasSubTypes = false;
        }
        foreach (EnemySO item in difficultyVeriants)
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


