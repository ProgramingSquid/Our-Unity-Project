using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using System;

public class GameplayPostProcessingEffect : MonoBehaviour
{
    [ReadOnly] public Volume volume;
    public GameplayEffect effect;

    [Button()]
    public void PlayEffect()
    {
        StartCoroutine(RunEffect());
    }
    public IEnumerator RunEffect()
    {
        float elapsed = 0;
        //setup effect by adding all overides
        foreach (var effectComponant in effect.componants)
        {
            volume.profile.Add(effectComponant.GetType());
        }
        //call all on start
        foreach (var effectControl in effect.OnStart)
        {
            effectControl.Invoke();
        }
        //while elapsed <= duration call all on update
        while (elapsed< effect.maxDuration)
        {
            foreach (var effectControl in effect.OnUpdate)
            {
                effectControl.Invoke();
            }
            yield return null;
        }
        //else call all on end and end by removing overides
        foreach (var effectControl in effect.OnEnd)
        {
            effectControl.Invoke();
        }

        foreach (var componant in volume.profile.components)
        {
            volume.profile.components.Remove(componant);
        }
    }
}
[Serializable]
public class GameplayEffect
{
    public float maxDuration;
    [SerializeReference]
    [InlineEditor] public List<VolumeComponent> componants = new List<VolumeComponent>();
    [InlineEditor] public List<IEffectControl> OnStart;
    [InlineEditor] public List<IEffectControl> OnUpdate;
    [InlineEditor] public List<IEffectControl> OnEnd;
    public interface IEffectControl
    {
        // the interface used for controlling gameplay effects, allowing for run to puase exicution of onUpdate until its finished 
        public IEnumerator Invoke();
    }

    public class Wait : IEffectControl
    {
        public float seconds;
        public IEnumerator Invoke()
        {
            yield return new WaitForSeconds(seconds);
        }
    }

    public class SetFloatValue : IEffectControl
    {
        public VolumeComponent component;
        [SerializeReference] public VolumeParameter volumeParameter;
        public float value;
        public IEnumerator Invoke()
        {
            if(volumeParameter is MinFloatParameter)
            {
                (volumeParameter as MinFloatParameter).value = value;
            }
            yield return null;
        }
    }
}
