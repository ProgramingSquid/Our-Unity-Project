using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Codice.CM.Client.Differences.Graphic;
using System.IO;
using static UnityEditor.Rendering.InspectorCurveEditor;
using Unity.Plastic.Antlr3.Runtime.Tree;

public class EnemyEditor : OdinMenuEditorWindow
{
    [MenuItem("Windows/GameEditors/EnemyEditor")]
    public static void ShowWindow()
    {
        GetWindow<EnemyEditor>("EnemyEditors");
    }

    protected override void OnDestroy()
    {
        if (enemyCreationGUI != null) { DestroyImmediate(enemyCreationGUI.enemyData); }
        if (subEnemyCreateGUI != null) { DestroyImmediate(subEnemyCreateGUI.enemyData); }
    }

    protected override void OnBeginDrawEditors() 
    {
        var selection = MenuTree.Selection;
        SirenixEditorGUI.BeginHorizontalToolbar();
        if (selection.SelectedValue is EnemyTypeSO)
        {
            var selectedEnemyData = selection.SelectedValue as EnemyTypeSO;
            if (selectedEnemyData.hasSubTypes)
            {
                if (SirenixEditorGUI.ToolbarButton("Create New Sub Type Of Selected"))
                {
                    subEnemyCreateGUI.baseEnemy = selectedEnemyData;
                    MenuTree.Selection.Clear();
                    MenuTree.Selection.Add(MenuTree.GetMenuItem("Create New Sub Enemy"));
                }
            }

            if (selectedEnemyData.hasDifficultyVeriants)
            {
                if (SirenixEditorGUI.ToolbarButton("Create New Difficulty Veriant Of Selected"))
                {
                    veriantEnemyCreateGUI.baseEnemy = selectedEnemyData;
                    MenuTree.Selection.Clear();
                    MenuTree.Selection.Add(MenuTree.GetMenuItem("Create New Difficulty Veriant Enemy"));
                }
            }


            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.ArrowUpSquare))
            {
                while (!MenuTree.Selection[0].SmartName.Contains("_base"))
                {
                    if (!MenuTree.Selection[0].ChildMenuItems.Any(child => child.SmartName.Contains("_base")))
                    {
                        var path = MenuTree.Selection[0].Parent.GetFullPath();
                        MenuTree.Selection.Clear();
                        MenuTree.Selection.Add(MenuTree.GetMenuItem(path));
                        continue;
                    }

                    foreach (var child in MenuTree.Selection[0].ChildMenuItems)
                    {
                        if (!child.SmartName.Contains("_base")) { continue; }
                        MenuTree.Selection.Clear();
                        MenuTree.Selection.Add(child);
                        return;
                    }
                }
            }

            if (SirenixEditorGUI.ToolbarButton(SdfIconType.Trash))
            {
                bool confirmed = EditorUtility.DisplayDialog("Confirm Deletion",
                    "Are you sure you want to delete this selected Enemy Data? This action cannot be undone.", "Delete", "Cancel");

                if (!confirmed) { return; }
                var assetPath = AssetDatabase.GetAssetPath(selectedEnemyData);
                string path = AssetUtilities.GetAssetFolder(assetPath);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
            

        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    EnemyCreateGUI enemyCreationGUI;
    SubEnemyCreateGUI subEnemyCreateGUI;
    VeriantEnemyCreateGUI veriantEnemyCreateGUI;
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree;
        tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = true;

        enemyCreationGUI = new EnemyCreateGUI();
        subEnemyCreateGUI = new SubEnemyCreateGUI();
        veriantEnemyCreateGUI = new VeriantEnemyCreateGUI();
        var enemyTypePath = "Assets/Game Items/Enemy";
        tree.Add("Create New Base Enemy", enemyCreationGUI);
        tree.Add("Create New Sub Enemy", subEnemyCreateGUI);
        tree.Add("Create New Difficulty Veriant Enemy", veriantEnemyCreateGUI);

        tree.AddAllAssetsAtPath("Enemy Data", enemyTypePath, typeof(EnemyTypeSO), true, false);
        return tree;


    }
    class EnemyCreateGUI
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public EnemyTypeSO enemyData;
        public EnemyCreateGUI()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.SetObjectName();
        }
        [Button("Create")]
        public void Create()
        {
            string GUIID = AssetDatabase.CreateFolder("Assets/Game Items/Enemy", enemyData.name.Replace("_base", string.Empty));
            string folder = AssetDatabase.GUIDToAssetPath(GUIID);
            string path = folder;
            AssetDatabase.CreateFolder(path, "Stats");
            AssetDatabase.CreateFolder(path, "Sub Types");
            AssetDatabase.CreateFolder(path, "Difficulty Verients");
            AssetDatabase.CreateAsset(enemyData, path + "/" + enemyData.name + ".asset");
            AssetDatabase.SaveAssets();

            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.SetObjectName();
        }

        
        
    } 
    class SubEnemyCreateGUI
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public EnemyTypeSO enemyData;
        public EnemyTypeSO baseEnemy;
        public SubEnemyCreateGUI()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.hasSubTypes = false;
            enemyData.SetObjectName();
        }

        [Button("Create")]
        public void CreateSubEnemy()
        {
            baseEnemy.SubTypes.Add(enemyData);
            baseEnemy.ValidateSubTypes();
            enemyData.SetObjectName();

            string assetPath = AssetDatabase.GetAssetPath(baseEnemy);
            string folder = System.IO.Path.GetDirectoryName(assetPath);

            string GUID = AssetDatabase.CreateFolder(folder + "/" + "Sub Types", enemyData.name.Replace("_subType", string.Empty));
            string path = AssetDatabase.GUIDToAssetPath(GUID);
            AssetDatabase.CreateFolder(path, "Stats");
            AssetDatabase.CreateFolder(path, "Difficulty Verients");
            AssetDatabase.CreateAsset(enemyData, path + "/" + enemyData.name + ".asset");
            AssetDatabase.SaveAssets();

            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.hasSubTypes = false;
            enemyData.SetObjectName();
        }
    }    
    class VeriantEnemyCreateGUI
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public EnemyTypeSO enemyData;
        public EnemyTypeSO baseEnemy;
        public VeriantEnemyCreateGUI()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.hasSubTypes = false;
            enemyData.hasDifficultyVeriants = false;
            enemyData.SetObjectName();
        }

        [Button("Create")]
        public void CreateEnemyVeriant()
        {
            baseEnemy.difficultyVeriants.Add(enemyData);
            baseEnemy.ValidateSubTypes();
            enemyData.SetObjectName();

            string assetPath = AssetDatabase.GetAssetPath(baseEnemy);
            string folder = System.IO.Path.GetDirectoryName(assetPath);

            string GUID = AssetDatabase.CreateFolder(folder + "/" + "Difficulty Verients", enemyData.name.Replace("_difficultyVeriant", string.Empty));
            string path = AssetDatabase.GUIDToAssetPath(GUID);
            AssetDatabase.CreateFolder(path, "Stats");
            AssetDatabase.CreateAsset(enemyData, path + "/" + enemyData.name + ".asset");
            AssetDatabase.SaveAssets();

            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.hasSubTypes = false;
            enemyData.hasDifficultyVeriants = false;
            enemyData.SetObjectName();
        }
    }

    
}
    
    
