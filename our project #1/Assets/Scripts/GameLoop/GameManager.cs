using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Dimension dimension;
    private void Awake()
    {
        MapManager.SetGameSettings();
        MapManager.target = player;
        MapManager.dimension = dimension;

        DifficultyManager.SetGameSettings();
        EnemyRoundManager.SetGameSettings();
    }
    //Calls Update and Start methods for static managing classes

    void Start()
    {
        DifficultyManager.Validate();
        DifficultyManager.ResetScalling();
        StartGame();
    }

    void Update()
    {
        MapManager.UpdateChunks();
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
