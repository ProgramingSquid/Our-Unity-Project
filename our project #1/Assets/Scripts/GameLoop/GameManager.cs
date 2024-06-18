using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DifficultyManager.SetGameSettings();
        EnemyRoundManager.SetGameSettings();
    }
    //Calls Update and Start methods for static mangaging classes

    void Start()
    {
        DifficultyManager.Validate();
        DifficultyManager.ResetScalling();
        StartGame();
    }

    void Update()
    {
        DifficultyManager.Update();
        
        EnemyRoundManager.Update(Time.deltaTime);
    }

    private void OnValidate()
    {
        DifficultyManager.Validate();
    }

    public void StartGame()
    {
        StartCoroutine(EnemyRoundManager.StartFirstRound());
    }
}
