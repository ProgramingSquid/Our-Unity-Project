using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using UnityEngine;
using System.Linq;

public class EnemyEditor : OdinMenuEditorWindow
{
    [MenuItem("Windows/GameEditors/EnemyEditor")]
    public static void ShowWindow()
    {
        var window = GetWindow<EnemyEditor>("EnemyEditors");
        window.minSize = new Vector2(700, 500);
    }

    protected override void OnDestroy()
    {
        if (enemyCreationGUI != null) { DestroyImmediate(enemyCreationGUI.enemyData); }
        if (subEnemyCreateGUI != null) { DestroyImmediate(subEnemyCreateGUI.enemyData); }
    }

    protected override void OnBeginDrawEditors() 
    {
        var selection = MenuTree.Selection;
        EnemyTypeSO selectedEnemyData;
        SirenixEditorGUI.BeginHorizontalToolbar();
        if (selection.SelectedValue is EnemyTypeSO)
        {
            selectedEnemyData = selection.SelectedValue as EnemyTypeSO;
            if (selectedEnemyData.hasSubTypes)
            {
                if (SirenixEditorGUI.ToolbarButton("New Sub Type Of Selected"))
                {
                    subEnemyCreateGUI.baseEnemy = selectedEnemyData;
                    MenuTree.Selection.Clear();
                    MenuTree.Selection.Add(MenuTree.GetMenuItem("Create New Sub Type"));
                }
            }

            if (selectedEnemyData.hasDifficultyVeriants)
            {
                if (SirenixEditorGUI.ToolbarButton("New Difficulty Veriant of Selected"))
                {
                    veriantEnemyCreateGUI.baseEnemy = selectedEnemyData;
                    MenuTree.Selection.Clear();
                    MenuTree.Selection.Add(MenuTree.GetMenuItem("New Difficulty Veriant"));
                }
            }

            if (SirenixEditorGUI.ToolbarButton("Create New Stat"))
            {
                enemyStatCreateGUI.enemyData = selectedEnemyData;
                MenuTree.Selection.Clear();
                MenuTree.Selection.Add(MenuTree.GetMenuItem("Create New Enemy Stat"));
            }
            GUILayout.FlexibleSpace();
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.ArrowClockwise))
            {
                selectedEnemyData.ValidateSubTypes();
            }
        }
        else { GUILayout.FlexibleSpace(); }

        if(!(selection.SelectedValue is EnemyTypeSO || selection.SelectedValue is EnemyStat)) 
        { SirenixEditorGUI.EndHorizontalToolbar(); return; }

        if (SirenixEditorGUI.ToolbarButton(SdfIconType.BarChartSteps))
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
            var assetPath = AssetDatabase.GetAssetPath((Object)selection.SelectedValue);
            string path = assetPath;
            if(selection.SelectedValue is EnemyTypeSO)
            {
                path = AssetUtilities.GetAssetFolder(assetPath);
            }
            
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    EnemyCreateGUI enemyCreationGUI;
    SubEnemyCreateGUI subEnemyCreateGUI;
    VeriantEnemyCreateGUI veriantEnemyCreateGUI;
    EnemyStatCreateGUI enemyStatCreateGUI;
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree;
        tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = true;

        enemyCreationGUI = new EnemyCreateGUI();
        subEnemyCreateGUI = new SubEnemyCreateGUI();
        veriantEnemyCreateGUI = new VeriantEnemyCreateGUI();
        enemyStatCreateGUI = new EnemyStatCreateGUI();

        var enemyTypePath = "Assets/Game Items/Enemy";
        tree.Add("Create New Base Enemy", enemyCreationGUI);
        tree.Add("Create New Sub Type", subEnemyCreateGUI);
        tree.Add("New Difficulty Veriant", veriantEnemyCreateGUI);
        tree.Add("Create New Enemy Stat", enemyStatCreateGUI);

        tree.AddAllAssetsAtPath("Enemy Data", enemyTypePath, typeof(EnemyTypeSO), true, false);
        tree.AddAllAssetsAtPath("Enemy Data", enemyTypePath, typeof(EnemyStat), true, false);
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
            string folder = AssetUtilities.GetAssetFolder(assetPath);

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
    class EnemyStatCreateGUI
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public EnemyStat enemyStat;

        public EnemyTypeSO enemyData;
        public EnemyStatCreateGUI()
        {
            enemyStat = ScriptableObject.CreateInstance<EnemyStat>();
            enemyStat.tag = "New Stat";
        }

        [Button("Create")]
        public void CreateEnemyVeriant()
        {
            enemyData.EnemyStats.Add(enemyStat);


            string assetPath = AssetDatabase.GetAssetPath(enemyData);
            string folder = System.IO.Path.GetDirectoryName(assetPath);

            AssetDatabase.CreateAsset(enemyStat, folder + "/Stats/" + enemyStat.tag.Replace(" ", string.Empty) + ".asset");
            AssetDatabase.SaveAssets();
            enemyStat = ScriptableObject.CreateInstance<EnemyStat>();
            enemyStat.tag = "New Stat";
        }
    }

    
}
    
    
