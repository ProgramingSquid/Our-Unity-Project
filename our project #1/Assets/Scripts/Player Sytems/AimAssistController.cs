using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IAimAssistable<T>
{
    public T GetAssistedValue(out T value, AimAssistController aimAssistController);
    //A interface that must be implemented on a class that has access to the value affected by aim assist 
}