using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DentedPixel;

public class HealthDisplay : MonoBehaviour
{
    
    [Foldout("Layers")] public Image healthBar;
    [Foldout("Layers")] public Image changeHealthBar;
    [Foldout("Layers")] public Image healHealthBar;
    
    public HealthSystem healthSystem;
    public PlayerHealManager healManager;
    [Space(10)]
    
    [ColorUsage(true, true), BoxGroup("Layer Visuals")] public Color tookDamageColor = Color.red;
    [BoxGroup("Layer Visuals")] public LeanTweenType tookDamageTween;

    [ColorUsage(true, true), BoxGroup("Layer Visuals")] public Color healedColor = Color.green;
    [BoxGroup("Layer Visuals")] public LeanTweenType healedTween;

    [ColorUsage(true, true), BoxGroup("Layer Visuals")] public Color healColor = Color.yellow;
    [BoxGroup("Layer Visuals")] public LeanTweenType healTween;

    public float chipAwaySpeed = 1f;




    private void Awake()
    {
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        changeHealthBar.fillMethod = Image.FillMethod.Horizontal;
        healHealthBar.fillMethod = Image.FillMethod.Horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBarUI();
    }
    void UpdateBarUI()
    {
        float fill_Health = healthBar.fillAmount; // bar for health
        float fill_Change = changeHealthBar.fillAmount; // bar for chip away effect
        float fill_Heal = healHealthBar.fillAmount; // bar for acumilated healing

        float healthFraction = healthSystem.currentHealth / healthSystem.maxHealth.value;
        float healFraction = (healthSystem.currentHealth + healManager.healHealth) / healthSystem.maxHealth.value;

        
        if(fill_Change > healthFraction)
        {
            healthBar.fillAmount = healthFraction;
            changeHealthBar.color = tookDamageColor;

            LeanTween.value(changeHealthBar.gameObject, fill_Change, healthFraction, chipAwaySpeed)
                .setEase(tookDamageTween)
                .setOnUpdate((value) =>
                {
                    changeHealthBar.fillAmount = value;
                });
        }

        if(fill_Health < healthFraction)
        {
            changeHealthBar.fillAmount = healthFraction;
            changeHealthBar.color = healedColor;

            LeanTween.value(changeHealthBar.gameObject, fill_Health, changeHealthBar.fillAmount, chipAwaySpeed)
                .setEase(healedTween)
                .setOnUpdate((value) =>
                {
                    changeHealthBar.fillAmount = value;
                });
        }

        if (healFraction > healthFraction)
        {

            LeanTween.value(healHealthBar.gameObject, fill_Heal, healFraction, chipAwaySpeed)
                .setEase(healTween)
                .setOnUpdate((value) =>
                {
                    healHealthBar.fillAmount = value;
                });
        }
        else { healHealthBar.fillAmount = healthFraction; }

    }

}

