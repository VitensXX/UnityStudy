using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2020/7/25 15:31:23
/// 
/// Description : 
///     单元格子的控制
/// </summary>
public class GridCellCtrl
{
    public GameObject gameObject;
    RawImage img;
    public Vector2Int pos;

    public GridCellCtrl(RawImage rawImage, Vector2Int pos)
    {
        gameObject = rawImage.gameObject;
        img = rawImage;
        this.pos = pos;
    }

    public void SetColor(Color color)
    {
        img.color = color;
    }

    int i;
    public void ChangeColor()
    {
        if(++i % 2 == 1)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.white);
        }
    }
}
