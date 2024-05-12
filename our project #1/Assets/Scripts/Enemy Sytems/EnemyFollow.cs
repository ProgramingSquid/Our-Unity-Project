using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Enemy))]
public class EnemyFollow : MonoBehaviour, IEnemyBehaviorNode
{

    Transform target;
    public EnemyPamater<float> speed;
    public EnemyPamater<float> targetDisFormTarget;
    public EnemyPamater<float> lookAngle/* = 25f*/;

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

    public void OnEnterBehavior()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void BehaviorUpdate()
    {
        Vector3 TargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        if ((transform.position - TargetPos).sqrMagnitude > targetDisFormTarget.value)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed.value * Time.deltaTime);
        }
        else if ((transform.position - TargetPos).sqrMagnitude < targetDisFormTarget.value)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, -speed.value * Time.deltaTime);
        }

        Vector3 difernce = target.position - transform.position;
        float angle = Mathf.Atan2(difernce.z, difernce.x) * Mathf.Rad2Deg;
        if (angle >= lookAngle.value / 2 || angle <= -lookAngle.value / 2)
        {
            transform.rotation = Quaternion.Euler(90, -angle, 0);
        }
    }
}
