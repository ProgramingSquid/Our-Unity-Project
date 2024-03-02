
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine.Events;
[System.Serializable]

public class MovementControl : MonoBehaviour
{
    [Expandable] public MovmentUpgrade movment;
    [Space(30)]
    public Camera camera;
    public Rigidbody rb;
    public KeyBinds inputActions;
    public bool isUsingController;
    [HideInInspector] public bool isThrotleHeld;
    bool throtolInput;
    [Space(30)]
    bool isDashing;
    bool dashingInput;
    public float dashSpeed = 14f;
    public float dashCooldown = 5f;
    public float dashCooldowTimmer;
    public float dashInvincibilityTime = .75f;
    public float dashInvincibilityTimmer = .75f;
    float drag;

    #region Feilds

    [HideInInspector] public float verticleSpringStrangth;
      [HideInInspector] public float verticleDampining;
      [HideInInspector] public float raycastMaxDistance = 4;

    [Space(30)]
    [HideInInspector] public AnimationCurve forceSpeedAccelerationCurve;
      [HideInInspector] public float acelerationSpeed;
      [HideInInspector] public float baseForceSpeed = 3f;
      [HideInInspector] public float maxForceSpeed = 6f;
      [HideInInspector] public float MaxSpeed = 10f;
        float forceSpeed;
        float elapsed = 0;


    [HideInInspector]
      public float lastHitDist;
    [HideInInspector] public Quaternion DesieredRot;
    [HideInInspector]
      public Vector3 difernce;
    [HideInInspector]
      public Vector2 current;
        Vector2 mousePos;
      [HideInInspector] public Vector3 worldPosition;
        Vector2 rotation;
    #endregion

    HealthSystem healthSystem;
    [Space(30)]
    [Foldout("Events")]
    public UnityEvent<float> OnTryDash;
    [Foldout("Events")]
    public UnityEvent<float> OnStartDash;
    [Foldout("Events")]
    public UnityEvent<float> OnDash;
    [Foldout("Events")]
    public UnityEvent<float> OnEndDash;
    

    private void Awake() 
    {
        inputActions = new KeyBinds();
        inputActions.Enable();
        inputActions.Player.Aim.performed += ctx => rotation = ctx.ReadValue<Vector2>();

        verticleSpringStrangth = movment.verticleSpringStrangth;
        verticleDampining = movment.verticleDampining;
        raycastMaxDistance = movment.raycastMaxDistance;
        forceSpeedAccelerationCurve = movment.forceSpeedAccelerationCurve;
        acelerationSpeed = movment.acelerationSpeed;
        baseForceSpeed = movment.baseForceSpeed;
        maxForceSpeed = movment.maxForceSpeed;
        MaxSpeed = movment.MaxSpeed;
        healthSystem = GetComponent<HealthSystem>();
        drag = rb.drag;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        #region Rotation
        mousePos = Mouse.current.position.ReadValue();

        
        Debug.DrawLine(transform.position, worldPosition);
        if (!isUsingController)
        {
            worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.farClipPlane));
            difernce = worldPosition - transform.position;
        }
        else
        {
            difernce = Vector3.right * rotation.x + Vector3.forward * rotation.y;
        }
        float angle = Mathf.Atan2(difernce.z, difernce.x);
        Debug.DrawRay(transform.position, difernce);
        DesieredRot = Quaternion.Euler(90, 0, angle * Mathf.Rad2Deg); // aplying rotation
        transform.rotation = DesieredRot;  
        #endregion

        #region Hovering
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Vector3.down, out hit, raycastMaxDistance))
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Vector3.down, Color.red);
            float forceAmount = HooksLawDamp(hit.distance);
            rb.AddForce(Vector3.up * forceAmount);
        }
        else
        {
            lastHitDist = raycastMaxDistance * 1.1f;
        }

        #endregion
        
        
        if(isThrotleHeld == true) { elapsed += acelerationSpeed * Time.deltaTime; }
        else { elapsed = 0; } 
        elapsed = Mathf.Clamp(elapsed, 0, 1);

        if (throtolInput == true)
        {
            forceSpeed = Mathf.Lerp(baseForceSpeed, maxForceSpeed, forceSpeedAccelerationCurve.Evaluate(elapsed));
            rb.AddRelativeForce(Vector3.right * forceSpeed);
            isThrotleHeld = true;
        }
        else {isThrotleHeld = false; }

        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        }
    }
    private void Update()
    {
        dashCooldowTimmer += Time.deltaTime;
        TryDash();
    }


    public float HooksLawDamp(float distance)
    {
        float force = verticleSpringStrangth * (-raycastMaxDistance - distance) + (verticleDampining * (lastHitDist - distance));
        force = Mathf.Max(0, force);
        lastHitDist = raycastMaxDistance;
        return force;
    }
    public void TryDash()
    {
        if (dashingInput)
        {
            
            
            if(dashCooldowTimmer >= dashCooldown)
            {
                OnStartDash.Invoke(0);
                StartCoroutine(Dash());
                dashCooldowTimmer = 0;
                
            }
            else { OnTryDash.Invoke(0); dashingInput = false; }
        }
    }
    public IEnumerator Dash()
    {
        rb.drag = 0;
        rb.AddRelativeForce(Vector3.right * dashSpeed * 1000);
        dashingInput = false;
        dashInvincibilityTimmer = dashInvincibilityTime;
        while (dashInvincibilityTimmer > 0)
        {
            OnDash.Invoke(0);
            healthSystem.Invincible = true;
            dashInvincibilityTimmer -= Time.deltaTime;
            yield return null;
        }
        if(dashInvincibilityTimmer <= 0) { healthSystem.Invincible = false; rb.drag = drag; OnEndDash.Invoke(0); }

    }
    public void SetThrotol(InputAction.CallbackContext context)  
    {
        if (context.started) { throtolInput = true; } 
        else if (context.canceled) { throtolInput = false; } 
    }
    public void SetDash(InputAction.CallbackContext context)
    {
        if (context.started) { dashingInput = true; }
       
    }
    public void OnDevieceChange(PlayerInput actions)
    {
        isUsingController = actions.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
