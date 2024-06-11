using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class EnemyEditor : OdinMenuEditorWindow
{
    [MenuItem("Windows/GameEditors/EnemyEditor")]
    public static void ShowWindow()
    {
        GetWindow<EnemyEditor>("EnemyEditors");
    }

    protected override void OnDestroy()
    {
        if (EnemyCreation != null) { EnemyCreation.enemyData = null; }
    }

    protected override void OnBeginDrawEditors() 
    {
        var selection = MenuTree.Selection;
        SirenixEditorGUI.BeginHorizontalToolbar();
        {

            if (selection.SelectedValue is EnemyTypeSO)
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("Delete Selected"))
                {
                    bool confirmed = EditorUtility.DisplayDialog("Confirm Deletion", 
                        "Are you sure you want to delete this asset? This action cannot be reversed.", "Delete", "Cancel");

                    if(!confirmed) { return; }
                    var asset = selection.SelectedValue as EnemyTypeSO;
                    var assetPath = AssetDatabase.GetAssetPath(asset);
                    string path = assetPath;

                    if (asset.hasSubTypes)
                    {
                        path = AssetUtilities.GetAssetFolder(assetPath);
                    }
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }

                if (SirenixEditorGUI.ToolbarButton("Create New SubType Of Selected"))
                {

                }
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();

    }


    EnemyCreationGUI EnemyCreation;
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        EnemyCreation = new EnemyCreationGUI();
        var enemyTypePath = "Assets/Game Items/Enemy";
        tree.Add("Enemy", EnemyCreation);
        var enemyDataMenue = tree.AddAllAssetsAtPath("Enemy/Enemy Data", enemyTypePath, typeof(EnemyTypeSO), true, false);

        return tree;


    }
    class EnemyCreationGUI
    {
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public EnemyTypeSO enemyData;

        public EnemyCreationGUI()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.SetObjectName();
        }

        [Button("Create")]
        public void CreateBaseEnemy()
        {
            string GUIID = AssetDatabase.CreateFolder("Assets/Game Items/Enemy", enemyData.name.Replace("_base", string.Empty));
            string folder = AssetDatabase.GUIDToAssetPath(GUIID);
            string path = folder;
            AssetDatabase.SaveAssets();
            AssetDatabase.CreateFolder(path, "Stats");
            AssetDatabase.CreateFolder(path, "Sub Types");
            AssetDatabase.CreateAsset(enemyData, path + "/" + enemyData.name + ".asset");
            AssetDatabase.SaveAssets();

            enemyData = ScriptableObject.CreateInstance<EnemyTypeSO>();
            enemyData.enemyName = "New Enemy";
            enemyData.SetObjectName();
        }

    }
}
