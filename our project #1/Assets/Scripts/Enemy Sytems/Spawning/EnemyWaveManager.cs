using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class Wave
{
    public List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> spawnEnemies = new();
    public List<Enemy> aliveEnemies = new List<Enemy>();
    public List<Enemy> activeEnemies = new List<Enemy>();
    public float waveNumber;
    public float priority;

    public void SpawnEnemies()
    {
        foreach (var enemy in spawnEnemies)
        {
            var gameObject =  enemy.enemyType.spawningType.Spawn(enemy.enemyType.prefab);
            aliveEnemies.Add(new Enemy(enemy.enemyType, gameObject));
        }
    }

    public void UpdateInfo()
    {

        activeEnemies.Clear();
        aliveEnemies.RemoveAll(enemy => enemy.healthSystem.currentHealth <= 0);

        foreach (var enemy in aliveEnemies)
        {
            if (enemy.enemySO.IsActiveCondition.DeterimainActiveness(enemy.enemyGameObject))
            {
                activeEnemies.Add(enemy);
            }
        }
    }

    public float GetTotalPriority()
    {
        float total = 0;
        foreach (var enemy in spawnEnemies)
        {
            total += enemy.enemyType.CalculatePriority() * enemy.enemyType.spawningPriorityInfluence;
        }
        priority = total;
        return total;
    }
}
public static class EnemyWaveManager
{

    [Serializable]
    public static class EnemySpawningPriority
    {
        public static ActiveEnemies activeEnemies = new ActiveEnemies();
        public static KilledEnemies killedEnemies = new KilledEnemies();
        public static LowHealthEnemies lowHealthEnemies = new LowHealthEnemies();
        public static HighHealthEnemies highHealthEnemies = new HighHealthEnemies();
        public static SpawnGroupEnemies spawnGroupEnemies = new SpawnGroupEnemies();
        public static SpawnGroupAmountOfEnemies spawnGroupAmount = new SpawnGroupAmountOfEnemies();


        public class ActiveEnemies : IEffectingParamater
        {
            [ShowInInspector] RandomValue<float> Multiplier { get; set; }
            public RandomValue<float> multiplier { get => Multiplier; set => Multiplier = value; }

            [ShowInInspector] List<EnemyTypeSO> InclusionFlags;
            public List<EnemyTypeSO> inclusionFlags { get => InclusionFlags; set => InclusionFlags = value; }
            public float value { get; set; }

            public float Calculate()
            {
                value = 0;
                foreach (var wave in EnemyRoundManager.currentRound.waves)
                {
                    value += wave.activeEnemies.Count();
                }
                return value * multiplier.RandonizeValue();
            }
        }
        public class KilledEnemies : IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float recentlyKilledTime;

            public float Calculate()
            {
                return value;
            }
        }

        public class LowHealthEnemies : IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float healthCutOff;

            public float Calculate()
            {
                return value;
            }
        }

        public class HighHealthEnemies : IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float healthCutOff;

            public float Calculate()
            {
                return value;
            }
        }


        public class SpawnGroupEnemies : IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float Calculate()
            {
                return value;
            }
        }
        public class SpawnGroupAmountOfEnemies : IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float Calculate()
            {
                return value;
            }
        }



        public interface IEffectingParamater
        {
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }

            public float value { get; set; }

            public float Calculate();
        }
    }

    public static class WaveFinder
    {
        public static List<Wave> FindNearestPriorityWaves(List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> enemies, float targetPriority, int minGroupSize, int maxGroupSize)
        {
            var allPossibleWaves = GetAllPossibleWaves(enemies, minGroupSize, maxGroupSize);
            var closestWaves = allPossibleWaves.OrderBy(wave => Math.Abs(targetPriority - wave.GetTotalPriority())).ToList();

            return closestWaves;
        }
        public static List<Wave> FindHighestPriorityWaves(List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> enemies, int minGroupSize, int maxGroupSize)
        {
            
            var allPossibleWaves = GetAllPossibleWaves(enemies, minGroupSize, maxGroupSize);
            var closestWaves = allPossibleWaves.OrderByDescending(wave => wave.GetTotalPriority()).ToList();

            return closestWaves;
        }

        private static List<Wave> GetAllPossibleWaves(List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> enemies, int minGroupSize, int maxGroupSize)
        {
            var allWaves = new List<Wave>();
            for (int i = minGroupSize; i < maxGroupSize; i++)
            {
                var combinations = GetCombinations(enemies, i);
                foreach (var comination in combinations)
                {
                    allWaves.Add(new Wave { spawnEnemies = comination });
                }
            }
            return allWaves;
        }

        private static IEnumerable<List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy>> GetCombinations(List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> enemies, int groupSize)
        {
            if (groupSize == 0)
            {
                yield return new List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy>();
                yield break;
            }

            if (enemies.Count < groupSize)
                yield break;

            DifficultyEnemyRangeFilter.DifficultyRangeEnemy first = enemies[0];
            List<DifficultyEnemyRangeFilter.DifficultyRangeEnemy> rest = enemies.Skip(1).ToList();

            foreach (var combination in GetCombinations(rest, groupSize - 1))
            {
                combination.Add(first);
                yield return combination;
            }

            foreach (var combination in GetCombinations(rest, groupSize))
            {
                yield return combination;
            }
        }
    }
}
