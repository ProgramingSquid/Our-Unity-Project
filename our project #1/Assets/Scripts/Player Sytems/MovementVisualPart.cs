using UnityEngine;

public class MovementVisualPart : MonoBehaviour
{
    
    public MovementControl movementControl;
    public Transform LowYtransform;
    public Transform HighYtransform;
    [Space(10)]
    public float yTunningMax = -.6f;
    public float yTunningMin = -1;
    [Space(30)]
    public Transform MaxSpeedtransform;
    [Space(30)]
    public Transform MaxBrakeSpeedtransform;
    public Transform MinBrakeSpeedtransform;
    [Space(20)]
    public AnimationCurve HightToSpeedCurve;
    public float HightToSpeedReactionSpeed;
    [Space(10)]
    public AnimationCurve SpeedToHightCurve;
    public float SpeedToHightReactionSpeed;
    [Space(10)]
    public float velocitySpeedBalencing;
    [Space(30)]
    public bool debugColors;
    float SpeedAndHightT;



    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Vector3 thrustPosHight = HighYtransform.position;
        Quaternion thrustRotHight = HighYtransform.rotation;

        Vector3 thrustPosSpeed = HighYtransform.position;
        Quaternion thrustRotSpeed = HighYtransform.rotation;
        

        #region Verticle Transforms
        float T_hight = Remap(movementControl.transform.position.y, yTunningMin, yTunningMax, 0, 1);

        thrustPosHight.x = Mathf.Lerp(HighYtransform.position.x, LowYtransform.position.x, T_hight);
        thrustPosHight.y = Mathf.Lerp(HighYtransform.position.y, LowYtransform.position.y, T_hight);
        thrustPosHight.z = Mathf.Lerp(HighYtransform.position.z, LowYtransform.position.z, T_hight);

        thrustRotHight.x = Mathf.Lerp(HighYtransform.rotation.x, LowYtransform.rotation.x, T_hight);
        thrustRotHight.y = Mathf.Lerp(HighYtransform.rotation.y, LowYtransform.rotation.y, T_hight);
        thrustRotHight.z = Mathf.Lerp(HighYtransform.rotation.z, LowYtransform.rotation.z, T_hight);

        #endregion



        if (movementControl.isThrotleHeld)
        {
            float t;
            //speed over hight when slow but want to go fast.
            //hight over speed when going at max speed and want to go fast/ Input is held down
            t = 1 / movementControl.rb.velocity.magnitude * HightToSpeedCurve.Evaluate(movementControl.rb.velocity.magnitude / movementControl.MaxSpeed) * velocitySpeedBalencing;
            SpeedAndHightT = Mathf.Lerp(SpeedAndHightT, t, HightToSpeedReactionSpeed * Time.deltaTime);
        }

        else {SpeedAndHightT = Mathf.Lerp(SpeedAndHightT, 0, SpeedToHightCurve.Evaluate(SpeedToHightReactionSpeed * Time.deltaTime)); }

        transform.position = Vector3.Lerp(thrustPosHight, MaxSpeedtransform.position, SpeedAndHightT);
        transform.rotation = Quaternion.Lerp(thrustRotHight, MaxSpeedtransform.rotation, SpeedAndHightT);

        if (debugColors)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            renderer.material.color = Color.Lerp(Color.Lerp(Color.red, Color.blue, T_hight), Color.green, SpeedAndHightT);
        }
        
    }

    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    /* public IEnumerator StartUp()
     {

     } 
     public IEnumerator Runing()
     {

     } 
     public IEnumerator Stoped()
     {

     }*/

}
