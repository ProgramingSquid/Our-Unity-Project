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

    public static int AmountOfRounds;
    public static float roundPriority;
    public static Round currentRound;
    [DisplayAsString] public static bool inRound = false;

    public static void SetGameSettings()
    {
        gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/GameSettings.asset");
    }

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
            var BestWave = BestWaves[0];
            #endregion

            #region Spawn Best Waves if meet requirments:

            if (BestWave.priority >= gameSettings.minWaveCreatingPriority.value)
            {
                var priority = BestWave.priority - gameSettings.waveCreatingRandomnessOffset.value;
                var wave = EnemyWaveManager.WaveFinder.FindNearestPriorityWaves(DifficultyManager.allowedEnemies, priority,
                    gameSettings.minEnemiesInWave.value,
                    gameSettings.maxEnemiesInWave.value)[0];

                currentRound.waves.Add(wave);
                currentRound.newestWave = wave;
                wave.SpawnEnemies();
            }
            #endregion
        }
        else
        {
            gameSettings.timmerBetweenRounds += Time.deltaTime;

            //Updating round creating conditions:
            foreach (var priority in gameSettings.priorities)
            {
                roundPriority += priority.CalculatePriority();
            }
            //Start new round if needed:
            if(roundPriority >= gameSettings.minRoundCreatingPriority.RandonizeValue()) 
            {
                if(gameSettings.timmerBetweenRounds < gameSettings.timeBetweenRounds.RandonizeValue()) { return; }
                currentRound = StartNewRound();
            }
        }
    }

    public static IEnumerator<Round> StartFirstRound()
    {
        if (roundPriority >= gameSettings.minRoundCreatingPriority.RandonizeValue())
        {
            new WaitForSeconds(gameSettings.gameStartGraceTime.RandonizeValue());
            AmountOfRounds = 0;
            var round = StartNewRound();
            currentRound = round;
            yield return round;
        }
    }


    public static Round StartNewRound()
    {
        inRound = true;
        gameSettings.roundMaxAmountOfWaves.RandonizeValue();
        gameSettings.minWaveCreatingPriority.RandonizeValue();
        gameSettings.waveCreatingRandomnessOffset.RandonizeValue();
        gameSettings.minEnemiesInWave.RandonizeValue();
        gameSettings.maxEnemiesInWave.RandonizeValue();
        gameSettings.timmerBetweenRounds = 0;
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

