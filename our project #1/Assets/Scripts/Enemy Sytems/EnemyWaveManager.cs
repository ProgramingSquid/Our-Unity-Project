using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    [HideInInspector] public static EnemyWaveManager instance;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Manage Current Wave
        //Decide When to Start New Wave and its values

    }

    
    // To Do: CreatNewWave()...
}
[Serializable]
public class Wave
{
    List<Enemy> spawnEnemies = new List<Enemy>();
    List<Enemy> aliveEnemies = new List<Enemy>();
    public float aliveEnemyElapsedTime;
    public float newestWaveElapsedTime;
    public bool updateNewestWaveElapsed;
    public float waveNumber;

    public void SpawnEnemies()
    {
        
    }
}

public class EnemyWaveOption
{
    public List<Enemy> enemies = new List<Enemy>();
    public float power; //The value showing how good of an option it is

}
[Serializable]
public struct EnemySpawningPriorityEffectingParamater
{
    [Serializable]
    public enum AplyingType
    {
        multiply,
        divide,
        add,
        subtract
    }

    public AplyingType aplyingType;
    public RandomnessValue<float> multiplier;
    public List<EnemyDataSO> inclusionFlags;


}
