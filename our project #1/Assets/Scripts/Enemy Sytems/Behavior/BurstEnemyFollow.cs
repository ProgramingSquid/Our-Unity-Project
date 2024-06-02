using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstEnemyFollow : MonoBehaviour
{
    Transform target;
    Vector3 TargetPos;
    public bool CanMove = true;
    public Rigidbody rb;
    public float speed;
    public float drag;
    public float targetDisFormTarget;
    public float lookAngle = 25f;
    public float burstTime;
    public float burstTimmer;
    public float windUp;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        burstTimmer = burstTime;
    }

    // Update is called once per frame
    public void Update()
    {
        burstTimmer -= Time.deltaTime;
        TargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        float magnitude = (transform.position - TargetPos).magnitude;

        if (magnitude < targetDisFormTarget)
        {
            pulse(speed);
        }
        else if (magnitude > targetDisFormTarget)
        {
            pulse(-speed);
        }
        rb.drag = drag;
        Vector3 difernce = target.position - transform.position;
        float angle = Mathf.Atan2(difernce.z, difernce.x) * Mathf.Rad2Deg;
        if (angle >= lookAngle / 2 || angle <= -lookAngle / 2)
        {
            transform.rotation = Quaternion.Euler(90, -angle, 0);
        }
    }

    void pulse(float speed)
    {
        if(!CanMove) { return; }

        if(burstTimmer <= 0)
        {
            rb.drag = 0;
            rb.AddForce((transform.position - TargetPos).normalized * windUp, ForceMode.Impulse);
            rb.AddForce((transform.position - TargetPos).normalized * speed, ForceMode.Impulse);

            burstTimmer = burstTime;
        }
    }
    

}   

