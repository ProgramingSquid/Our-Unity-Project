using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    [Range(0,1)]
    public float smoothSpeed = .5f;
    public float MinSmoothSpeed = .1f;
    [Space(15)]

    public GameObject zoomCameraObj;
    public float zoomOutDist;
    public float zoomInDist;
    float zoom;
    float zoomIn;
    float zoomOut;
    [Space(15)]
    [Tooltip("A bias to control camera's look ahead: 0 = target position and  1 = mouse position")]
    [Range(0, 1)] public float lookAheadBias = .5f;
    [Tooltip("A clamp to control how far the camera can look ahead, use  <= 0 for no clamp ")]
    public float lookAheadClampingDist;
    public Vector3 offset;

    Vector3 smoothedPos;
    Vector3 velocity;

    private void FixedUpdate()
    {
        
        float zoomLerp = MovementControl.player.rb.velocity.sqrMagnitude / Mathf.Pow(MovementControl.player.MaxSpeed, 2);


        Vector3 lookAhead = Vector3.Lerp(target.position, MovementControl.player.worldPosition, lookAheadBias);
        lookAhead.y = target.position.y;

        if (lookAheadClampingDist > 0 && Vector3.Distance(target.position, lookAhead) > lookAheadClampingDist)
        {
                Vector3 direction = (lookAhead - target.position).normalized;
                lookAhead = target.position + direction * lookAheadClampingDist;
        }


        zoom = Mathf.Lerp(-zoomOutDist, zoomInDist, zoomLerp);


        Vector3 disieredPos = lookAhead + offset;
        disieredPos.y = 0;
        smoothedPos = Vector3.SmoothDamp(transform.position, disieredPos, ref velocity, smoothSpeed, Mathf.Infinity, Time.deltaTime);
        transform.position = smoothedPos;

        Vector3 pos = zoomCameraObj.transform.localPosition;
        pos.z = Mathf.Lerp(pos.z, zoom, Time.deltaTime); 
        zoomCameraObj.transform.localPosition = pos;
        
    }

}
