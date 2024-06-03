using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;



public enum RandomnesssType
{
    none,
    MinAndMax,
    RandomOnCurve
}


public class EnemyRoundManager : MonoBehaviour
{
    // Use old dificulty weights (player dificulty choice (should be more powerful then upgrade weights), upgrades, ect)
    // determen a loop showing how to increas difficulty.
    // in the loop define a round
    //At the start of every round get a randimized difficulty setValue from the difficulty at the end(Max) and begining(Min) of the round
    //Use dificulty setValue and the enemies left from previous rounds to determine what combination of enemies to spawn or not to spawn
    //(balencing the number of enemies on player. May also want to take in how fast/easaly the player took down the previuos enemies)
    // 


    #region weights
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public float playerStartDificultyWeight;
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public float playerSpeedDificultyWeight;
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public float UpgradeStartDificultyWeight;
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public float UpgradeSpeedDificultyWeight;
    [GUIColor("#C2CBCE"), Sirenix.OdinInspector.BoxGroup("Weights")]
    [Range(0, 1)]
    public float startDificultyTypeWeight;
    [GUIColor("#C2CBCE"), Sirenix.OdinInspector.BoxGroup("Weights")]
    [Range(0, 1)]
    public float speedDificultyTypeWeight;
    #endregion


    [Sirenix.OdinInspector.BoxGroup("Grace Time in Between Rounds", false)]
    [LabelText("Between Round Grace Time")]
    public RandomValue<float> roundGraceTime;



    [Sirenix.OdinInspector.BoxGroup("Time Before First Round", false)]
    [LabelText("Start Of Game Grace Time")]
    public RandomValue<float> gameStartGraceTime;

    [FoldoutGroup("Round Langth")]
    public RandomValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public RandomValue<float> MaxRoundTime;

    [FoldoutGroup("Wave Amount")]
    public RandomValue<float> roundMinAmountOfWaves;
    [FoldoutGroup("Wave Amount")]
    public RandomValue<float> roundMaxAmountOfWaves;


    public Round currentRound;
    [DisplayAsString] public bool inRound = false;


    [HideInInspector] public static EnemyRoundManager instance;

    public void Awake()
    {

            instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
    }


    // Update is called once per frame
    private void Update()
    {
        if(inRound)
        {
            //To Do: Update round info

            //To Do: Update wave ending conditions

            //To Do: Update exsiting wave info

            //To Do: Creat new wave if needed
        }
        else
        {
            //To Do: Update round creating conditions

            //To Do: Start/Creat new round if needed
        }

    }

    public void StartNewRound()
    {

    }

}
[System.Serializable]
public class Round
{
    public float levelNumber;
    public float entryDifficulty;
    public float elapsedTime;
    public bool updateElapsed; 
    public List<Wave> waves = new List<Wave>();
    public Wave newestWave;
    public float newestWaveElapsedTime;
    public bool updateNewestWaveElapsed;
}

