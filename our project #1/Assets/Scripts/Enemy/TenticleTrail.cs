using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenticleTrail : MonoBehaviour
{
    public int segmentAmount;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses;
    Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;

    private void Start()
    {
        lineRenderer.positionCount = segmentAmount;
        segmentPoses = new Vector3[segmentAmount];
        segmentV = new Vector3[segmentAmount];

    }
    private void Update()
    {
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
