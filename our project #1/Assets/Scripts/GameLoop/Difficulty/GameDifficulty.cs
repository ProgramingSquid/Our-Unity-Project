using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDifficulty
{
    public static PlayerAgression agressivness = new PlayerAgression();
    public class PlayerAgression : IDifficultyCalculation
    {
        public float value { get; set; }


        public float Calculate()
        {
            value = 1;
            return 1;
        }
    }

}
