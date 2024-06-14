using NaughtyAttributes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public static class DifficultyManager
{
    //Is responsible for handling difficulty
    public static GameSettings gameSettings;

    [Sirenix.OdinInspector.ReadOnly]
    public static List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> allowedEnemies = new List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
    
    [Sirenix.OdinInspector.ReadOnly]
    public static List<DifficultyEnemyRangeFilter> currentRanges = new List<DifficultyEnemyRangeFilter>();

    public static void SetGameSettings()
    {
        gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/GameSettings.asset");
    }

    // Update is called once per frame
    public static void Update()
    {
        CalculateSkill();
        currentRanges = GetCurrentRanges();
        allowedEnemies = GetFilteredEnemies(currentRanges);

            foreach (var enemy in allowedEnemies)
            {
                foreach (var stat in enemy.scallingStats)
                {
                    stat.ScaleStat(enemy.rangeFilter.difficultyCaculation);
                }
            }
    }

    public static void CalculateSkill()
    {
        GameDifficulty.agressivness.Calculate();
    }

    static List<DifficultyEnemyRangeFilter> GetCurrentRanges()
    {
        var ranges = new List<DifficultyEnemyRangeFilter>();

        foreach (var range in gameSettings.difficultyEnemyRangeFilters)
        {
            if (range.difficultyCaculation.value >= range.minValue && range.difficultyCaculation.value <= range.maxValue)
            {
                ranges.Add(range);
            }
        }
        return ranges;
    }

    static List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> GetFilteredEnemies(List<DifficultyEnemyRangeFilter> currentRanges)
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
        foreach (var enemy in filteredBlocked)
        {
            combinedAllowedEnemies.Remove(enemy.Value);
        }
        foreach (var enemy in mustHave)
        {
            combinedAllowedEnemies.Add(enemy.Value);
        }

        //Remove duplicates in final enemy list
        var allowedEnemies = FilterDuplicates(combinedAllowedEnemies);

        return allowedEnemies;
    }

    #region FilterDuplicateValues(dictionary)
    static Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> FilterDuplicateValues
        (Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> dictionary)
    {
        var uniqueValues = new HashSet<EnemyTypeSO>();
        var final = new Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();

        foreach (var pair in dictionary)
        {
            if (uniqueValues.Add(pair.Value.enemyType))
            {
                final.Add(pair.Key, pair.Value);
            }
        }

        return final;
    }
    #endregion

    static List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> FilterDuplicates(List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> list, bool LogErorMessage = false)
    {
        var uniqueValues = new HashSet<EnemyTypeSO>();
        var final = new List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();

        foreach (var item in list)
        {
            if (uniqueValues.Add(item.enemyType))
            {
                final.Add(item);
            }
            else if (LogErorMessage) { Debug.LogError("Removed duplicate: " + item.enemyType.name); }
        }

        return final;
    }

    #region GetRangeEnemies(Ranges)
    static
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
            foreach (var enemy in range.blockedEnemies)
            {
                allBlocked.Add(range, enemy);
            }
            foreach (var enemy in range.mustHaveEnemies)
            {
                allMustHave.Add(range, enemy);
            }
        }
        return (included: allIncluded, blocked: allBlocked, mustHave: allMustHave);

    }
    #endregion

    #region CrossFilter(Dictionary1, Dictionary2)
    static Dictionary<DifficultyEnemyRangeFilter, DifficultyEnemyRangeFilter.DifficultyRangeEnemy> 
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

    public static void Validate()
    {
        if(gameSettings == null) { SetGameSettings(); }
        for (int i = 0; i < gameSettings.difficultyEnemyRangeFilters.Count; i++)
        {
            DifficultyEnemyRangeFilter difficultyEnemyRangeFilter = gameSettings.difficultyEnemyRangeFilters[i];

            difficultyEnemyRangeFilter.includedEnemies = FilterDuplicates(difficultyEnemyRangeFilter.includedEnemies, true);
            difficultyEnemyRangeFilter.blockedEnemies = FilterDuplicates(difficultyEnemyRangeFilter.blockedEnemies, true);
            difficultyEnemyRangeFilter.mustHaveEnemies = FilterDuplicates(difficultyEnemyRangeFilter.mustHaveEnemies, true);

            foreach (var enemy in difficultyEnemyRangeFilter.includedEnemies)
            {
                enemy.rangeFilter = difficultyEnemyRangeFilter;
                enemy.Validate();
            }
            foreach (var enemy in difficultyEnemyRangeFilter.blockedEnemies)
            {
                enemy.rangeFilter = difficultyEnemyRangeFilter;
                enemy.Validate();
            }
            foreach (var enemy in difficultyEnemyRangeFilter.mustHaveEnemies)
            {
                enemy.rangeFilter = difficultyEnemyRangeFilter;
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
    public List<DifficultyRangeEnemy> blockedEnemies;
    
    [Tooltip("The list of enemies that are always going to be included in the total when ranges are combined regardless of priority (over powers other blocked enemy list from other scripts depending on priority)")]
    public List<DifficultyRangeEnemy> mustHaveEnemies;

    [Serializable]
    public class DifficultyRangeEnemy
    {
       
        [Sirenix.OdinInspector.Required]public EnemyTypeSO enemyType;
        public List<EnemyScallingStat> scallingStats;
        public DifficultyEnemyRangeFilter rangeFilter;

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