using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    //Is responsible for handling difficulty
    //Decides which enemies should spawn and which values should be scalled based on player's skill

    public float currentDificulty { get; private set; }
    public LeanTweenType difficultyIncreaseCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //calculate player's skill
        //spawn enemies based on which ranges match up with skill value.
    }
}

public class DifficultyEnemyRangeFilter
{

    public DifficultyCalculation calculationValue;
    public float minValue;
    public float maxValue;
    [Tooltip("A value to control the liklyhood of this range's enemy settings overpowering other ranges' settings")]
    public float priority;
    [Space(20)]
    [Tooltip("The list of inluded enemies that may or may not be included depending on priority when ranges are combined")]
    public List<EnemyTypeSO> includedEnemies;
    [Tooltip("The list of enemies that are never going to be included in the total when ranges are combined regardless of priority")]
    public List<EnemyTypeSO> BlockedEnemies;
    [Tooltip("The list of enemies that are always going to be included in the total when ranges are combined regardless of priority (over powers other blocked enemy list from other scripts depending on priority)")]
    public List<EnemyTypeSO> MustHaveEnemies;

    //TO Do: custom data type for included enemies.
    //for enemty type and the enemy type's stats.
}

[SerializeField]
public interface DifficultyCalculation
{
    public dynamic Calculate();
}