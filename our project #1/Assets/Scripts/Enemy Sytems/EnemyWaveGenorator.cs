using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveGenorator : MonoBehaviour
{
    //TO DO: public List<Enemies>...

    [HideInInspector] public static EnemyWaveGenorator instance;

    
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void/*List<>*/ GetSpawnEnemies()
    {
        //return List<>;
    }

}
