using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                enemy.enemyType.spawningType.Spawn(enemy.enemyType.prefab);
            }
        }

        public void UpdateInfo()
        {

            aliveEnemies.Clear();
            activeEnemies.Clear();
            foreach (var enemy in spawnEnemies)
            {
                if (enemy.enemyType.prefab.GetComponent<HealthSystem>().currentHealth > 0)
                {
                    aliveEnemies.Add(new Enemy(enemy.enemyType));
                }
            }

            foreach (var enemy in aliveEnemies)
            {
                if (enemy.enemySO.IsActiveCondition.DeterimainActiveness())
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
            public RandomValue<float> multiplier { get; set; }
            public List<EnemyTypeSO> inclusionFlags { get; set; }
            public float value { get; set; }

            public float Calculate()
            {
                return value;
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
            Debug.Log("enemy amount: "+ enemies.Count);
            
            //To Fix:
            for (int i = minGroupSize; i <= maxGroupSize; i++)
            {
                //For Loop Code isn't get ran...
                var combinations = GetCombinations(enemies, i);
                Debug.Log("combination amount: "+ combinations.ToList().Count);
                foreach (var comination in combinations)
                {
                    Debug.Log("combination enemy:"+ comination[0].enemyType.name);
                    allWaves.Add(new Wave { spawnEnemies = comination});
                }
                
            }
            Debug.Log("all wave amount: "+allWaves.Count);
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
