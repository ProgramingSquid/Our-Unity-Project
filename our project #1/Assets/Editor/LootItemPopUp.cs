using System.IO;
using UnityEditor;
using UnityEngine;


public class LootItemPopUp : EditorWindow
{
    static LootItemPopUp window;
    string Title = "LootItem";
    [Tooltip("If false this loot will not be collected atomaticaly")]bool isAutomatic = true;
    bool initializedPosition = false;
    [MenuItem("Assets/Create/Loot Item Script", false, 1)]
    public static void ShowWindow()
    {
        window = (LootItemPopUp)GetWindow(typeof(LootItemPopUp));
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

            EditorGUILayout.LabelField("Title:", GUILayout.Width(35));
            Title = EditorGUILayout.TextField(Title, GUILayout.MaxWidth(150));
            EditorGUILayout.LabelField("Is Automatic:", GUILayout.Width(75));
            isAutomatic = EditorGUILayout.Toggle(isAutomatic, GUILayout.MaxWidth(150));


            if (GUILayout.Button(">", GUILayout.Width(20)))
            {
                CreateLootItemScript();
                window.Close();
            }

        GUILayout.EndHorizontal();

    }
    
    void CreateLootItemScript()
    {
        string text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Loot/LootItemTemplate.txt").text;
        string content = text.Replace("LOOTNAME", Title);
        AssetDatabase.CreateFolder("Assets/Loot", Title);
        AssetDatabase.Refresh();
        File.WriteAllText("Assets/Loot/" + Title + "/" + Title + "_Script" + ".cs", content);
        AssetDatabase.Refresh();
    }



}
