using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class VolumeEffectTrigger : MonoBehaviour
{
    public PostProcesingEffectFeildSet[] postProcesing;
    
    public void TrigerEffect(float feild) 
    {
        int intFeild = Mathf.RoundToInt(feild);
        
        VolumeEffectTigerer.intance.SetBloom(postProcesing[intFeild].bloomIntesity, postProcesing[intFeild].bloomThreshold, postProcesing[intFeild].bloomScatter, postProcesing[intFeild].bloomTint);
        VolumeEffectTigerer.intance.SetChromaticAberration(postProcesing[intFeild].AberrationIntesity);
        VolumeEffectTigerer.intance.SetVignette(postProcesing[intFeild].vignetteIntesity, postProcesing[intFeild].vignetteSmoothness, postProcesing[intFeild].vignetteColor);
        VolumeEffectTigerer.intance.SetLensDistortion(postProcesing[intFeild].distortionIntesity, postProcesing[intFeild].distortionScale);
        
    }
    public void ResetAll(float Delay) => VolumeEffectTigerer.intance.Invoke("ResetAll", Delay);
    public void ResetBloom(float Delay) => VolumeEffectTigerer.intance.Invoke("ResetBloom", Delay);
    public void ResetAberration(float Delay) => VolumeEffectTigerer.intance.Invoke("ResetAberration", Delay);
    public void ResetVignette(float Delay) => VolumeEffectTigerer.intance.Invoke("ResetVignette", Delay);
    public void ResetLensDistortion(float Delay) => VolumeEffectTigerer.intance.Invoke("ResetLensDistortion", Delay);



}
[System.Serializable]
public class PostProcesingEffectFeildSet
{
    public string name = "Effect";
    public float bloomIntesity, bloomThreshold, bloomScatter;
    public Color bloomTint;
    [Space(25)]
    public float AberrationIntesity;
    [Space(25)]
    public float vignetteIntesity, vignetteSmoothness;
    public Color vignetteColor;
    [Space(25)]
    public float distortionIntesity, distortionScale;

}
