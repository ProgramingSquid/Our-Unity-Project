using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class Weapon : MonoBehaviour
{
    [Expandable] public WeaponUpgrade weapon;
    public float timer = 0;
    public CameraShake CameraShake;
    public GameObject player;
    MovementControl PlayerMovment;
    float timeBetweenShots;
    Ray ray;
    Vector3 dir;
    Vector3 pos;

    private void Start()
    {
        PlayerMovment = player.GetComponent<MovementControl>();
        timeBetweenShots = 1 / weapon.fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        weapon.lifeTime = weapon.distance / weapon.projectileSpeed;
        weapon.projectile.damage = weapon.damage;
        weapon.projectile.speed = weapon.projectileSpeed;
        weapon.projectile.lifeTime = weapon.lifeTime;

        timer -= Time.deltaTime;
        ray = new Ray(player.transform.position, (PlayerMovment.worldPosition - player.transform.position).normalized);
        pos = ray.GetPoint(1);

        dir = player.transform.position - pos;
        TryShoot();
    }

    public void TryShoot()
    {
        if (timer <= 0)
        {
            if (weapon.canShoot) 
            {
                timer = timeBetweenShots;
                float spread = Random.Range(-weapon.speadAngle / 2, weapon.speadAngle / 2);
                float angle = transform.rotation.eulerAngles.z + spread;
                GameObject gameObject = ObjectPoolManager.spawnObject(weapon.projectile.gameObject, transform.position, Quaternion.Euler(90, angle, 0));
                gameObject.transform.position = transform.position;
                gameObject.GetComponent<Projectile>().shootforceDir = new Vector3(PlayerMovment.difernce.x, 0, PlayerMovment.difernce.z);
                StartCoroutine(CameraShake.Shake(weapon.shakeDuration, weapon.shakeMagnitude, weapon.recoveryTime, 2, dir));
                if (!weapon.isAutomatic) { weapon.canShoot = false; }
            }
        }
    }
    public void FireInput(InputAction.CallbackContext context) { weapon.Fire(context); }
}
