using UnityEditor;
using UnityEngine;

public class GameEditor : EditorWindow
{
    float val;
    GUIGraph graph = new GUIGraph();
    [MenuItem("Window/PlotWindow")]
    public static void ShowWindow()
    {
        GetWindow<GameEditor>("GameEditor");
    }

    private void Update()
    {
        val++;
        graph.UpdateValues(val, Time.realtimeSinceStartup, 150, EditorApplication.isPlaying && !EditorApplication.isPaused, .1f);
        Repaint();
    }
    private void OnGUI()
    { 
        graph.Draw(new Rect(10, 10, 400, 400), new Rect(410, 10, 400, 400), 10, 50, Color.red, false);
    }

    private void OnEnable()
    {
        val = 0;
    }
}
