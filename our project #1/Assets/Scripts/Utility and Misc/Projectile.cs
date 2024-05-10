using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 shootforceDir = Vector3.right;
    public float speed;
    public float damage;
    public float lifeTime = 5;
    float lifeTimmer = 5;

    private void OnEnable()
    {
        lifeTimmer = lifeTime;
    }
    
    void Update()
    {
        lifeTimmer -= Time.deltaTime;

        if(lifeTimmer <= 0)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
            lifeTimmer = lifeTime;
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = shootforceDir.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(damage);
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}