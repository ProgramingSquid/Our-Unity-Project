using NaughtyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;

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
        CalculateSkill();
        var currentRanges = GetCurrentRanges();
        var allowedEnemies = GetFilteredEnemies(currentRanges);

        //spawn enemies based on which ranges match up with skill value. 
    }

    private static void CalculateSkill()
    {
        GameDifficulty.agressivness.Calculate();
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

    List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> GetFilteredEnemies(List<DifficultyEnemyRangeFilter> currentRanges)
    {
        var allIncluded = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
        var allBlocked = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
        var allMustHave = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();

        //Get dictionaries for each list on each range:
        allIncluded = GetRangeEnemies(currentRanges).included;
        allBlocked = GetRangeEnemies(currentRanges).blocked;
        allMustHave = GetRangeEnemies(currentRanges).mustHave;

        //filter duplicates from each dictionary
        var included = FilterDuplicateValues(allIncluded);
        var blocked = FilterDuplicateValues(allBlocked);
        var mustHave = FilterDuplicateValues(allMustHave);

        //Only keep enemies with higher priority when mustHave enemies conflict with blocked enemies
        var filteredBlocked = CrossFilter(blocked, mustHave);

        //Combining All dictionaries into one list
        var combinedAllowedEnemies = new List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
        foreach (var enemy in included)
        {
            combinedAllowedEnemies.Add(enemy.Value);
        }
        foreach (var enemy in blocked)
        {
            combinedAllowedEnemies.Add(enemy.Value);
        }
        foreach (var enemy in mustHave)
        {
            combinedAllowedEnemies.Add(enemy.Value);
        }

        //Remove duplicates in final enemy list
        var allowedEnemies = combinedAllowedEnemies.GroupBy(pair => pair)
        .Select(group => group.First())
        .ToList();

        return allowedEnemies;
    }

    #region FilterDuplicateValues(dictionary)
    public Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> FilterDuplicateValues
        (Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> dictionary)
    {
        var final = dictionary.GroupBy(pair => pair.Value)
        .Select(group => group.First())
        .ToDictionary(pair => pair.Key, pair => pair.Value);
        return final;
    }
    #endregion

    #region GetRangeEnemies(Ranges)
    public
        (
        Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> included,
        Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> blocked,
        Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> mustHave
        )
        GetRangeEnemies(List<DifficultyEnemyRangeFilter> Ranges)
    {
        var allIncluded = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
        var allBlocked = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
        var allMustHave = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();

        foreach (var range in Ranges)
        {
            foreach (var enemy in range.includedEnemies)
            {
                allIncluded.Add(range, enemy);
            }
            foreach (var enemy in range.MustHaveEnemies)
            {
                allBlocked.Add(range, enemy);
            }
            foreach (var enemy in range.BlockedEnemies)
            {
                allMustHave.Add(range, enemy);
            }
        }
        return (included: allIncluded, blocked: allBlocked, mustHave: allMustHave);

    }
    #endregion

    #region CrossFilter(Dictionary1, Dictionary2)
    public Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> 
        CrossFilter(Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> dict1,
        Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> dict2)
    {
        var keysToRemove = new List<DifficultyEnemyRangeFilter>();

        foreach (var kvp in dict1)
        {
            var key1 = kvp.Key;
            var value1 = kvp.Value;

            foreach (var kvp2 in dict2)
            {
                var key2 = kvp2.Key;
                var value2 = kvp2.Value;

                if (value1 == value2)
                {
                    if (key1.priority < key2.priority)
                        keysToRemove.Add(key1);
                    else
                        keysToRemove.Add(key2);
                }
            }
        }

        // Remove the smaller keys
        foreach (var key in keysToRemove)
        {
            dict1.Remove(key);
        }
        return dict1;
    }
    #endregion

    private void OnValidate()
    {
        for (int i = 0; i < difficultyEnemyRangeFilters.Count; i++)
        {
            DifficultyEnemyRangeFilter difficultyEnemyRangeFilter = difficultyEnemyRangeFilters[i];
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
    [SerializeReference] public IDifficultyCalculation difficultyCaculation;
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
}

public interface IDifficultyCalculation
{
    public float value { get; set; }
    public float Calculate();
}