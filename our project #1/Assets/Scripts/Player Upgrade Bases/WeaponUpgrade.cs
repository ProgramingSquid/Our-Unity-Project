using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "New Weapon Upgrade", menuName = "Upgrades/Weapon Upgrade")]
public class WeaponUpgrade : Upgrade
{
    [Space(30)]
    public float fireRate = 4;
    public float damage = 1;
    public float projectileSpeed;
    public float speadAngle;
    public float distance = 5;
    public bool isAutomatic = true;
    [HideInInspector] public float lifeTime;
    public float shakeDuration = .15f;
    public float shakeMagnitude = .25f;
    public float recoveryTime = .5f;
    public Projectile projectile;
    public static Weapon Weapon;
    [HideInInspector] public bool canShoot = false;

    protected override void OnEqiuped()
    {
        base.OnEqiuped();
        Weapon = (Weapon)GameObject.FindObjectOfType(typeof(Weapon));
        Weapon.weapon = this;
    }
    public virtual void Fire(InputAction.CallbackContext context)
    {
        
        if (isAutomatic)
        {
            if (context.started) { canShoot = true; }
            else if (context.canceled) { canShoot = false; }
        }
        else if (context.performed) { canShoot = true; }
        
    }
}
