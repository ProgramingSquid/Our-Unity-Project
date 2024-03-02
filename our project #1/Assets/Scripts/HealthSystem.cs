using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth = 10;
    public float graceTime = 0;
    float graceTimmer;
    public new SpriteRenderer renderer;
    [ColorUsage(true, true)]
    public Color flashColor = Color.white;
    public float flashDuration = .2f;
    public AnimationCurve flashSpeedCurve;
    public float hitLagDuration = .01f;
    public bool Invincible;
    public UnityEvent OnDie;
    


    private void Start()
    {
        graceTimmer = graceTime;
        SetupHealth();
        currentHealth = maxHealth;
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void SetupHealth()
    {
        if (gameObject.tag == "Enemy")
        {
            maxHealth = GetComponent<Enemy>().type.health;
        }
        
    }

    void Update()
    {
        
        graceTimmer -= Time.deltaTime;
        SetupHealth();
    }
    
    public void TakeDamage(float damageAmount)
    {
        if(graceTimmer <= 0 && !Invincible)
        {
            currentHealth -= damageAmount;
            graceTimmer = graceTime;
        }
        if(renderer != null) { StartCoroutine(HitFlash(flashColor, flashDuration)); }
        
        FindObjectOfType<HitLag>().HitLagEffect(hitLagDuration);

        if (currentHealth <= 0 )
        {
            OnDie.Invoke();
        }
        
    }

    
    
    IEnumerator HitFlash(Color flashColor, float duration)
    {
        renderer.material.SetColor("_FlashColor", flashColor);
        float elapsed =  0;
        float currrentAmount = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                currrentAmount = Mathf.Lerp(1, flashSpeedCurve.Evaluate(elapsed), (elapsed / flashDuration));
                renderer.material.SetFloat("_FlashAmount", currrentAmount);
                yield return null;

            }

        }
    }
   


