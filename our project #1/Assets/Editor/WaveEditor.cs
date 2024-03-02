using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveGenerator))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaveGenerator generator = (WaveGenerator)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Enemies"))
        {
            generator.spawnEnemies();
        }
    }
}
