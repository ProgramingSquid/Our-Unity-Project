using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

public class BiomeConditionPopUp : EditorWindow
{
    static BiomeConditionPopUp window;
    string Title = "BiomeQuestConditon";
    bool initializedPosition = false;

    [MenuItem("Assets/Create/Biomes/Biome Quests/Createn new Condition", false, 1)]
    public static void ShowWindow()
    {
        window = (BiomeConditionPopUp)GetWindow(typeof(BiomeConditionPopUp));
        window.minSize = new Vector2(300, 20);
        window.maxSize = window.minSize;
    }
    private void OnGUI()
    {



        if (initializedPosition == false)
        {
            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            position = new Rect(mousePos.x, mousePos.y, position.width, position.height);
            initializedPosition = true;
        }
        GUILayout.BeginHorizontal();

        Title = EditorGUILayout.TextField("Title:", Title);

        if (GUILayout.Button(">", GUILayout.Width(20)))
        {
            CreateWeaponAbilityScript();
            window.Close();
        }

        GUILayout.EndHorizontal();
    }

    void CreateWeaponAbilityScript()
    {
        string text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/BiomeQuests/BiomeQuestConditionTemplate.txt").text;
        string content = text.Replace("BiomeQuestCondition123", Title);
        AssetDatabase.CreateFolder("Assets/BiomeQuests/Conditions", Title);
        AssetDatabase.Refresh();
        File.WriteAllText("Assets/BiomeQuests/Conditions/" + Title + "/" + Title + "_Script" + ".cs", content);
        AssetDatabase.Refresh();
    }
}
