using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoundManager))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoundManager generator = (RoundManager)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Enemies"))
        {
            generator.spawnEnemies();
        }
    }
}
