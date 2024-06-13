
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "EnemyType")]
public class EnemyTypeSO : ScriptableObject
{

    //The base class that all enemy classes inheret from
    /*(All enemies must have...)*/
    [TabGroup("tab group","basic values")]
    [HorizontalGroup("tab group/basic values/base", 160)]
    [HideLabel, LabelWidth(90)]
    [VerticalGroup("tab group/basic values/base/left"), OnValueChanged("SetObjectName"), Title("Name", Bold = false, HorizontalLine = false)]
    public string enemyName;

    [VerticalGroup("tab group/basic values/base/left")]
    [AssetsOnly, HideLabel, PreviewField(150)] public GameObject prefab;

    [VerticalGroup("tab group/basic values/base/right"),TextArea(4, 6)]
    public string discription;

    [Title("Health:")]
    [BoxGroup("tab group/basic values/base/right/Health",false), InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    public EnemyStat maxHealth;

    
    [VerticalGroup("tab group/basic values/stats"), InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    public List<EnemyStat> EnemyStats = new List<EnemyStat>();

    [EnableIf("hasSubTypes")]
    [TabGroup("tab group", "Enemy Veriants")]
    public List<EnemyTypeSO> SubTypes;

    [TabGroup("tab group", "Enemy Veriants")]
    [DisplayAsString, LabelText("can have subTypes:")]public bool hasSubTypes = true;

    [EnableIf("hasDifficultyVeriants")]
    [TabGroup("tab group", "Enemy Veriants")]
    public List<EnemyTypeSO> difficultyVeriants;

    [TabGroup("tab group", "Enemy Veriants")]
    [DisplayAsString, LabelText("can have Difficulty Veriants:")] public bool hasDifficultyVeriants = true;



    #region Spawning priority
    [TabGroup("tab group", "Spawning")]
    [Tooltip(
        "The base value to control the lickly hood of" +
        "the enemy spawning. A negitive value makes it more rare," +
        "A value above zero makes it more common" +
        "Zero has no effect"
    )]
    [TabGroup("tab group", "Spawning")]
    [Title("Enemy Spawning Priority")]
    public float baseSpawningPriority;

    [TabGroup("tab group", "Spawning")]
    [Tooltip("How much this Enemy influences thier wave's total spawningPriority when spawned as a wave")]
    public float spawningPriorityInfluence;

    [TabGroup("tab group", "Spawning")]
    [SerializeReference]
    public List<EnemyWaveManager.EnemySpawningPriority.IEffectingParamater> spawningPriority = new List<EnemyWaveManager.EnemySpawningPriority.IEffectingParamater>();
    #endregion

    [TabGroup("tab group", "Spawning")]
    [SerializeReference]
    public EnemySpawning spawningType;

    [SerializeReference]
    [TabGroup("tab group", "Active Condition")]
    public ActiveEnemyCondition IsActiveCondition;

    [TabGroup("tab group", "Active Condition")]
    [Button()]
    void TestIsActive()
    {
        Debug.Log(IsActiveCondition.DeterimainActiveness());
    }

    
    private void OnValidate()
    {
        if (!hasDifficultyVeriants) 
        { 
            SubTypes.Clear(); difficultyVeriants.Clear();
        }

        if (!hasSubTypes)
        {
            SubTypes.Clear();
        }
        
        ValidateSubTypes();
    }

    public void SetObjectName()
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

    public void ValidateSubTypes()
    {
        hasDifficultyVeriants = true;
        hasSubTypes = true;

        foreach (EnemyTypeSO item in SubTypes)
        {
            if(item == this) { SubTypes.Remove(item);  continue; }
            item.hasSubTypes = false;
        }
        foreach (EnemyTypeSO item in difficultyVeriants)
        {
            if (item == this) { difficultyVeriants.Remove(item); continue; }
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
