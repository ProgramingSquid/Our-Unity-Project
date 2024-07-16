using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BiomeProgressionManager : MonoBehaviour
{
    public CompassUI compassUI;
    public bool Compleation;
    public static BiomeProgressionManager instance;
    public bool justCompleated = false;
    public HealthDisplay playerHealth;
    MovementControl movement;
    Vector2 playerStartPos;
    float traveledDist;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        movement = MovementControl.player;
        justCompleated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Compleation == false) { justCompleated = false; }

        if(justCompleated)
        {
            playerStartPos = movement.transform.position;
            justCompleated = false;
            
        }
        

        if (Compleation && compassUI.playerZAngle > compassUI.BiomeAngle - 5 && compassUI.playerZAngle < compassUI.BiomeAngle + 5) 
        {
            string stringDistance = (playerStartPos - (Vector2)movement.transform.position).magnitude.ToString();
            traveledDist = float.Parse(stringDistance.Substring(0, stringDistance.Length - 2));
            compassUI.distanceTraveled = traveledDist;
        }

        if (traveledDist >= compassUI.questDistanceGoal && compassUI.questDistanceGoal > 0)
        {
            ProgressToNewBiome();
        }
    }

    void ProgressToNewBiome()
    {
        //To Fix
        /*MapGenerator.Generator.GenerateStartValues(true);*/ 

        BiomeQuestManager.instance.ClearEquiped();
        playerHealth.healManager.Heal();

    }
}
