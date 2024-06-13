using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStat", menuName = "Enemy Stat")]
public class EnemyStat : ScriptableObject
{
    public string tag;
    public bool AllowScalling;
    
    [HideLabel]public RandomValue<float> value;
}


