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

    [Sirenix.OdinInspector.BoxGroup("Grace Time in Between Rounds", false)]
    [LabelText("Between Round Grace Time")]
    public static RandomValue<float> roundGraceTime;



    [Sirenix.OdinInspector.BoxGroup("Time Before First Round", false)]
    [LabelText("Start Of Game Grace Time")]
    public static RandomValue<float> gameStartGraceTime;

    public static RandomValue<float> timeBetweenRounds;

    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MaxRoundTime;

    [FoldoutGroup("Wave Amount")]
    public static RandomValue<int> roundMinAmountOfWaves;
    [FoldoutGroup("Wave Amount")]
    public static RandomValue<int> roundMaxAmountOfWaves;

    [Space(20)]
    public static Round currentRound;
    [DisplayAsString] public static bool inRound = false;

    [Space(20)]
    [Tooltip("How high the highest wave spawning priority needs to be for a round to creat a new wave")]
    public static RandomValue<float> minWaveCreatingPriority;
    [Tooltip("Creats a wave who's priority is the cloasest to highest wave's priority - this value. " +
        "With a value of zero the round always creats the wave with the highest posible priority")]
    public static RandomValue<float> waveCreatingRandomnessOffset;
    [HorizontalGroup]
    public static RandomValue<int> maxEnemiesInWave;
    [HorizontalGroup]
    public static RandomValue<int> minEnemiesInWave;


    // Update is called once per frame
    public static void Update()
    {
        if(inRound)
        {
            //Update elapsed
            if (currentRound.updateElapsed)
            {
                currentRound.elapsedTime += Time.deltaTime;
            }

            foreach(Wave wave in currentRound.waves)
            {
                wave.UpdateInfo();
            }

            var enemies = new List<Enemy>();
            foreach (var enemy in DifficultyManager.allowedEnemies)
            {
                enemies.Add(new Enemy(enemy.enemyType));
            }

            var BestWaves = EnemyWaveManager.WaveFinder.FindHighestPriorityWaves
                (
                    enemies,
                    minEnemiesInWave.value,
                    maxEnemiesInWave.value

                );
            var BestWave = BestWaves[0];

            if (BestWave.priority >= minWaveCreatingPriority.value)
            {
                var priority = BestWave.priority - waveCreatingRandomnessOffset.value;
                var wave = EnemyWaveManager.WaveFinder.FindNearestPriorityWaves( enemies, priority, 
                    minEnemiesInWave.value, 
                    maxEnemiesInWave.value)[0];

                wave.SpawnEnemies();

            }


        }
        else
        {
            //To Do: Update round creating conditions

            //To Do: Start/Creat new round if needed
        }
    }

    public static Round StartNewRound()
    {
        //TO DO
        //randimize random values

        return null;
    }
    public static class RoundPriority
    {

        public interface IPriorityParamater
        {
            public RandomValue<float> multiplier { get; set; }

            public float value { get; set; }

            public float Calculate();
        }
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

