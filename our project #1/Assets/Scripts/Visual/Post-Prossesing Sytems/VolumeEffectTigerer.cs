using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;



public class VolumeEffectTigerer : MonoBehaviour
{
    Volume volume;

    public Bloom bloom;
    float bloomDefualtIntesity, bloomDefualtThreshold, bloomDefualtScatter; 
    Color bloomDefualtTint;

    public ChromaticAberration chromaticAberration;
    float aberrationDefualtIntensity;

    public Vignette vignette;
    float vignetteDefualtIntesity, vignetteDefualtSmoothness;
    Color vignetteDefualtColor;

    public LensDistortion lensDistortion;
    float lensDistortionDefualtIntesity, lensDistortionDefualtScale;

    public static VolumeEffectTigerer intance;
    private void Awake()
    {
        intance = this;
        volume = gameObject.GetComponent<Volume>();
        if (volume.profile.TryGet(out bloom))
        {
            bloomDefualtIntesity = bloom.intensity.value;
            bloomDefualtThreshold = bloom.threshold.value;
            bloomDefualtScatter = bloom.scatter.value;
            bloomDefualtTint = bloom.tint.value;
        }
        if (volume.profile.TryGet(out chromaticAberration))
        {
            aberrationDefualtIntensity = chromaticAberration.intensity.value;
        }
        if (volume.profile.TryGet(out lensDistortion))
        {
            lensDistortionDefualtIntesity = lensDistortion.intensity.value;
            lensDistortionDefualtScale = lensDistortion.scale.value;
            
        }
        if (volume.profile.TryGet(out vignette))
        {
            vignetteDefualtIntesity = vignette.intensity.value;
            vignetteDefualtSmoothness= vignette.smoothness.value;
            vignetteDefualtColor= vignette.color.value;
        }

    }
    public void ResetAll()
    {
        ResetAberration();
        ResetBloom();
        ResetVignette();
        ResetLensDistortion();
    }
    public void SetBloom(float intesity, float threshold, float scatter, Color tint)
    {

                bloom.intensity.overrideState = true;
                bloom.intensity.value = intesity;

                bloom.threshold.overrideState = true;
                bloom.threshold.value = threshold;

                bloom.scatter.overrideState = true;
                bloom.scatter.value = scatter;

                bloom.tint.overrideState = true;
                bloom.tint.value = tint;
        
    }
    public void ResetBloom() 
    {
        bloom.intensity.value = bloomDefualtIntesity; bloom.threshold.value = bloomDefualtThreshold; bloom.scatter.value = bloomDefualtScatter; bloom.tint.value = bloomDefualtTint; 
    }
    public void SetChromaticAberration(float intesity)
    {
            chromaticAberration.intensity.overrideState = true;
            chromaticAberration.intensity.value = intesity;
    }
    public void ResetAberration() 
    {
        chromaticAberration.intensity.value = aberrationDefualtIntensity;
    }
    public void SetVignette(float intesity, float smoothness, Color color)
    {

        vignette.intensity.overrideState = true;
        vignette.intensity.value = intesity;

        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = smoothness;
        
        vignette.color.overrideState = true;
        vignette.color.value = color;

    }
    public void ResetVignette()
    {
        vignette.intensity.value = vignetteDefualtIntesity; vignette.smoothness.value = vignetteDefualtSmoothness; vignette.color.value = vignetteDefualtColor;
    }
    public void SetLensDistortion(float intesity, float scale)
    {

        lensDistortion.intensity.overrideState = true;
        lensDistortion.intensity.value = intesity;

        lensDistortion.scale.overrideState = true;
        lensDistortion.scale.value = scale;

    }
    public void ResetLensDistortion()
    {
        lensDistortion.intensity.value = lensDistortionDefualtIntesity; lensDistortion.scale.value = lensDistortionDefualtScale;
    }
}

