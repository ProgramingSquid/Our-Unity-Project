using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooting : MonoBehaviour
{

    public GameObject projectile;
    public Transform target;
    public Vector3 offsetFromTransform;
    Vector3 difernce;
    float timer;
    public float timeBetweenShots = 0.5f;
    public float spreadAngle = 0;
    public float SwivleAngle = 25;
    


    // Start is called before the first frame update
    void Start()
    {
        timer = timeBetweenShots;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
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

    private void Shoot()
    {
        timer = timeBetweenShots;
        float spread = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        float SpreadY = transform.rotation.eulerAngles.y + spread;
        Vector3 dir = difernce;
        GameObject gameObject = ObjectPoolManager.spawnObject(projectile, transform.position, Quaternion.Euler(90, SpreadY, 0));
        gameObject.GetComponent<Projectile>().shootforceDir = dir;
        gameObject.transform.position = transform.position;
    }
}
