using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyRoundManager))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyRoundManager manager = (EnemyRoundManager)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Enemies"))
        {
            Debug.Log("Not Implemented Yet");
            
        }
    }
}
