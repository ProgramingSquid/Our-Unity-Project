using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Debugger : MonoBehaviour
{
    
    public int targetFPS;
    public KeyCode startStopwatchKey;
    public KeyCode restetStopwatchKey;
    float stopwatch;
    //public  UpgradeEdittorWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFPS != 0) { Application.targetFrameRate = targetFPS; }
        if (Input.GetKeyDown(startStopwatchKey)) { StartCoroutine(StopWatch()); }
        if (Input.GetKeyDown(restetStopwatchKey)) { stopwatch = 0; }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //UpgradeEdittorWindow
        }
    }

    IEnumerator StopWatch()
    {
        
        bool isRunning = false;

        isRunning = !isRunning;

        while(isRunning == true) { stopwatch += Time.deltaTime; yield return null; if (Input.GetKeyDown(startStopwatchKey)) { isRunning = false; } }
        if(isRunning == false) { Debug.Log(stopwatch); stopwatch = 0; }
    }
}
