using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyType", menuName ="EnemyType")]
public class EnemyType : ScriptableObject
{

    [HideInInspector] public int typeIndex;

    public float maxHealth;
    public float health;
    public float healthMultiplyer;
    [Space(20)]
    public int baseDifficultyLevel;
    public float currentDifficultyLevel;
    [HideInInspector, Min(1)] public float difficultyAmount;
    public float difficultyIncreaseSpeed;
    [Space(10)]
    public float spawnYPos;

   
    public void UpdateDifficulty()
    {

        currentDifficultyLevel += (Time.deltaTime * difficultyIncreaseSpeed);
        difficultyAmount = currentDifficultyLevel / baseDifficultyLevel;

        health = maxHealth * (difficultyAmount * healthMultiplyer) + maxHealth;
    }

    
}
