using NaughtyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    //Is responsible for handling difficulty
    //Decides which enemies should spawn and which values should be scalled based on player's skill
    public List<DifficultyEnemyRangeFilter> difficultyEnemyRangeFilters = new List<DifficultyEnemyRangeFilter>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentRanges = GetCurrentRanges();

        //calculate player's skill
        GameDifficulty.Agression.Calculate();

        //spawn enemies based on which ranges match up with skill value.

        
    }


    List<DifficultyEnemyRangeFilter> GetCurrentRanges()
    {
        var ranges = new List<DifficultyEnemyRangeFilter>();

        foreach (var range in difficultyEnemyRangeFilters)
        {
            if (range.difficultyCaculation.value >= range.minValue && range.difficultyCaculation.value <= range.maxValue)
            {
                ranges.Add(range);
            }
        }
        return ranges;
    }

    List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> GetFilteredEnemies()
    {
        //TO DO:

        //Get all included enemies in dictionary of enemies with range key
        //Get all blocked enemies in dictionary of enemies with range key
        //Get all musthave enemies in dictionary of enemies with range key

        //filter duplicates from each dictionary

        //Only keep enemies with higher priority when musthave enemies conflict with blocked enemies

        //Remove modifyied blocked enemies form all included enemies
        //Add musthave enemies to modfiyied included enemies

        //Remove duplicates in final included enemy dictionary

        //Return final included enemy dictionary as a list of enemies.

        return null;
    }

    private void OnValidate()
    {
        for (int i = 0; i < difficultyEnemyRangeFilters.Count; i++)
        {
            DifficultyEnemyRangeFilter difficultyEnemyRangeFilter = difficultyEnemyRangeFilters[i];
            difficultyEnemyRangeFilter.Validate();
            foreach (var enemy in difficultyEnemyRangeFilter.includedEnemies)
            {
                enemy.Validate();
            }
            foreach (var enemy in difficultyEnemyRangeFilter.BlockedEnemies)
            {
                enemy.Validate();
            }
            foreach (var enemy in difficultyEnemyRangeFilter.MustHaveEnemies)
            {
                enemy.Validate();

            }
        }
    }
}

[Serializable]
public class DifficultyEnemyRangeFilter
{
    [ShowInInspector, LabelText("Difficulty Value")] object inspectorDifficultyCaculation;
    [DisplayAsString] public IDifficultyCalculation difficultyCaculation;
    [Space(20)]
    public float minValue;
    public float maxValue;
    [Tooltip("A value to control the liklyhood of this range's enemy settings overpowering other ranges' settings")]
    public float priority;
    [Space(20)]
    [Tooltip("The list of inluded enemies that may or may not be included depending on priority when ranges are combined")]
    
    public List<DifficultyRangeEnemy> includedEnemies;
    
    [Tooltip("The list of enemies that are never going to be included in the total when ranges are combined regardless of priority")]
    public List<DifficultyRangeEnemy> BlockedEnemies;
    
    [Tooltip("The list of enemies that are always going to be included in the total when ranges are combined regardless of priority (over powers other blocked enemy list from other scripts depending on priority)")]
    public List<DifficultyRangeEnemy> MustHaveEnemies;

    [Serializable]
    public class DifficultyRangeEnemy
    {
       
        [Sirenix.OdinInspector.Required]public EnemyTypeSO enemyType;
        public List<EnemyScallingStat> scallingStats = new List<EnemyScallingStat>();
        
        public void Validate()
        {
            for (int i = 0; i < scallingStats.Count; i++)
            {
                var stat = scallingStats[i];
                if(enemyType == null) { return; }
                if(stat == null) { return; }

                if (!enemyType.EnemyStats.Contains(stat.enemyStat) && enemyType.maxHealth != stat.enemyStat)
                {
                    stat = null;
                    Debug.LogError("scallingStats at index of " + i.ToString() + " must be a stat from enemy Type");
                }
            }
        }
    }

    public void Validate()
    {
        if(inspectorDifficultyCaculation is IDifficultyCalculation)
        {
            difficultyCaculation = (IDifficultyCalculation)inspectorDifficultyCaculation;
        }
        else
        {
            inspectorDifficultyCaculation = null;
            Debug.LogError("Difficulty Value must be of implement IDifficultyCalculation");
        }
    }
}

public interface IDifficultyCalculation
{
    public float value { get; set; }
    public float Calculate();
}