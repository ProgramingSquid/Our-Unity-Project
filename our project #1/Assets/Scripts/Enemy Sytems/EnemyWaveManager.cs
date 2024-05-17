using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    [HideInInspector] public static EnemyWaveManager instance;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Manage Current Wave
        //Decide When to Start New Wave and its values

    }

   
}
