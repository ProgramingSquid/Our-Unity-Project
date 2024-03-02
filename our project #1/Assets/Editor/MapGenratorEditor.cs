using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapGenerator))]
public class MapGenratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator MapGenerator = (MapGenerator)target;
        base.OnInspectorGUI();

        if(GUILayout.Button("Genrate")) { MapGenerator.GenerateStartValues(); }
        
    }
}
