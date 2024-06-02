using NaughtyAttributes;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


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
    public RandomnessValue<float> roundGraceTime;



    [Sirenix.OdinInspector.BoxGroup("Time Before First Round", false)]
    [LabelText("Start Of Game Grace Time")]
    public RandomnessValue<float> gameStartGraceTime;

    [FoldoutGroup("Round Langth")]
    public RandomnessValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public RandomnessValue<float> MaxRoundTime;

    [FoldoutGroup("Wave Amount")]
    public RandomnessValue<float> roundMinAmountOfWaves;
    [FoldoutGroup("Wave Amount")]
    public RandomnessValue<float> roundMaxAmountOfWaves;


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
        Setup();
    }
    public void Setup()
    {
        DetermenGameDifficulty();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDifficulty();

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
    public void UpdateDifficulty()
    {
        //To Do: Add togle for increasing dificulty...
    }

    public void DetermenGameDifficulty()
    {

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
}

[System.Serializable]
public class RandomnessValue<T>
{
    [Sirenix.OdinInspector.BoxGroup]
    [ShowInInspector] RandomnesssType randomnessType;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.MinAndMax)]
    [ShowInInspector, HorizontalGroup] T min;
    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.MinAndMax)]
    [ShowInInspector, HorizontalGroup] T max;

    [CurveRange(0, 0, 1, 1, EColor.Orange), Sirenix.OdinInspector.BoxGroup]
    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.RandomOnCurve)]
    [ShowInInspector] AnimationCurve curve;
    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.RandomOnCurve)]
    [Sirenix.OdinInspector.BoxGroup]
    [ShowInInspector] T multiplyier;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.none)]
    [ShowInInspector, LabelText("value")] T setValue;

    public T value;



    public T RandonizeValue()
    {
        switch (randomnessType)
        {
           
            default:
                value = setValue;
                return setValue;

            case RandomnesssType.none:
                return setValue;
            
            case RandomnesssType.MinAndMax:
                if (typeof(T) == typeof(float))
                {
                    float _min = (float)(object)min;
                    float _max = (float)(object)max;

                    T randomValue = (T)(object)Random.Range(_min, _max);
                    value = randomValue;
                    return randomValue;
                }
                else if(typeof(T) == typeof(int))
                {
                    int _min = (int)(object)min;
                    int _max = (int)(object)max;
                    T randomValue = (T)(object)Random.Range(_min, _max);
                    value = randomValue;
                    return randomValue;
                }
                else 
                {
                    Debug.LogError("Cannot use randomness on types that are not float or int");
                    return default;
                }


            case RandomnesssType.RandomOnCurve:
                if (typeof(T) == typeof(float))
                {
                    float random = Random.Range(0f, 1f);
                    float curveValue = (float)curve.Evaluate(random) * (float)(object)multiplyier;
                    value = (T)(object)curveValue;
                    return (T)(object)curveValue;
                }
                else if (typeof(T) == typeof(int))
                {
                    int random = Random.Range(0, 1);
                    int curveValue = (int)curve.Evaluate(random) * (int)(object)multiplyier;
                    value = (T)(object)curveValue;
                    return (T)(object)curveValue;
                }
                else
                {
                    Debug.LogError("Cannot use randomness on types that are not float or int");
                    return default;
                }

        }
            
    }
}

