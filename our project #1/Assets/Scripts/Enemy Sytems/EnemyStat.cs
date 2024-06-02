using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStat", menuName = "Enemy Stat")]
public class EnemyStat : ScriptableObject
{
    public string tag;
    public bool AllowScalling;
    public RandomValue<float> value;
}


