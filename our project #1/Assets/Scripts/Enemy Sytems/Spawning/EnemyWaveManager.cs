using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Wave
    {
        public List<Enemy> spawnEnemies = new List<Enemy>();
        public List<Enemy> aliveEnemies = new List<Enemy>();
        public List<Enemy> activeEnemies = new List<Enemy>();
        public float waveNumber;
        public float priority; //The setValue showing how good of an option it is

        public void SpawnEnemies()
        {

        }

        public void UpdateInfo()
        {

            aliveEnemies.Clear();
            activeEnemies.Clear();
            foreach (var enemy in spawnEnemies)
            {
                if (enemy.healthSystem.currentHealth > 0)
                {
                    aliveEnemies.Add(enemy);
                }
            }

            foreach (var enemy in aliveEnemies)
            {
                if (enemy.enemySO.isActiveCondition.DeterimainActiveness())
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
                total += enemy.enemySO.CalculatePriority() * enemy.enemySO.spawningPriorityInfluence;
            }
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
        public static List<Wave> FindNearestPriorityWaves(List<Enemy> enemies, float targetPriority, int minGroupSize, int maxGroupSize)
        {
            var allPossibleWaves = GetAllPossibleWaves(enemies, minGroupSize, maxGroupSize);
            var closestWaves = allPossibleWaves.OrderBy(wave => Math.Abs(targetPriority - wave.GetTotalPriority())).ToList();

            return closestWaves;
        }
        public static List<Wave> FindHighestPriorityWaves(List<Enemy> enemies, int minGroupSize, int maxGroupSize)
        {
            var allPossibleWaves = GetAllPossibleWaves(enemies, minGroupSize, maxGroupSize);
            var closestWaves = allPossibleWaves.OrderByDescending(wave => wave.GetTotalPriority()).ToList();

            return closestWaves;
        }

        private static List<Wave> GetAllPossibleWaves(List<Enemy> enemies, int minGroupSize, int maxGroupSize)
        {
            var allWaves = new List<Wave>();

            for (int i = minGroupSize; i <= maxGroupSize; i++)
            {
                allWaves.AddRange(GetCombinations(enemies, i).Select(combination => new Wave { spawnEnemies = combination }));
            }

            return allWaves;
        }

        private static IEnumerable<List<Enemy>> GetCombinations(List<Enemy> enemies, int groupSize)
        {
            if (groupSize == 0)
            {
                yield return new List<Enemy>();
                yield break;
            }

            if (enemies.Count < groupSize)
                yield break;

            Enemy first = enemies[0];
            List<Enemy> rest = enemies.Skip(1).ToList();

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
