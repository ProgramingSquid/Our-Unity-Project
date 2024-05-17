using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyRoundManager : MonoBehaviour
{
    // Use old dificulty weights (player dificulty choice (should be more powerful then upgrade weights), upgrades, ect)
    // determen a loop showing how to increas difficulty.
    // in the loop define a round
    //At the start of every round get a randimized difficulty value from the difficulty at the end(Max) and begining(Min) of the round
    //Use dificulty value and the enemies left from previous rounds to determine what combination of enemies to spawn or not to spawn
    //(balencing the number of enemies on player. May also want to take in how fast/easaly the player took down the previuos enemies)
    // 


    #region weights
    public float playerStartDificultyWeight;
    public float playerSpeedDificultyWeight;
    public float UpgradeStartDificultyWeight;
    public float UpgradeSpeedDificultyWeight;
    [Range(0, 1)]
    public float startDificultyTypeWeight;
    [Range(0, 1)]
    public float speedDificultyTypeWeight;
    #endregion

    public LeanTweenType difficultyIncreaseCurve;
    public float startDificulty { get; private set; }
    public float currentDificulty { get; private set; }
    public float currentRoundDificulty { get; private set; }
    public float currentMaxRoundDificulty { get; private set; }
    public float currentMinRoundDificulty { get; private set; }

    [HideInInspector] public static EnemyRoundManager instance;



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
        DetermenGameStartingDifficulty();
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDifficulty();
    }
    public void UpdateDifficulty()
    {
        //Add togle for increasing dificulty...
    }

    public void DetermenGameStartingDifficulty()
    {

    }

    public void StartNewRound()
    {

    }

}

public class Round
{
    public float levelNumber;
    public float entryDifficulty;
    public float elapsedTime;
    public List<Wave> waves = new List<Wave>();
}

public class Wave
{
    List<Enemy> spawnEnemies = new List<Enemy>();
    List<Enemy> aliveEnemies = new List<Enemy>();
    public float elapsedTime;
    public bool updateElapsed;
    public float waveNumber;

    public void SpawnEnemies()
    {

    }

}

