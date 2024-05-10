using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName ="EnemyType", menuName ="EnemyType")]
public class EnemySO : ScriptableObject
{

    //The base class that all enemy classes inheret from
    /*(All enemies must have...)*/

    public string name;
    public float maxHealth;
    [Space(5),
    Tooltip(
        "A value to controll the lickly hood of" +
        "the enemy to spawn. A negitive value makes it rare," +
        "A value above zero makes it more common" +
        "Zero has no effect"
    )]
    public float bias;

    [Expandable, ShowIf("hasSubTypes")]
    public List<EnemySO> SubTypes;
    [HideInInspector] public bool hasSubTypes = true;

    [Expandable]
    [ShowIf("hasDifficultyVeriants")] public List<EnemySO> difficultyVeriants;
    [HideInInspector] public bool hasDifficultyVeriants = true;
    
    private void OnValidate()
    {
        Validate();
    }

    void Validate()
    {
        foreach (EnemySO item in SubTypes)
        {
            item.hasSubTypes = false;
        }
        foreach (EnemySO item in difficultyVeriants)
        {
            item.hasSubTypes = false;
            item.hasDifficultyVeriants = false;
        }

        if (!hasDifficultyVeriants)
        {
            difficultyVeriants = null;
        }
        if (!hasSubTypes)
        {
            SubTypes = null;
        }

    }

}


