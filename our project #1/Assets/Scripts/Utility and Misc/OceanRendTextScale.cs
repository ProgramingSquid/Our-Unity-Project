using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanRendTextScale : MonoBehaviour
{
    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        // Scale object from camera plane at object pos
        float distance = Vector3.Distance(camera.transform.position, gameObject.transform.position);
        float frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector3 scale = gameObject.transform.localScale;
        scale.y = frustumHeight;
        scale.x = frustumHeight * camera.aspect;

        gameObject.transform.localScale = scale;

    }
}
