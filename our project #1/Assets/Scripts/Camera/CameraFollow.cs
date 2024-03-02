using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    [Range(0,1)]
    public float smoothSpeed = .5f;
    public float MinSmoothSpeed = .1f;
    float currentSmothedSpeed;
    public float transitionSpeed;
    public Vector3 offset;
    public Vector2 screenBorderOffset;
    Vector3 smoothedPos;
    Vector2 screenBorders;
    Vector3 volocity;
    private void Start()
    {
        currentSmothedSpeed = smoothSpeed;
    }
    private void FixedUpdate()
    {


        if
        #region BoderConditions
            (
        #region X
            target.transform.position.x < screenBorders.x && target.transform.position.x > screenBorders.x &&
        #endregion
        #region Y
            target.transform.position.y < screenBorders.y && target.transform.position.y > screenBorders.y
        #endregion
           )
        #endregion
        {
            Debug.Log("Taget is off screen");
            currentSmothedSpeed = Mathf.Lerp(currentSmothedSpeed, MinSmoothSpeed, Time.deltaTime * transitionSpeed);
        }
        else 
        {
            currentSmothedSpeed = smoothSpeed;
        }

        Vector3 disieredPos = target.position + offset;
        disieredPos.y = 0;
        smoothedPos = Vector3.SmoothDamp(transform.position, disieredPos, ref volocity, currentSmothedSpeed, Mathf.Infinity, Time.deltaTime);
        transform.position = smoothedPos;
    }

}
