using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YlockFollow : MonoBehaviour
{
    public Transform followTransform;

    private void FixedUpdate()
    {
        Vector3 tempPos = followTransform.position;
        tempPos.y = transform.position.y;
        transform.position = tempPos;
    }
}
