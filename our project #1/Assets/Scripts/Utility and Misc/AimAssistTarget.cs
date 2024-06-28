using NaughtyAttributes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AimAssistTarget : MonoBehaviour
{
    public float radius = 3;
    public float strangth;
    [CurveRange(0, -1, 1, 1, EColor.Orange)]
    public AnimationCurve falloffCurve;
    public Vector2 positionOffset;
    [Sirenix.OdinInspector.ReadOnly] public float assistValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //To do: move updating/calculating logic to AimAssistController
        assistValue = 0;
        var player = MovementControl.player;
        Ray ray = new Ray(player.transform.position, player.difernce2D);
        assistValue = CalculateAssistValue(ray);
        Debug.DrawRay(ray.origin, ray.direction.normalized * 15, Color.cyan);
    }
    public float CalculateAssistValue(Ray ray)
    {
        Vector3 postition = transform.position + (Vector3)positionOffset;
        float distance = DistanceFromPointToRay(postition, ray);
        float percent = (distance / radius);
        return (1 - percent) * strangth * falloffCurve.Evaluate(percent);
    }
    float DistanceFromPointToRay(Vector3 point, Ray ray)
    {
        // Calculate the vector from the ray origin to the point, ignoring the y component
        Vector3 originToPoint = new Vector3(point.x - ray.origin.x, 0, point.z - ray.origin.z);

        // Project the vector onto the ray direction to find the closest point, ignoring the y component
        Vector3 projection = Vector3.Dot(originToPoint, new Vector3(ray.direction.x, 0, ray.direction.z)) * new Vector3(ray.direction.x, 0, ray.direction.z);

        // Calculate the vector from the point to the closest point on the ray
        Vector3 pointToClosestPoint = projection - originToPoint;

        // Return the distance between the point and the closest point on the ray
        return pointToClosestPoint.magnitude;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,1,.5f);
        Gizmos.DrawSphere(transform.position + (Vector3)positionOffset, radius);
 
    }
}

