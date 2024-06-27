using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
public class Upgrade : ScriptableObject
{

    public string titel;
    [Multiline] public string discription;
    [Space(40)]
    public bool isEquiped;


    protected virtual void OnEqiuped()
    {
        isEquiped = true;

    }
}
