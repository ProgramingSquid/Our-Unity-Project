using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Enemy))]
public class EnemyFollow : MonoBehaviour
{

    Transform target;
    public float speed;
    public float targetDisFormTarget;
    public float lookAngle = 25f;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    public void Update()
    {

        Vector3 TargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        if ((transform.position - TargetPos).sqrMagnitude > targetDisFormTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        }
        else if ((transform.position - TargetPos).sqrMagnitude < targetDisFormTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, -speed * Time.deltaTime);
        }

        Vector3 difernce = target.position - transform.position;
        float angle = Mathf.Atan2(difernce.z, difernce.x) * Mathf.Rad2Deg;
        if(angle >= lookAngle / 2 || angle <= -lookAngle /2)
        {
            transform.rotation = Quaternion.Euler(90, -angle, 0);
        }
    }

}
