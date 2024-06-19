using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum RandomnesssType
{
    none,
    MinAndMax,
    RandomOnCurve
}


public static class EnemyRoundManager
{
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    public static GameSettings gameSettings;
    [ReadOnly] public static float timmerBetweenRounds;
    public static int AmountOfRounds;
    public static float roundPriority;
    public static Round currentRound;
    [DisplayAsString] public static bool inRound = false;

    public static void SetGameSettings()
    {
        gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/GameSettings.asset");
    }

    // Update is called once per frame
    public static void Update(float deltaTime)
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
            if(currentRound.waves.Count > gameSettings.roundMaxAmountOfWaves.value)
            {
                TryEndRound();
                
            }
            if(currentRound.elapsedTime >= gameSettings.MaxRoundTime.RandonizeValue())
            {
                inRound = false;
            }
            if(!inRound) { return; }

            #region caluclate best waves
            var BestWaves = EnemyWaveManager.WaveFinder.FindHighestPriorityWaves
                (
                    DifficultyManager.allowedEnemies,
                    gameSettings.minEnemiesInWave.value,
                    gameSettings.maxEnemiesInWave.value
                );
            var bestWaveRandomIndex = Random.Range(0, BestWaves.Count);
            var BestWave = BestWaves[bestWaveRandomIndex];
            #endregion

            #region Spawn Best Waves if meet requirments:

            if (BestWave.priority >= gameSettings.minWaveCreatingPriority.value)
            {
                var priority = BestWave.priority - gameSettings.waveCreatingRandomnessOffset.value;
                var waves = EnemyWaveManager.WaveFinder.FindNearestPriorityWaves(DifficultyManager.allowedEnemies, priority,
                    gameSettings.minEnemiesInWave.value,
                    gameSettings.maxEnemiesInWave.value);
                var randomIndex = Random.Range(0, waves.Count);
                var wave = waves[randomIndex];

                currentRound.waves.Add(wave);
                currentRound.newestWave = wave;
                wave.SpawnEnemies();
            }
            #endregion
        }
        else
        {
            timmerBetweenRounds += deltaTime;

            //Updating round creating conditions:
            foreach (var priority in gameSettings.priorities)
            {
                roundPriority += priority.CalculatePriority();
            }
            //Start new round if needed:
            if(roundPriority >= gameSettings.minRoundCreatingPriority.RandonizeValue()) 
            {
                if(timmerBetweenRounds < gameSettings.timeBetweenRounds.RandonizeValue()) { return; }
                currentRound = StartNewRound();
            }
        }
    }

    public static IEnumerator StartFirstRound()
    {
        if (roundPriority >= gameSettings.minRoundCreatingPriority.RandonizeValue())
        {

            yield return new WaitForSeconds(gameSettings.gameStartGraceTime.RandonizeValue());
            AmountOfRounds = 0;
            var round = StartNewRound();
            currentRound = round;
        }
    }


    public static Round StartNewRound()
    {
        inRound = true;
        timmerBetweenRounds = 0;
        gameSettings.roundMaxAmountOfWaves.RandonizeValue();
        gameSettings.minWaveCreatingPriority.RandonizeValue();
        gameSettings.waveCreatingRandomnessOffset.RandonizeValue();
        gameSettings.minEnemiesInWave.RandonizeValue();
        gameSettings.maxEnemiesInWave.RandonizeValue();
        AmountOfRounds++;
        var round = new Round(AmountOfRounds);
        return round;
    }

    public static void TryEndRound()
    {
        if(currentRound.elapsedTime < gameSettings.MinRoundTime.RandonizeValue())
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

