using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    [Range(0,1)]
    public float smoothSpeed = .5f;
    public float MinSmoothSpeed = .1f;

    public GameObject zoomCameraObj;
    public float zoomOutDist;
    public float zoomInDist;
    float zoom;
    float zoomIn;
    float zoomOut;

    public Vector3 offset;

    Vector3 smoothedPos;
    Vector3 velocity;

    private void FixedUpdate()
    {
        
        float zoomLerp = MovementControl.player.rb.velocity.sqrMagnitude / Mathf.Pow(MovementControl.player.MaxSpeed, 2);
        

        zoom = Mathf.Lerp(-zoomOutDist, zoomInDist, zoomLerp);


        Vector3 disieredPos = target.position + offset;
        disieredPos.y = 0;
        smoothedPos = Vector3.SmoothDamp(transform.position, disieredPos, ref velocity, smoothSpeed, Mathf.Infinity, Time.deltaTime);
        transform.position = smoothedPos;

        Vector3 pos = zoomCameraObj.transform.localPosition;
        pos.z = Mathf.Lerp(pos.z, zoom, Time.deltaTime); 
        zoomCameraObj.transform.localPosition = pos;
        
    }

}
