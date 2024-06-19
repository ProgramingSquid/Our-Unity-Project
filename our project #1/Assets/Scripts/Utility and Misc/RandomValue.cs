using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class RandomValue<T>
{
    [Sirenix.OdinInspector.BoxGroup]
    public RandomnesssType randomnessType;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.MinAndMax)]
    [HorizontalGroup] public  T min;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.MinAndMax)]
    [HorizontalGroup] public T max;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.MinAndMax)]

    [CurveRange(0, -2, 1, 2, EColor.Orange), Sirenix.OdinInspector.BoxGroup]
    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.RandomOnCurve)]
    public AnimationCurve curve;
    [Sirenix.OdinInspector.HideIf("@this.randomnessType == RandomnesssType.none")]
    [Sirenix.OdinInspector.BoxGroup]
    public T multiplyier;

    [Sirenix.OdinInspector.ShowIf("randomnessType", RandomnesssType.none)]
    [LabelText("value")] public T setValue;

    [DisplayAsString, HideInInlineEditors]public T value;



    public T RandonizeValue()
    {
        switch (randomnessType)
        {
           
            default:
                value = setValue;
                return setValue;

            case RandomnesssType.none:
                value = setValue;
                return setValue;
            
            case RandomnesssType.MinAndMax:
                if (typeof(T) == typeof(float))
                {
                    float _min = (float)(object)min;
                    float _max = (float)(object)max;

                    T randomValue = (T)(object)(Random.Range(_min, _max) * (float)(object)multiplyier);
                    value = randomValue;
                    return randomValue;
                }
                else if(typeof(T) == typeof(int))
                {
                    int _min = (int)(object)min;
                    int _max = (int)(object)max;
                    T randomValue = (T)(object)(Random.Range(_min, _max) * (int)(object)multiplyier);
                    value = randomValue;
                    return randomValue;
                }
                else 
                {
                    Debug.LogError("Cannot use randomness on types that are not float or int");
                    return default;
                }


            case RandomnesssType.RandomOnCurve:
                if (typeof(T) == typeof(float))
                {
                    float random = Random.Range(0f, 1f);
                    float curveValue = (float)curve.Evaluate(random) * (float)(object)multiplyier;
                    value = (T)(object)curveValue;
                    return (T)(object)curveValue;
                }
                else if (typeof(T) == typeof(int))
                {
                    int random = Random.Range(0, 1);
                    int curveValue = (int)curve.Evaluate(random) * (int)(object)multiplyier;
                    value = (T)(object)curveValue;
                    return (T)(object)curveValue;
                }
                else
                {
                    Debug.LogError("Cannot use randomness on types that are not float or int");
                    return default;
                }

        }
            
    }
}

