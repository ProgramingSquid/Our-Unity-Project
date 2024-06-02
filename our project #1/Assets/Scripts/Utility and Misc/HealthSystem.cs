using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public RandomnessValue<float> maxHealth;
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
        SetUp();
        graceTimmer = graceTime;
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void SetUp()
    {

        currentHealth = maxHealth.RandonizeValue();
    }
    void Update()
    {
        graceTimmer -= Time.deltaTime;
    }
    
    public void TakeDamage(float damageAmount)
    {
        if(graceTimmer <= 0 && !Invincible)
        {
            currentHealth -= damageAmount;
            graceTimmer = graceTime;
        }
        if(renderer != null) { StartCoroutine(HitFlash(flashColor, flashDuration)); }
        
        HitLag.GetHitLag.HitLagEffect(hitLagDuration);

        if (currentHealth <= 0 )
        {
            OnDie.Invoke();
        }
        
    }

    
    
    public IEnumerator HitFlash(Color flashColor, float duration)
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
   


