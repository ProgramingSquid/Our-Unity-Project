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
        //Decide New Wave's values

    }

    
    // To Do: CreatNewWave()...
}
[Serializable]
public class Wave
{
    List<Enemy> spawnEnemies = new List<Enemy>();
    List<Enemy> aliveEnemies = new List<Enemy>();
    public float aliveEnemyElapsedTime;
    public float waveNumber;
    public float priority; //The setValue showing how good of an option it is
    

    public void SpawnEnemies()
    {
        
    }
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
    public RandomValue<float> multiplier;
    public List<EnemyTypeSO> inclusionFlags;
}
