using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Calls Update and Start methods for static mangaging classes

    void Start()
    {
        DifficultyManager.Validate();
    }

    void Update()
    {
        DifficultyManager.Update();
        
        EnemyRoundManager.Update();
    }

    private void OnValidate()
    {
        DifficultyManager.Validate();
    }

    public void StartGame()
    {
        EnemyRoundManager.StartFirstRound();
    }
}
