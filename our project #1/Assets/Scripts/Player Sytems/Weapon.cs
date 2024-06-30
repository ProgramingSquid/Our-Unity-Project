using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Weapon : MonoBehaviour, IAimAssistable
{
    [InlineEditor] public WeaponUpgrade weapon;
    public float timer = 0;
    public CameraShake CameraShake;
    public GameObject player;
    MovementControl playerMovment;
    float timeBetweenShots;
    Quaternion DesieredRot;

    Transform IAimAssistable.transform { get => transform; }

    private void Start()
    {
        playerMovment = player.GetComponent<MovementControl>();
        timeBetweenShots = 1 / weapon.fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        weapon.lifeTime = weapon.distance / weapon.projectileSpeed;
        weapon.projectile.damage = weapon.damage;
        weapon.projectile.speed = weapon.projectileSpeed;
        weapon.projectile.lifeTime = weapon.lifeTime;

      
        transform.rotation = Quaternion.Slerp(transform.rotation, DesieredRot,weapon.turretSpeed * Time.deltaTime);

        timer -= Time.deltaTime;
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

                Vector3 dir = Quaternion.Euler(0, spread, 0) * transform.right;
                gameObject.GetComponent<Projectile>().shootforceDir = dir;
                StartCoroutine(CameraShake.Shake(weapon.shakeDuration, weapon.shakeMagnitude, weapon.recoveryTime, 2, -dir));
                
                if (!weapon.isAutomatic) { weapon.canShoot = false; }
            }
        }
    }
    public void FireInput(InputAction.CallbackContext context) { weapon.Fire(context); }

    public Ray GetInput()
    {
        var player = MovementControl.player;
        return new Ray(player.transform.position, player.difernce2D);
    }

    public void SetAim(Ray assist)
    {
        var angle = Mathf.Atan2(assist.direction.z, assist.direction.x);
        Debug.DrawRay(transform.position, assist.direction);
        DesieredRot = Quaternion.Euler(90, 0, angle * Mathf.Rad2Deg); // aplying rotation
    }
}
