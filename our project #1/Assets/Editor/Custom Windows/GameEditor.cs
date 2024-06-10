using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;

public class GameEditor : OdinMenuEditorWindow
{
    [MenuItem("Window/PlotWindow")]
    public static void ShowWindow()
    {
        GetWindow<GameEditor>("GameEditor");
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        var enemyTypePath = "Assets/Game Items/Enemy";
        var enemyDataMenue = tree.AddAllAssetsAtPath("Enemy Data", enemyTypePath, typeof(EnemyTypeSO) , true, false);
        
        return tree;
    }
}
