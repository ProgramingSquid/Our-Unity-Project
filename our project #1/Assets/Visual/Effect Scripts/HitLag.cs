using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLag : MonoBehaviour
{
    public static HitLag GetHitLag;
    bool isWaiting = false;
    public void HitLagEffect(float duration)
    {
        if (isWaiting == false)
        {
            Time.timeScale = 0;
            StartCoroutine(Wait(duration));
        }



    }

    IEnumerator Wait(float duration)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        isWaiting = false;
    }

    private void Awake()
    {
        GetHitLag = this;
    }
}
