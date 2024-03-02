using System.IO;
using UnityEditor;
using UnityEngine;


public class WeaponAbilityPopUp : EditorWindow
{
    static WeaponAbilityPopUp window;
    string Title = "WeaponAbility";
    bool initializedPosition = false;

    [MenuItem("Assets/Create/Weapon Ability Script", false, 1)]
    public static void ShowWindow()
    {
        window = (WeaponAbilityPopUp)GetWindow(typeof(WeaponAbilityPopUp));
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
        string text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WeaponAbilityTemplate.txt").text;
        string content = text.Replace("Upgrade123", Title);
        AssetDatabase.CreateFolder("Assets/UpgradeData/WEAPONS/Abilities", Title);
        AssetDatabase.Refresh();
        File.WriteAllText("Assets/UpgradeData/WEAPONS/Abilities/" + Title + "/" + Title + "_Script" + ".cs", content);
        AssetDatabase.Refresh();
    }



}
