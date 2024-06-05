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
    [ReadOnly] public static float timmerBetweenRounds;

    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MinRoundTime;
    [FoldoutGroup("Round Langth")]
    public static RandomValue<float> MaxRoundTime;

    [LabelText("Wave Amount")]
    public static RandomValue<int> roundMaxAmountOfWaves;
    [Space(20)]
    public static int AmountOfRounds;
    public static Round currentRound;
    [DisplayAsString] public static bool inRound = false;
    [Space(20)]
    [SerializeReference]
    public static List<RoundPriority.IPriorityParamater> priorities = new List<RoundPriority.IPriorityParamater>();
    public static RandomValue<float> minRoundCreatingPriority;
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
            if (currentRound.updateElapsed)
            {
                currentRound.elapsedTime += Time.deltaTime;
            }

            foreach(Wave wave in currentRound.waves)
            {
                wave.UpdateInfo();
            }

            //Update round ending conditions:
            if(currentRound.waves.Count > roundMaxAmountOfWaves.value)
            {
                TryEndRound();
                
            }
            if(currentRound.elapsedTime >= MaxRoundTime.RandonizeValue())
            {
                inRound = false;
            }
            if(!inRound) { return; }

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

                currentRound.waves.Add(wave);
                currentRound.newestWave = wave;
                wave.SpawnEnemies();
            }
        }
        else
        {
            timmerBetweenRounds += Time.deltaTime;
            var totalPriority = new float();

            //Updating round creating conditions:
            foreach (var priority in priorities)
            {
                totalPriority += priority.CalculatePriority();
            }
            //Start new round if needed:
            if(totalPriority >= minRoundCreatingPriority.RandonizeValue()) 
            {
                if(timmerBetweenRounds < timeBetweenRounds.RandonizeValue()) { return; }
                currentRound = StartNewRound();
            }
        }
    }

    public static Round StartNewRound()
    {
        inRound = true;
        roundMaxAmountOfWaves.RandonizeValue();
        minWaveCreatingPriority.RandonizeValue();
        waveCreatingRandomnessOffset.RandonizeValue();
        minEnemiesInWave.RandonizeValue();
        maxEnemiesInWave.RandonizeValue();
        timmerBetweenRounds = 0;
        AmountOfRounds++;
        var round = new Round(AmountOfRounds);
        return round;
    }

    public static void TryEndRound()
    {
        if(currentRound.elapsedTime < MinRoundTime.RandonizeValue())
        {
            return;
        }
        inRound = false;
    }
    public static class RoundPriority
    {

        public interface IPriorityParamater
        {
            public RandomValue<float> multiplier { get; set; }

            public float value { get; set; }

            public float CalculatePriority();
        }
    }
    
}
[System.Serializable]
public class Round
{
    public int roundNumber;
    public float elapsedTime;
    public bool updateElapsed; 
    public List<Wave> waves = new List<Wave>();
    public Wave newestWave;

    public Round(int number)
    {
        roundNumber = number;
        updateElapsed = true;
        elapsedTime = 0;
    }
}

