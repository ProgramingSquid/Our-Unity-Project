using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.TerrainTools;
using System;

public class HealthDisplay : MonoBehaviour
{
    public List<DisplayLayer> healthBarLayers = new List<DisplayLayer>();
    public HealthSystem healthSystem;
    [ColorUsage(true, true)] public Color tookDamageColor = Color.red;
    [ColorUsage(true, true)] public Color healedColor = Color.green;
    [ColorUsage(true, true)] public Color healColor = Color.yellow;
    public float chipAwaySpeed = 1f;

    float lerpTimmer;
    float lastHealthfraction;


    private void Awake()
    {
        foreach (DisplayLayer layer in healthBarLayers)
        {
            layer.image.fillMethod = Image.FillMethod.Horizontal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBarUI();
    }
    void UpdateBarUI()
    {
        float fill_Health = healthBarLayers[0].image.fillAmount; // bar for health
        float fill_Change = healthBarLayers[1].image.fillAmount; // bar for chip away effect
        float fill_Heal = healthBarLayers[2].image.fillAmount; // bar for acumilated healing

        float healthFraction = healthSystem.currentHealth / healthSystem.maxHealth;

        if(healthFraction < lastHealthfraction) { lerpTimmer = 0; }

        //If took damge...
        if (fill_Change > healthFraction)
        {
            healthBarLayers[0].image.fillAmount = healthFraction;
            healthBarLayers[1].image.color = tookDamageColor;
            lerpTimmer += Time.deltaTime;
            float percentLerpCompleat = lerpTimmer / chipAwaySpeed;
            healthBarLayers[1].image.fillAmount = Mathf.Lerp(fill_Change, healthFraction, percentLerpCompleat);
        }
        lastHealthfraction = healthFraction;
    }

    //Sets maximum layers:
    private void OnValidate()
    {
        if(healthBarLayers.Count > 3)
        {
            healthBarLayers.RemoveRange(2, healthBarLayers.Count - 3);
        }
    }

}

[System.Serializable]
public class DisplayLayer
{
    public Image image;
}
