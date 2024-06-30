using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AimAssistController : MonoBehaviour
{
    //To fix. not properly serilized in the edittor
    public UnityEngine.Object aimAssist;
    [ReadOnly] public IAimAssistable aim;
    public float range;
    [Range(0,1)] public float wieght;
    public bool enableAssist;
    List<AimAssistTarget> targets = new();
    [HideInInspector] public Ray assist { get; private set; }

    private void OnValidate()
    {
        if(aimAssist is IAimAssistable assistable)
        {
            aim = assistable;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Ray inputRay = aim.GetInput();
        Vector3 sum = Vector3.zero;
        float totalWeight = 0;

        //Get all targets in range and add them to list: targets.
        targets = GetTargets();

        foreach (var target in targets)
        {
            //Calculate assit value for each target.
            target.assistValue = target.CalculateAssistValue(inputRay);
            //Calculate a inputRay that points in the average direction of all targets weighted by thier assistValue.
            Vector3 dir = target.transform.position - inputRay.origin;
            Debug.DrawRay(inputRay.origin, dir, Color.yellow + new Color(0, targets.Count - (targets.IndexOf(target) / 10),0));
            sum += dir * target.assistValue;
            totalWeight += target.assistValue;
        }
        var avg = sum / totalWeight;
        //Calculate a new inputRay with a direction from a average of the previous inputRay and the inputRay retured by GetInput()
        Vector3 assitDir = Vector3.Lerp(inputRay.direction.normalized, avg.normalized, wieght);

        //Call SetAim() passing in the final calculated inputRay
        Debug.Log(totalWeight);

        if(totalWeight == 0) { assitDir = inputRay.direction; }
        assist = new Ray(inputRay.origin, assitDir.normalized);
        aim.SetAim(assist);
        Debug.DrawRay(inputRay.origin, avg.normalized * 15, Color.cyan);
        Debug.DrawRay(assist.origin, assitDir.normalized * 15, Color.green);
    }

    private List<AimAssistTarget> GetTargets()
    {
        var collidersInRange = Physics.OverlapSphere(transform.position, range);
        var targets = new List<AimAssistTarget>();
        targets.Clear();
        foreach (var collider in collidersInRange)
        {
            if(collider.TryGetComponent(out AimAssistTarget target))
            {
                targets.Add(target);
            }
        }
        return targets;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, .75f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
public interface IAimAssistable
{
    public Transform transform { get; }
    public Ray GetInput();
    public void SetAim(Ray assist);
}