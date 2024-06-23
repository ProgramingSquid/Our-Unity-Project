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

    public void PlayEffect()
    {
        StartCoroutine(RunEffect());
    }
    public IEnumerator RunEffect()
    {
        //setup effect by adding all overides
        //call all on start
        //while elapsed <= duration call all on update
        //else call all on end and end by removing overides

        throw new NotImplementedException("To do:");
    }
}

public class GameplayEffect
{
    [InlineEditor]
    public List<VolumeComponent> componants;
    public List<IEffectControl> OnStart;
    public List<IEffectControl> OnUpdate;
    public List<IEffectControl> OnEnd;
    public interface IEffectControl
    {
        // the interface used for controlling gameplay effects, allowing for run to puase exicution of onUpdate until its finished 
        public IEnumerator Invoke();
    }
}
