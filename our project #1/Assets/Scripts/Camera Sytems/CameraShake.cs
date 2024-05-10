using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform p;
    Vector3 vector3;
    public IEnumerator Shake(float duration, float magnatude, float recoveryTime, int type, Vector3 biasDir)
    {
        Vector3 startPos = transform.localPosition;

        float elapsed = 0f;
        Vector3 randPos = startPos;

        if(type == 0)
        {
            // Rumbling shake
            while (elapsed < duration)
            {
                float x1 = Random.Range(-100, 100);
                float x2 = Random.Range(-100, 100);
                float y1 = Random.Range(-100, 100);
                float y2 = Random.Range(-100, 100);
                randPos.x = Mathf.PerlinNoise(x1 + elapsed * 100, x2 + elapsed * 100) * magnatude;
                randPos.y = Mathf.PerlinNoise(y1 + elapsed * 100, y2 + elapsed * 100) * magnatude;
                randPos.z = 0;
                transform.localPosition = randPos;

                elapsed += Time.deltaTime;

                yield return null;
            }
            
            transform.localPosition = Vector2.Lerp(randPos, startPos, Time.deltaTime * recoveryTime);
        }

        if (type == 1)
        {
            // Shake one time in a circle
            transform.localPosition = Random.insideUnitCircle * magnatude;

            while (transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoveryTime);
                yield return 0;
            }
        }
        if (type == 2)
        {
            Ray ray = new Ray(transform.position, biasDir);
            vector3 = ray.GetPoint(magnatude);
            Debug.DrawLine(transform.position, vector3, Color.green, 1);
            transform.position = vector3;
            
            yield return new WaitForSeconds(duration);

            while(transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoveryTime);
                yield return 0;
            }
            

        }

    }
    private void Update()
    {
        
        Vector3 pos = transform.localPosition;
        pos.z = 0;
        transform.localPosition = pos;
        
    }

}
