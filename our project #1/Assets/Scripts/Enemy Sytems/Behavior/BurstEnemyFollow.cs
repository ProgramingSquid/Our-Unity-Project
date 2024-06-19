using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstEnemyFollow : MonoBehaviour, IEnemyBehaviorNode
{
    [ShowInInspector, ReadOnly]Transform target;
    Vector3 TargetPos;
    public bool CanMove = true;
    public Rigidbody rb;
    public EnemyPamater<float> speed;
    public EnemyPamater<float> drag;
    public EnemyPamater<float> targetDisFormTarget;
    public EnemyPamater<float> lookAngle;
    public EnemyPamater<float> burstTime;
    public float burstTimmer;
    public EnemyPamater<float> windUp;

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

    void pulse(float speed)
    {
        if(!CanMove) { return; }

        if(burstTimmer <= 0)
        {
            rb.drag = 0;
            rb.AddForce((transform.position - TargetPos).normalized * windUp.randomnessValue.value, ForceMode.Impulse);
            rb.AddForce((transform.position - TargetPos).normalized * speed, ForceMode.Impulse);

            burstTimmer = burstTime.randomnessValue.value;
        }
    }

    public void OnEnterBehavior()
    {
        burstTimmer = burstTime.randomnessValue.value;
    }

    public void BehaviorUpdate()
    {
        burstTimmer -= Time.deltaTime;
        TargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        float magnitude = (transform.position - TargetPos).magnitude;

        if (magnitude < targetDisFormTarget.randomnessValue.value)
        {
            pulse(speed.randomnessValue.value);
        }
        else if (magnitude > targetDisFormTarget.randomnessValue.value)
        {
            pulse(-speed.randomnessValue.value);
        }
        rb.drag = drag.randomnessValue.value;
        Vector3 difernce = target.position - transform.position;
        float angle = Mathf.Atan2(difernce.z, difernce.x) * Mathf.Rad2Deg;
        if (angle >= lookAngle.randomnessValue.value / 2 || angle <= -lookAngle.randomnessValue.value / 2)
        {
            transform.rotation = Quaternion.Euler(90, -angle, 0);
        }
    }

    public void OnEnemySpawn()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed.randomnessValue.RandonizeValue();
        drag.randomnessValue.RandonizeValue();
        targetDisFormTarget.randomnessValue.RandonizeValue();
        lookAngle.randomnessValue.RandonizeValue();
        burstTime.randomnessValue.RandonizeValue();
    }
}   

