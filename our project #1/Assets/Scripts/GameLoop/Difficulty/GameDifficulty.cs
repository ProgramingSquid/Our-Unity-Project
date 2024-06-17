using System;
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
[Serializable]
public class GameDifficultyVar
{
    [SerializeReference] public IDifficultyCalculation difficultyCaculationType;
    public string difficultyCaculationFeildName;
    public IDifficultyCalculation difficultyCaculation;

    public IDifficultyCalculation TrySetDifficultyCaculation()
    {

        var fields = typeof(GameDifficulty).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        List<IDifficultyCalculation> matchingFields = new();
        foreach (var fieldInfo in fields)
        {
            if (difficultyCaculationFeildName == "")
            {
                if (fieldInfo.FieldType == difficultyCaculationType.GetType() && fieldInfo.Name == difficultyCaculationFeildName)
                {
                    matchingFields.Add((IDifficultyCalculation)fieldInfo.GetValue(null)); continue;
                }
            }
            if (fieldInfo.FieldType == difficultyCaculationType.GetType())
            {
                matchingFields.Add((IDifficultyCalculation)fieldInfo.GetValue(null));
            }
        }
        if (matchingFields.Count > 1) { Debug.LogError(this.difficultyCaculation + " and " + this.difficultyCaculationFeildName + "matches multyiple Game difficulties"); }
        if (matchingFields.Count < 1) { Debug.LogError(this.difficultyCaculation + " and " + this.difficultyCaculationFeildName + "does not match any Game difficulties"); }
        difficultyCaculation = matchingFields[0];
        return matchingFields[0];
    }
}