using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class GUIGraph
{
    private List<float> values = new List<float>();
    private List<float> times = new List<float>();
    private Rect zoomAreaRect;
    private Vector2 scrollPosition;
    private float minY;
    private float maxY;
    private float minX;
    private float maxX;

    public void ResetGraph()
    {
        values.Clear();
        times.Clear();
    }

    public void UpdateValues(float value)
    {
        UpdateValues(value, Time.realtimeSinceStartup, 150);
    }

    public void UpdateValues(float value, float yValue, int maxValueStorage, bool ShouldUpdate = true, float updateFrequency = 0)
    {
        // Only run the code if the editor is in play mode.
        if (ShouldUpdate)
        {
            // Update every 0.1 seconds.
            if (yValue - times.LastOrDefault() <= updateFrequency)
            {
                return;
            }
            values.Add(value);
            times.Add(yValue); // Add the current time to the times list

            // Keep the list length under max storage.
            while (values.Count > maxValueStorage)
            {
                values.RemoveAt(0);
                times.RemoveAt(0); // Also remove the first time value
            }
        }
    }

    #region Draw()
    public void Draw(Rect graphRect, Rect overviewGraphRect, float gridLineAmount, float padding,
       Color color, bool drawPreveiw = true)
    {
        ResetZoom();
        Draw(graphRect, overviewGraphRect, gridLineAmount, padding, true ,minY, maxY, minX, maxX,
            color, color, new Color(color.r, color.g, color.b, .1f), Color.grey, drawPreveiw);
    }
    public void Draw(Rect graphRect, Rect overviewGraphRect, float gridLineAmount, float padding,
      Color color, Color gridLineColor, float overviewColorAlpha, bool drawPreveiw = true)
    {
        ResetZoom();
        Draw(graphRect, overviewGraphRect, gridLineAmount, padding, true, minY, maxY, minX, maxX,
            color, color, new Color(color.r, color.g, color.b, overviewColorAlpha), gridLineColor, drawPreveiw);
    }
    public void Draw(Rect graphRect, Rect overviewGraphRect, float gridLineAmount, float padding,
       Color lineColor, Color zoomPreveiwBoxColor, Color GridLineColor, bool drawPreveiw = true)
    {
        ResetZoom();
        Draw(graphRect, overviewGraphRect, gridLineAmount, padding, true ,minY, maxY, minX, maxX,
            lineColor, new Color(zoomPreveiwBoxColor.r, zoomPreveiwBoxColor.g, zoomPreveiwBoxColor.b), zoomPreveiwBoxColor, GridLineColor, drawPreveiw);
    }

    public void Draw(Rect graphRect, Rect overviewGraphRect, float gridLineAmount, float padding,
      Color lineColor, Color zoomPreveiwBoxOutlineColor, Color zoomPreveiwBoxColor, Color GridLineColor, bool drawPreveiw = true)
    {
        ResetZoom();
        Draw(graphRect, overviewGraphRect, gridLineAmount, padding, true, minY, maxY, minX, maxX,
            lineColor, zoomPreveiwBoxOutlineColor, zoomPreveiwBoxColor, GridLineColor, drawPreveiw);
    }

    public void Draw(Rect graphRect, Rect overviewGraphRect, float gridLineAmount, float padding, bool automaticZoom,
        float minYValue, float maxYValue, float minXValue, float maxXValue,
        Color LineColor, Color zoomPreveiwBoxOutlineColor, Color zoomPreveiwBoxColor, Color GridLineColor, bool drawPreveiw = true)
    {
        minY = minYValue;
        maxY = maxYValue;
        minX = minXValue;
        maxX = maxXValue;

        if (automaticZoom)
        {
            ResetZoom();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(graphRect.width * 2), GUILayout.Height(graphRect.height));
        
        DrawGraph(graphRect, values, LineColor, padding, minY, maxY, minX, maxX, GridLineColor, gridLineAmount);

        if (drawPreveiw && overviewGraphRect != null) 
        {
            UpdateZoomBox(minXValue, maxXValue, minYValue, maxYValue, overviewGraphRect);
            DrawGraph(overviewGraphRect, values, LineColor, padding, values.Count > 0 ? values.Min() : 0, values.Count > 0 ? values.Max() : 1,
                times.Count > 0 ? times.Min() : 0, times.Count > 0 ? times.Max() : 1, GridLineColor, gridLineAmount);
            // Draw the zoom box on the overview graph
            Handles.DrawSolidRectangleWithOutline(zoomAreaRect, zoomPreveiwBoxColor, zoomPreveiwBoxOutlineColor);
        }
        
        EditorGUILayout.EndScrollView();
        Handles.EndGUI();
    }
    #endregion

    public void ResetZoom()
    {
        minY = values.Count > 0 ? values.Min() : 0;
        maxY = values.Count > 0 ? values.Max() : 1;
        minX = times.Count > 0 ? times.Min() : 0;
        maxX = times.Count > 0 ? times.Max() : 1;
    }

    private void DrawGraph(Rect rect, List<float> values, Color color, float padding, float minY, float maxY, float minX, float maxX, Color GridColor, float GridLineAmount)
    {

        if (values.Count < 2)
        {
            return;
        }

       

        Handles.BeginGUI();

        // Draw grid lines and labels.
        Handles.color = GridColor;
        float yInterval = (minY - maxY) / GridLineAmount;
        float xInterval = (maxX - minX) / GridLineAmount;

        for (int i = 0; i <= GridLineAmount; i++)
        {
            // Vertical grid lines
            float x = rect.xMin + padding + i * (rect.width - 2 * padding) / GridLineAmount;
            Handles.DrawLine(new Vector2(x, rect.yMin + padding), new Vector2(x, rect.yMax - padding));
            GUI.Label(new Rect(x - 25, rect.yMax - padding + 5, 50, 20), (minX + i * xInterval).ToString("F1"), new GUIStyle() { alignment = TextAnchor.UpperCenter });

            // Horizontal grid lines
            float y = rect.yMin + padding + i * (rect.height - 2 * padding) / GridLineAmount;
            Handles.DrawLine(new Vector2(rect.xMin + padding, y), new Vector2(rect.xMax - padding, y));
            GUI.Label(new Rect(rect.xMin, y - 10, padding, 20), (maxY + i * yInterval).ToString("F1"), new GUIStyle() { alignment = TextAnchor.UpperRight });
        }

        // Draw the plot.
        Handles.color = color;
        Vector2 lastPoint = Vector2.zero;
        for (int i = 0; i < values.Count; i++)
        {
            Vector2 point = new Vector2(
                Mathf.Lerp(rect.xMin + padding, rect.xMax - padding, (times[i] - minX) / (maxX - minX)),
                Mathf.Lerp(rect.yMax - padding, rect.yMin + padding, (values[i] - minY) / (maxY - minY))
            );

            if (i > 0)
            {
                Handles.DrawLine(lastPoint, point);
            }
            lastPoint = point;
        }


        // Draw the most recent y value.
        GUIStyle style = new GUIStyle();
        style.normal.background = MakeTex(100, 20, new Color(.4f, .4f, .4f, .75f));
        style.alignment = TextAnchor.UpperRight;
        GUI.Box(new Rect(lastPoint, new Vector2(100, 20)), values.Last().ToString("F2"), style);

        
    }

    Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    private void UpdateZoomBox(float minXValue, float maxXValue, float minYValue, float maxYValue, Rect rect)
    {
        // Assuming you have sliders or some input fields that determine the zoom level:
        // float minXValue, maxXValue, minYValue, maxYValue;

        // Calculate the normalized positions based on the full data range
        float normalizedMinX = (minXValue - times.Min()) / (times.Max() - times.Min());
        float normalizedMaxX = (maxXValue - times.Min()) / (times.Max() - times.Min());
        float normalizedMinY = (minYValue - values.Min()) / (values.Max() - values.Min());
        float normalizedMaxY = (maxYValue - values.Min()) / (values.Max() - values.Min());

        // Calculate the position and size of the zoom box on the overview graph
        zoomAreaRect.x = rect.xMin + normalizedMinX * rect.width;
        zoomAreaRect.width = (normalizedMaxX - normalizedMinX) * rect.width;
        zoomAreaRect.y = rect.yMin + (1 - normalizedMaxY) * rect.height;
        zoomAreaRect.height = (normalizedMaxY - normalizedMinY) * rect.height;
    }
}
