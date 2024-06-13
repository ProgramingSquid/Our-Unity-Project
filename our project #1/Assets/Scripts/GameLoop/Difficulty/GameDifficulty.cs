using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDifficulty
{
    public static Difficulty agressivness = new Difficulty();
    public class Difficulty : IDifficultyCalculation
    {
        public float value { get; set; }


        public float Calculate()
        {
            value++;
            return value;
        }
    }

}
