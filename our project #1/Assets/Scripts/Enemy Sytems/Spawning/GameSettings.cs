using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings",menuName ="Game Settings")]
public class GameSettings : ScriptableObject
 {
    [Title("Rounds")]
    [LabelText("Start Of Game Grace Time")]
    public  RandomValue<float> gameStartGraceTime;

    public  RandomValue<float> timeBetweenRounds;
    [ReadOnly] public  float timmerBetweenRounds;

    [FoldoutGroup("Round Langth")]
    public  RandomValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public  RandomValue<float> MaxRoundTime;

    [LabelText("Max Wave Amount")]
    public  RandomValue<int> roundMaxAmountOfWaves;
    [Space(20)]
    [SerializeReference]
    public  List<EnemyRoundManager.RoundPriority.IPriorityParamater> priorities = new List<EnemyRoundManager.RoundPriority.IPriorityParamater>();
    public  RandomValue<float> minRoundCreatingPriority;
    [Space(20)]
    [Tooltip("How high the highest wave spawning priority needs to be for a round to creat a new wave")]
    public  RandomValue<float> minWaveCreatingPriority;
    [Tooltip("Creats a wave who's priority is the cloasest to highest wave's priority - this value. " +
        "With a value of zero the round always creats the wave with the highest posible priority")]
    public  RandomValue<float> waveCreatingRandomnessOffset;
    [HorizontalGroup]
    public  RandomValue<int> maxEnemiesInWave;
    [HorizontalGroup]
    public  RandomValue<int> minEnemiesInWave;

    [Title("Difficulty")]
    //Decides which enemies should spawn and which values should be scalled based on player's skill
    public List<DifficultyEnemyRangeFilter> difficultyEnemyRangeFilters = new List<DifficultyEnemyRangeFilter>();
}

