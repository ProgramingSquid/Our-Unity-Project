using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDifficulty
{
    public class PlayerAgression : IDifficultyCalculation
    {
        public float Calculate()
        {
            return 1;
        }
    }

}
