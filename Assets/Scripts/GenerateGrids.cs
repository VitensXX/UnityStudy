using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2020/7/25 14:16:57
/// 
/// Description : 
///     用小方块创建N*N的网格
/// </summary>
public class GenerateGrids : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler
{
    public Transform gridAnchor;
    public Vector2Int size;//i*j
    public float cellSize;//单元格子的尺寸
    public float interval;//间距

    Dictionary<Vector2Int, GridCellCtrl> _dicPos2Ctrl = new Dictionary<Vector2Int, GridCellCtrl>();
    Dictionary<GameObject, GridCellCtrl> _dicGO2Ctrl = new Dictionary<GameObject, GridCellCtrl>();

    void Start()
    {
        Generate();
    }

    //创建格子
    public void Generate()
    {
        Clear();

        //计算左下角的位置,为了得到偏移量,使得每次创建的方格盘都能显示在屏幕中间
        float minX = -(size.x * cellSize + (size.x - 1) * interval) / 2;
        float minY = -(size.y * cellSize + (size.y - 1) * interval) / 2;
        float offsetX = minX + cellSize / 2;
        float offsetY = minY + cellSize / 2;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                RawImage cell = new GameObject(i + "," + j).AddComponent<RawImage>();
                cell.transform.SetParent(gridAnchor);
                cell.transform.localPosition = new Vector3(i * (cellSize + interval) + offsetX, j * (cellSize + interval) + offsetY);
                cell.rectTransform.sizeDelta = new Vector2(cellSize, cellSize);

                Vector2Int pos = new Vector2Int(i, j);
                GridCellCtrl cellCtrl = new GridCellCtrl(cell, pos);
                //添加记录
                _dicPos2Ctrl.Add(pos, cellCtrl);
                _dicGO2Ctrl.Add(cell.gameObject, cellCtrl);
            }
        }
    }

    //清空当前所有格子
    public void Clear()
    {
        //强制清除gridAnchor下面的所有cell
        RawImage[] cells = gridAnchor.GetComponentsInChildren<RawImage>();
        for (int i = 0; i < cells.Length; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(cells[i].gameObject);
            }
            else
            {
                DestroyImmediate(cells[i].gameObject);
            }
        }

        _dicPos2Ctrl.Clear();
        _dicGO2Ctrl.Clear();
    }

    Vector2Int _start = Vector2Int.zero;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject pressGO = eventData.pointerEnter;
        if (_dicGO2Ctrl.ContainsKey(pressGO))
        {
            //DrawLine(_dicGO2Ctrl[pressGO].pos);
            GridCellCtrl ctrl = _dicGO2Ctrl[pressGO];
            _start = ctrl.pos;
            ClearLine();
            ctrl.SetColor(Color.red);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject pressGO = eventData.pointerEnter;
        if (_dicGO2Ctrl.ContainsKey(pressGO))
        {
            DrawLine(_dicGO2Ctrl[pressGO].pos);
        }
    }

    void DrawLine(Vector2Int end)
    {
        ClearLine();

        _dicPos2Ctrl[_start].SetColor(Color.red);

        GridCellCtrl ctrl = _dicPos2Ctrl[end];
        ctrl.SetColor(Color.green);

        List<Vector2Int> touchedGrids = GridHelper.GetTouchedPosBetweenTwoPoints(_start, ctrl.pos);
        for (int i = 0; i < touchedGrids.Count; i++)
        {
            _dicPos2Ctrl[touchedGrids[i]].SetColor(Color.black);
        }
    }

    void ClearLine()
    {
        foreach (var item in _dicGO2Ctrl)
        {
            item.Value.SetColor(Color.white);
        }
    }
}
