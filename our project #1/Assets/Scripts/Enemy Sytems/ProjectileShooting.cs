using NaughtyAttributes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooting : MonoBehaviour, IEnemyBehaviorNode
{

    public GameObject projectile;
    [DisplayAsString] Transform target;
    [Tag] public string targetTag;
    public Vector3 offsetFromTransform;
    Vector3 difernce;
    float timer;
    public EnemyPamater<float> timeBetweenShots;
    public EnemyPamater<float> spreadAngle;
    public EnemyPamater<float> SwivleAngle;

    public BehaviorExitReturn behaviorExitReturn
    {
        get => nodeExitReturn;
        set => nodeExitReturn = value;
    }
    [SerializeField] BehaviorExitReturn nodeExitReturn;
    public BehaviorExitReturn previousbehaviorExitReturns
    {
        get => previousbehaviorExitReturns;
        set => previousbehaviorExitReturns = value;
    }
    
    [SerializeField] BehaviorExitReturn previousExitReturns;
    public bool exit 
    {
        get => exitNode;
        set => exitNode = value;
    }
    [SerializeField] bool exitNode;


    private void Shoot()
    {
        timer = timeBetweenShots.randomnessValue.value;
        float spread = Random.Range(-spreadAngle.randomnessValue.value / 2, spreadAngle.randomnessValue.value / 2);
        float SpreadY = transform.rotation.eulerAngles.y + spread;
        Vector3 dir = difernce;
        GameObject gameObject = ObjectPoolManager.spawnObject(projectile, transform.position, Quaternion.Euler(90, SpreadY, 0));
        gameObject.GetComponent<Projectile>().shootforceDir = dir;
        gameObject.transform.position = transform.position;
    }

    public void OnEnterBehavior()
    {
        timer = timeBetweenShots.randomnessValue.value;
        target = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Transform>();
    }

    public void BehaviorUpdate()
    {

        difernce = (target.position + offsetFromTransform) - transform.position;
        Debug.DrawRay(transform.position, difernce, Color.yellow);


        transform.LookAt(target.position + offsetFromTransform, -Vector3.forward);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Shoot();

        }
    }

    public void OnEnemySpawn()
    {
        timeBetweenShots.randomnessValue.RandonizeValue();
        spreadAngle.randomnessValue.RandonizeValue();
        SwivleAngle.randomnessValue.RandonizeValue();
    }
}
