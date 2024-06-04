using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum RandomnesssType
{
    none,
    MinAndMax,
    RandomOnCurve
}


public static class EnemyRoundManager
{

    #region weights
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public static float playerStartDificultyWeight;
    [GUIColor("#CFD8DC"), Sirenix.OdinInspector.BoxGroup("Weights")]
    public static float playerSpeedDificultyWeight;
    #endregion


    [Sirenix.OdinInspector.BoxGroup("Grace Time in Between Rounds", false)]
    [LabelText("Between Round Grace Time")]
    public static RandomValue<float> roundGraceTime;



    [Sirenix.OdinInspector.BoxGroup("Time Before First Round", false)]
    [LabelText("Start Of Game Grace Time")]
    public static RandomValue<float> gameStartGraceTime;

    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MaxRoundTime;

    [FoldoutGroup("Wave Amount")]
    public static RandomValue<float> roundMinAmountOfWaves;
    [FoldoutGroup("Wave Amount")]
    public static RandomValue<float> roundMaxAmountOfWaves;


    public static Round currentRound;
    [DisplayAsString] public static bool inRound = false;


    // Update is called once per frame
    public static void Update()
    {
        if(inRound)
        {
            if (currentRound.updateElapsed)
            {
                currentRound.elapsedTime += Time.deltaTime;
            }

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

    public static void StartNewRound()
    {

    }

}
[System.Serializable]
public class Round
{
    public float roundNumber;
    public float elapsedTime;
    public bool updateElapsed; 
    public List<Wave> waves = new List<Wave>();
    public Wave newestWave;
    public float newestWaveElapsedTime;
    public bool updateNewestWaveElapsed;
}

