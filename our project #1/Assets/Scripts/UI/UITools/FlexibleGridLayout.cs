using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Hight,
        FixedRows,
        FixedCollumns
    }




    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public FitType fitType;
    public bool fitX;
    public bool fitY;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Hight)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if(fitType == FitType.Width || fitType == FitType.FixedCollumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Hight || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parrentHight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHight = (parentWidth / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);


        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHight : cellSize.y;

        int collumnCount = 0;
        int rowCount = 0;

        for(int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            collumnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * collumnCount) + (spacing.x * collumnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
