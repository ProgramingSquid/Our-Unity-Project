using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AimAssistController : MonoBehaviour
{
    public IAimAssistable aim;
    public float range;
    [Range(0,1)] public float wieght;
    public bool enableAssist;
    List<AimAssistTarget> targets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get all targets in range and add them to list: targets.
        //Calculate assit value for each target.
        //Calculate a ray that points in the average direction of all targets weighted by thier assistValue.
        //Calculate a new ray with a direction from a average of the previous ray and the ray retured by GetInput()
        //wighted by wieght.
        //Call SetAim() passing in the final calculated ray.
    }
}
public interface IAimAssistable
{
    public Ray GetInput();
    public void SetAim(Ray assist);
}