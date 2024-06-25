using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movment Upgrade", menuName = "Upgrades/Movment Upgrade")]
public class MovmentUpgrade : Upgrade
{
    public float rotationSpeed = 3;
    public float verticleSpringStrangth;
    public float verticleDampining;
    public float raycastMaxDistance = 4;
    public AnimationCurve forceSpeedAccelerationCurve;
    public float acelerationSpeed;
    public float baseForceSpeed = 3f;
    public float maxForceSpeed = 6f;
    public float MaxSpeed = 10f;

    public static MovementControl movementControl;

    protected override void OnEqiuped()
    {
        base.OnEqiuped();
        movementControl = (MovementControl)GameObject.FindObjectOfType(typeof(MovementControl));
        movementControl.movment = this;
    }
}
