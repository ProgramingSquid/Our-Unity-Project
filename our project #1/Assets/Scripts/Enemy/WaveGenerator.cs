using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveGenerator : MonoBehaviour
{
    // determen  the speed of wich the difficulty will increase at
    // bassed on the difficulty that the player chose and by adding all upgrades' power level together
    // Determent the difficulty at any time and add a random number between a max and min value you want the difficulty to change at any time
    // If how powerfull the enemy is eaqual the sum of those two numbers , or is in between a range bassed off the sum of the two numbers
    // if this is true spawn the enemy

    public float startingDifficulty;
    public float currentDifficulty;
    public float difficultySpeed;
    public float playerDifficulty;
    public Transform[] spawnpoints;
    public bool increaseDifficulty;

    [Space, Header("Range Modifyers")]
    [Tooltip("The higher number in a range that adds to the difficulty to determin if a enemy should spawn")]
    public float maxDifficultyRange;
    [Tooltip("The lower number in a range that subtracts to the difficulty to determin if a enemy should spawn")]
    public float minDifficultyRange;

    [HideInInspector] public static WaveGenerator instance;



    public void Awake()
    {

            instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        Setup();
    }
    public void Setup()
    {
        
        currentDifficulty = startingDifficulty;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDifficulty();
    }
    public void UpdateDifficulty()
    {
        if(increaseDifficulty == true)
        {
            currentDifficulty += (Time.deltaTime * difficultySpeed);
        }
    }

    public void DetermenStartingDifficulty()
    {
        startingDifficulty += (playerDifficulty / 2); //+ upgrades..
        difficultySpeed += (playerDifficulty / 15);
    }

    public void spawnEnemies()
    {
        int rand = Random.Range(0, EnemyWaveManager.instance.enemyTypes.Length);
        int randSpawn = Random.Range(0, spawnpoints.Length);

        if (EnemyWaveManager.instance.enemyTypes[rand].currentDifficultyLevel >= currentDifficulty - minDifficultyRange && EnemyWaveManager.instance.enemyTypes[rand].currentDifficultyLevel <= currentDifficulty + maxDifficultyRange)
        {
            Vector3 pos = new Vector3(spawnpoints[randSpawn].position.x,EnemyWaveManager.instance.enemyTypes[rand].spawnYPos, spawnpoints[randSpawn].position.z);
            Instantiate(EnemyWaveManager.instance.enemies[rand], pos, Quaternion.identity);
        }
    }
}



