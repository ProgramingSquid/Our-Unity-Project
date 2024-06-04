using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyWaveManager
{
    public static List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    // Update is called once per frame
    public static void Update()
    {
        //Manage Current Wave
        //Decide New Wave's values
    }

    
    public static Wave CreatNewWave()
    {
        //TO DO
        return null;
    }
}
[Serializable]
public class Wave
{
    List<Enemy> spawnEnemies = new List<Enemy>();
    List<Enemy> aliveEnemies = new List<Enemy>();
    List<Enemy> activeEnemies = new List<Enemy>();
    public float aliveEnemyElapsedTime;
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
            if(enemy.healthSystem.currentHealth > 0)
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
}
[Serializable]
public static class EnemySpawningPriority
{
    public static ActiveEnemies activeEnemies = new ActiveEnemies();
    public static KilledEnemies killedEnemies = new KilledEnemies();
    public static LowHealthEnemies lowHealthEnemies= new LowHealthEnemies();
    public static HighHealthEnemies highHealthEnemies = new HighHealthEnemies();
    public  static SpawnGroupEnemies spawnGroupEnemies = new SpawnGroupEnemies();
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
