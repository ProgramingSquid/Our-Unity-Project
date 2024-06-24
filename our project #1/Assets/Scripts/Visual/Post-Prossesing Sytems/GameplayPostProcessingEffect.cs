using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Mono.Cecil.Cil;
using System.Linq;
[RequireComponent(typeof(Volume))]
public class GameplayPostProcessingEffect : MonoBehaviour
{
    [ReadOnly] public Volume volume;
    public GameplayEffect effect;
    private void OnValidate()
    {
        volume = GetComponent<Volume>();
    }
    [Button()]
    public void PlayEffect()
    {
        Debug.Log(volume.profile.components.Count);
        StartCoroutine(RunEffect());
    }
    public IEnumerator RunEffect()
    {
        float elapsed = 0;
        //setup effect by adding all overides
        for (int i = 0; i < volume.profile.components.Count; i++)
        {
            VolumeComponent effectComponant = volume.profile.components[i];
            if (volume.profile.TryGet(effectComponant.GetType(), out VolumeComponent component)) { continue; }
            volume.profile.Add(effectComponant.GetType());
        }

        //call all on start
        foreach (var effectControl in effect.OnStart)
        {
            yield return StartCoroutine(effectControl.Invoke(volume.profile));
        }
        //while elapsed <= duration call all on update
        while (elapsed < effect.maxDuration)
        {
            foreach (var effectControl in effect.OnUpdate)
            {
                float startTime = Time.time;
                yield return StartCoroutine(effectControl.Invoke(volume.profile)); // Wait for each effect to finish
                elapsed += Time.time - startTime; // Update elapsed time
            }
        }
        //else call all on end
        foreach (var effectControl in effect.OnEnd)
        {
            yield return StartCoroutine(effectControl.Invoke(volume.profile));
        }
    }
}
[Serializable]
public class GameplayEffect
{
    public float maxDuration;
    [InlineEditor, SerializeReference] public List<IEffectControl> OnStart = new();
    [InlineEditor, SerializeReference] public List<IEffectControl> OnUpdate = new();
    [InlineEditor, SerializeReference] public List<IEffectControl> OnEnd = new();
    public interface IEffectControl
    {
        // the interface used for controlling gameplay effects, allowing for run to puase exicution of onUpdate until its finished 
        public IEnumerator Invoke(VolumeProfile profile);
    }

    public class Wait : IEffectControl
    {
        public float waitTime;

        public IEnumerator Invoke(VolumeProfile profile)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }

    public class SetFloatValue : IEffectControl
    {
        public int componentIndex;
        public int prameterIndex;
        public float value;
        public bool debug;

        public IEnumerator Invoke(VolumeProfile profile)
        {
            if (componentIndex >= profile.components.Count)
            {
                Debug.LogError("Component index out of range");
                yield break;
            }

            var component = profile.components[componentIndex];

            if (prameterIndex >= component.parameters.Count)
            {
                Debug.LogError("Parameter index out of range");
                yield break;
            }
            if (component.parameters[prameterIndex] is FloatParameter floatParam)
            {
                var oldvalue = floatParam.value;
                floatParam.value = value;
                if (!debug) { yield break; }
                Debug.Log("Changed a : " + component.parameters[prameterIndex].GetType() + " from: " + oldvalue + " to: " + value.ToString());
            }

            yield return null;
        }
    }
    public class LerpFloatValue : IEffectControl
    {
        public int componentIndex;
        public int parameterIndex;
        public float startValue;
        public float endValue;
        public float duration;

        public IEnumerator Invoke(VolumeProfile profile)
        {
            if (componentIndex < profile.components.Count)
            {
                VolumeComponent component = profile.components[componentIndex];
                if (parameterIndex < component.parameters.Count && component.parameters[parameterIndex] is FloatParameter floatParam)
                {
                    float elapsedTime = 0;
                    while (elapsedTime < duration)
                    {
                        float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                        floatParam.value = newValue;
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    // Ensure the final value is set to endValue
                    floatParam.value = endValue;
                }
                else
                {
                    Debug.Log("Parameter not found");
                }
            }
            else
            {
                Debug.Log("Component not found");
            }
        }
    }

    public class SetColorValue : IEffectControl
    {
        public int componentIndex;
        public int parameterIndex;
        [ColorUsage(false, true)] public Color value;

        public IEnumerator Invoke(VolumeProfile profile)
        {
            if (componentIndex < profile.components.Count)
            {
                VolumeComponent component = profile.components[componentIndex];
                if (parameterIndex < component.parameters.Count && component.parameters[parameterIndex] is ColorParameter colorParam)
                {
                    colorParam.value = value;
                }
                else
                {
                    Debug.Log("Parameter not found");
                }
            }
            else
            {
                Debug.Log("Component not found");
            }
            yield return null;
        }
    }

    public class LerpColorValue : IEffectControl
    {
        public int componentIndex;
        public int parameterIndex;
        [ColorUsage(false, true)] public Color startValue;
        [ColorUsage(false, true)] public Color endValue;
        public float duration;

        public IEnumerator Invoke(VolumeProfile profile)
        {
            if (componentIndex < profile.components.Count)
            {
                VolumeComponent component = profile.components[componentIndex];
                if (parameterIndex < component.parameters.Count && component.parameters[parameterIndex] is ColorParameter colorParam)
                {
                    float elapsedTime = 0;
                    while (elapsedTime < duration)
                    {
                        Color newValue = Color.Lerp(startValue, endValue, elapsedTime / duration);
                        colorParam.value = newValue;
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    // Ensure the final value is set to endValue
                    colorParam.value = endValue;
                }
                else
                {
                    Debug.Log("Parameter not found");
                }
            }
            else
            {
                Debug.Log("Component not found");
            }
        }
    }
}
