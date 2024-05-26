using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    public Texture2D mouseSprite;
    public Camera camera;
    public Rigidbody rb;
    public float speed;
    public float maxSpeed;
    public AnimationCurve accelerationCurve;
    public float accelerationTimeMultiplyer;
    public float drag;
    public bool isDead;
    [HideInInspector]
    public Vector3 worldPosition;

    Vector2 mousePos;
    Vector3 difernce;

    float accelerationElapsed = 0;
    float accelerationModedElapsed = 0;
    bool hasAccelerated = false;

    [Range(0, 1)]
    public float stearingMultiplyer;

    public bool setVelocityToZero = false;

    float magnitude;
    float Value;
    [HideInInspector] public float angle;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 hotSpot = new Vector2(mouseSprite.width / 2, mouseSprite.height / 2);
        Cursor.SetCursor(mouseSprite, hotSpot, CursorMode.Auto);
        
    }

    // Update is called once per frame
    void Update()
    {

        rb.drag = drag;
        mousePos = Input.mousePosition; // getting mouse position on screen
        worldPosition = camera.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
        difernce = worldPosition - transform.position; 
        angle = Mathf.Atan2(difernce.y, difernce.x); 
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // aplying rotation
       
    }
    public void FixedUpdate()
    {
        if (rb.velocity.magnitude < .1) { rb.velocity = Vector2.zero; }

        magnitude = rb.velocity.magnitude;
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        
        if(accelerationModedElapsed >= accelerationTimeMultiplyer) { hasAccelerated = true; } else { hasAccelerated = false; }

        if (magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 velocity;
            if(hasAccelerated) {
                if (magnitude > maxSpeed) { rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed); }  
                rb.AddRelativeForce(Vector2.right * speed); 
            }

            if (magnitude < maxSpeed)
            {
                while (accelerationModedElapsed < accelerationTimeMultiplyer)
                {
                    accelerationModedElapsed += Time.deltaTime;
                    accelerationElapsed = accelerationModedElapsed / accelerationTimeMultiplyer;
                    Value = accelerationCurve.Evaluate(accelerationElapsed);
                    rb.velocity = ((rb.velocity.normalized * (1 - stearingMultiplyer)) + (difernce.normalized * stearingMultiplyer)).normalized * Value * speed;
                    yield return null;
                }
            }
        }
        else { accelerationModedElapsed = EvaluateTime(accelerationCurve, magnitude / maxSpeed) * accelerationTimeMultiplyer;}


    }


    /// <summary>
    /// Inverse of Evaluate()
    /// </summary>
    /// <param name="curve">normalized AnimationCurve (time goes from 0 to 1)</param>
    /// <param name="value">setValue to search</param>
    /// <returns>time at which we have the closest setValue not exceeding it</returns>
    public float EvaluateTime(AnimationCurve curve, float value, int decimals = 6)
    {
        // Retrieve the closest decimal and then go down
        float time = 0.1f;
        float step = 0.1f;
        float evaluate = curve.Evaluate(time);
        while (decimals > 0)
        {
            // Loop until we pass our setValue
            while (evaluate < value)
            {
                time += step;
                evaluate = curve.Evaluate(time);
            }

            // Go one step back and increase precision of the step by one decimal
            time -= step;
            evaluate = curve.Evaluate(time);
            step /= 10f;
            decimals--;
        }

        return time;
    }
}
