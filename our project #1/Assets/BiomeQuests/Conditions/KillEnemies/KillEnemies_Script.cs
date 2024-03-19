using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillEnemies", menuName = "Biomes/Biome Quests/Conditions/KillEnemies")]
public class KillEnemies : BiomeQuestCondition
{
    public override float UpdateCompleatness()
    {
        float compleatage = 1;
        /* return compleat percentage */
        return compleatage;
    }
}
