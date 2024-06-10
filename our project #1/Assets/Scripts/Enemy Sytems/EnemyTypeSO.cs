using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "EnemyType", menuName = "EnemyType")]
public class EnemyTypeSO : ScriptableObject
{

    //The base class that all enemy classes inheret from
    /*(All enemies must have...)*/

    [LabelText("Name")]
    [OnValueChanged("ChangeName")]
    public string enemyName;

    public EnemyStat maxHealth;
    [AssetsOnly] public GameObject prefab;
    [Space(5)]


    [InlineEditor, ShowIf("hasSubTypes")]
    public List<EnemyTypeSO> SubTypes;
    [DisplayAsString] public bool hasSubTypes = true;

    [InlineEditor]
    [ShowIf("hasDifficultyVeriants")] public List<EnemyTypeSO> difficultyVeriants;
    [DisplayAsString] public bool hasDifficultyVeriants = true;

    public List<EnemyStat> EnemyStats = new List<EnemyStat>();

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

    [SerializeReference]
    public List<EnemyWaveManager.EnemySpawningPriority.IEffectingParamater> spawningPriority = new List<EnemyWaveManager.EnemySpawningPriority.IEffectingParamater>();
    #endregion

    [SerializeReference]
    public ActiveEnemyCondition IsActiveCondition;
    [SerializeReference]
    public EnemySpawning spawningType;

    [Button()]
    void TestIsActive()
    {
        Debug.Log(IsActiveCondition.DeterimainActiveness());
    }

    private void OnEnable()
    {
        
        
    }

    private void OnValidate()
    {
        ValidateSubTypes();
    }

    void ChangeName()
    {
        var path = AssetDatabase.GetAssetPath(this);
        var supioriority = new string(string.Empty);
        if (this.hasSubTypes)
        {
            supioriority = "_base";
        }
        else if (this.hasDifficultyVeriants)
        {
            supioriority = "_subType";
        }
        
        else
        {
            supioriority = "_difficultyVeriant";
        }
        string assetName = (this.enemyName.Replace("basic", string.Empty, StringComparison.OrdinalIgnoreCase)).Replace(" ", string.Empty) + supioriority;
        this.name = assetName;
        AssetDatabase.RenameAsset(path, assetName);
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

    public float CalculatePriority()
    {
        float total = baseSpawningPriority;
        foreach (var priority in spawningPriority)
        {
            total += priority.Calculate();
        }
        return total;
    }

    public abstract class EnemySpawning
    {
        public virtual GameObject Spawn(GameObject prefab)
        {
            var newGameObject = Instantiate(prefab);
            return newGameObject;
        }
    }
}

[Serializable]
public class EnemyScallingStat
{
    public EnemyStat enemyStat;
    public float scallingMax;
    public float scallingMin;
    public float scallingMultiplyier;
    public float scallingSpeedMultiplyier;
    public ScallingType scallingType;
    
    [ShowIf("scallingType", ScallingType.function)]
    [SerializeReference]
    public MathimaticalFunctions.IMathimaticalFunction scallingCurveType;

    public enum ScallingType
    {
        function,
        multiply,
        divide,
        add,
        substract
    }

    public void ScaleStat(IDifficultyCalculation difficulty)
    {
        if (!enemyStat.AllowScalling) { return; }
        switch (scallingType)
        {
            case ScallingType.function:
                enemyStat.value.min += scallingMultiplyier * scallingCurveType.GetYValue(difficulty.value * scallingSpeedMultiplyier);
                enemyStat.value.max += scallingMultiplyier * scallingCurveType.GetYValue(difficulty.value * scallingSpeedMultiplyier);
                break;

            case ScallingType.multiply:
                enemyStat.value.min *= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                enemyStat.value.max *= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                break;
            case ScallingType.divide:
                enemyStat.value.min /= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                enemyStat.value.max /= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                break;
            case ScallingType.add:
                enemyStat.value.min += scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                enemyStat.value.max += scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                break;
            case ScallingType.substract:
                enemyStat.value.min -= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                enemyStat.value.max -= scallingMultiplyier * difficulty.value * scallingSpeedMultiplyier;
                break;
        } 
    }
}

public class ActiveByDistance : ActiveEnemyCondition
{
    public float distance;
    public GameObject enemyGameObject;
    public override bool DeterimainActiveness()
    {
        isActive = false;
        var _distance = MovementControl.player.transform.position -
            enemyGameObject.transform.position;

        if(_distance.magnitude <= distance)
        {
            isActive = true;
        }
        return isActive;
    }
}

public abstract class ActiveEnemyCondition
{
    public bool isActive { get; set; }

    public abstract bool DeterimainActiveness();
}


