using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings",menuName ="Game Settings")]
public class GameSettings : ScriptableObject
 {
    [Title("Rounds")]
    [LabelText("Start Of Game Grace Time")]
    public  RandomValue<float> gameStartGraceTime; //Randomized on use

    public  RandomValue<float> timeBetweenRounds; //Randomized on use

    [FoldoutGroup("Round Length")]
    public  RandomValue<float> minRoundTimeOffset; //Randomized on use, todo; on round start
    [FoldoutGroup("Round Length")]
    public  RandomValue<float> baseMinRoundTime; //Randomized on round start
    [FoldoutGroup("Round Length")]
    public  RandomValue<float> maxRoundTimeOffset; //Randomized on use
    [FoldoutGroup("Round Length")]
    public  RandomValue<float> baseMaxRoundTime; //Randomized on round start

    [LabelText("Max Wave Amount Offset")]
    public  RandomValue<int> roundMaxAmountOfWavesOffset; // Randomized on use
    [LabelText("Max Wave Amount")]
    public  RandomValue<int> baseRoundMaxAmountOfWaves; // Randomized on round start
    [Space(20)]
    [SerializeReference]
    public  List<EnemyRoundManager.RoundPriority.IPriorityParamater> priorities = new List<EnemyRoundManager.RoundPriority.IPriorityParamater>();
    public  RandomValue<float> minRoundCreatingPriority; //Randomized on 2 uses
    [Space(20)]
    [Tooltip("How high the highest wave spawning priority needs to be for a round to create a new wave")]
    public  RandomValue<float> baseMinWaveCreatingPriority; // Randomized on round start
    public  RandomValue<float> minWaveCreatingPriorityOffset; // Randomized on use
    [Tooltip("Creates a wave who's priority is the closest to highest wave's priority - this value. " +
        "With a value of zero the round always creates the wave with the highest possible priority")]
    public  RandomValue<float> baseWaveCreatingRandomness; // Randomized on round start
    public  RandomValue<float> waveCreatingRandomnessOffset; // Randomized on use
    [HorizontalGroup("EnemiesInWave")]
    [BoxGroup("EnemiesInWave/Max")]public  RandomValue<int> baseMaxEnemiesInWave; // Randomized on round start
    [BoxGroup("EnemiesInWave/Max")] public  RandomValue<int> maxEnemiesInWaveOffset; // Randomized on 2 uses
    [HorizontalGroup("EnemiesInWave")]
    [BoxGroup("EnemiesInWave/Min")] public  RandomValue<int> baseMinEnemiesInWave; // Randomized on round start, todo on 2 of its uses
    [BoxGroup("EnemiesInWave/Min")] public  RandomValue<int> minEnemiesInWaveOffset; // Randomized on 2  uses

    [Title("Difficulty")]
    //Decides which enemies should spawn and which values should be scaled based on player's skill
    public List<DifficultyEnemyRangeFilter> difficultyEnemyRangeFilters = new List<DifficultyEnemyRangeFilter>();

    [Title("Map Generation")]
    public int renderDistance;
}

