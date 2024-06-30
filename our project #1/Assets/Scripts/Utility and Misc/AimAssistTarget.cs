using NaughtyAttributes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistTarget : MonoBehaviour
{
    public float radius = 3;
    public float strangth;
    [CurveRange(0, -1, 1, 1, EColor.Orange)]
    public AnimationCurve falloffCurve;
    public Vector2 positionOffset;
    [Sirenix.OdinInspector.ReadOnly] public float assistValue;
    [Sirenix.OdinInspector.ReadOnly] public float percent;
    public float CalculateAssistValue(Ray ray)
    {
        Vector3 postition = transform.position + (Vector3)positionOffset;
        float distance = DistanceFromPointToRay(postition, ray);
        percent = Mathf.Clamp(distance / radius, 0,1);
        return (1 - percent) * strangth * falloffCurve.Evaluate(percent);
    }
    float DistanceFromPointToRay(Vector3 point, Ray ray)
    {
        //To fix...

        // Calculate the vector from the ray origin to the point, ignoring the y component
        Vector3 originToPoint = new Vector3(point.x - ray.origin.x, 0, point.z - ray.origin.z);
        Debug.DrawRay(point, originToPoint, Color.red);
        // Project the vector onto the ray direction to find the closest point, ignoring the y component
        Vector3 projection = Vector3.Dot(originToPoint, new Vector3(ray.direction.x, 0, ray.direction.z)) * new Vector3(ray.direction.x, 0, ray.direction.z);

        // Calculate the vector from the point to the closest point on the ray
        Vector3 pointToClosestPoint = projection - originToPoint;
        Debug.DrawRay(point, pointToClosestPoint, Color.magenta);
        // Return the distance between the point and the closest point on the ray
        return pointToClosestPoint.magnitude;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,1,.5f);
        Gizmos.DrawSphere(transform.position + (Vector3)positionOffset, radius);
 
    }
}

