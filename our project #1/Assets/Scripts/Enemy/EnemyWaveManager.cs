using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [Tooltip("The amount of time to stop spawning enemies in between spawning."), Min(0), Space(5)]
    public float graceTime;
    [Tooltip("The amount of time to spawn enemies for."), Min(.0001f), Space(5)]
    public float spawnTime;
    [Tooltip("A delay between the spawning of each enemy (spawntime / this = the number of enemies you want to spawn)"), Min(.00005f), Space(5)]
    public float timeInBetweenSpawning;
    [Tooltip("A grace time that happens only once at the begening of a round"), Min(0), Space(5)]
    public float CountdownTime;
    [Space(5)]
    public  EnemyType[] enemyTypes;
    [Space(5)]
    public GameObject[] enemies;
    [HideInInspector] public static EnemyWaveManager instance;

    
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        for (int i = 0; i == enemyTypes.Length; i++)
        {
            enemyTypes[i].typeIndex = i;
            
        }
        foreach (EnemyType type in enemyTypes)
        {

         type.currentDifficultyLevel = type.baseDifficultyLevel;
         type.health = type.maxHealth;
        }
        Invoke("SpawnEnemies", CountdownTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveGenerator.instance.increaseDifficulty == true)
        {
            foreach (EnemyType type in enemyTypes)
            {
                type.UpdateDifficulty();
            }

        }
    }
    void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemey());
    }
    IEnumerator SpawnEnemey()
    {
        
        for (int i = 0; i != spawnTime / timeInBetweenSpawning; i++)
        {
            WaveGenerator.instance.spawnEnemies();
            yield return new WaitForSeconds(timeInBetweenSpawning);
        }

        WaveGenerator.instance.increaseDifficulty = false;

        yield return new WaitForSeconds(graceTime);

        WaveGenerator.instance.increaseDifficulty = true;
        yield return StartCoroutine(SpawnEnemey());
    }
}
