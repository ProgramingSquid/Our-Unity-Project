using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DamageOnContact : MonoBehaviour
{
    public float damage;
    [Tag] public string tag;

    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.CompareTag(tag))
        {

            if (collider.TryGetComponent<HealthSystem>(out HealthSystem system) == true)
            {
                collider.GetComponent<HealthSystem>().TakeDamage(damage);
            }
        }
    }
}
