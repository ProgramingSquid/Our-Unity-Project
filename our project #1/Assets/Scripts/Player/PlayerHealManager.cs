using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealManager : MonoBehaviour
{
    public float healHealth = 0;
    public HealthSystem healthSystem;
    public static PlayerHealManager instance;
    public void Heal()
    {
        if(healthSystem.currentHealth < 0) { return; }
        healthSystem.currentHealth = Mathf.Clamp(healthSystem.currentHealth + healHealth, 0, healthSystem.maxHealth);
        healHealth = 0;
    }

    public void AddHeal(float healAmount)
    {
        healHealth += healAmount;
        healthSystem.HitFlash(Color.green, .01f);
    }
    private void Start()
    {
        instance = this;
    }

}
