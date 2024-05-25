using NaughtyAttributes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour, IEnemyBehaviorNode
{

    [DisplayAsString] public Transform target;
    [Tag] public string targetTag;
    public EnemyPamater<float> speed;
    public EnemyPamater<float> targetDisFormTarget;
    public EnemyPamater<float> lookAngle;
    public Vector3 rotationOffset;

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

    public bool drawDebugGizmos = false;

    public void OnEnterBehavior()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Transform>();
    }

    private void OnValidate()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Transform>();
    }

    public void BehaviorUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        if ((transform.position - targetPos).sqrMagnitude > Mathf.Pow(targetDisFormTarget.value, 2))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed.value * Time.deltaTime);
        }
        else if ((transform.position - targetPos).sqrMagnitude < Mathf.Pow(targetDisFormTarget.value, 2))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, -speed.value * Time.deltaTime);
        }
        Vector3 diference =  transform.position - target.position;

        float angle = Mathf.Atan2(-diference.x, -diference.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(rotationOffset.x, angle + rotationOffset.y, rotationOffset.z);
    }

    private void OnDrawGizmos()
    {
        if(drawDebugGizmos) 
        {
            Vector3 diference = transform.position - target.position;
        
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, diference);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, new Vector3(diference.x, 0, 0));
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, new Vector3(0, diference.y, 0));
        
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, new Vector3(0, 0, diference.z));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .9f, .9f, .8f);
        Gizmos.DrawWireSphere(target.position, targetDisFormTarget.value);
    }
}
