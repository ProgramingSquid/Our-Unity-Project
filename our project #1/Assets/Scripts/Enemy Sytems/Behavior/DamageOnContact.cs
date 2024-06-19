using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DamageOnContact : MonoBehaviour, IEnemyBehaviorNode
{
    public EnemyPamater<float> damage;
    [Tag] public string tagFilter;

    public BehaviorExitReturn behaviorExitReturn
    {
        get => nodeExitReturn; set => nodeExitReturn = value;
    }
    [SerializeField] BehaviorExitReturn nodeExitReturn;
    public BehaviorExitReturn previousbehaviorExitReturns
    {
        get => previousExitReturns;
        set => previousExitReturns = value;
    }
    [SerializeField] BehaviorExitReturn previousExitReturns;
    public bool exit
    {
        get => exitNode;
        set => exitNode = value;
    }
    [SerializeField] bool exitNode;

    public void BehaviorUpdate()
    {
        
    }

    public void OnEnemySpawn()
    {
        damage.randomnessValue.RandonizeValue();
    }

    public void OnEnterBehavior()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(tagFilter))
        {

            if (collider.TryGetComponent<HealthSystem>(out HealthSystem system) == true)
            {
                collider.GetComponent<HealthSystem>().TakeDamage(damage.randomnessValue.value);
            }
        }
    }
}
