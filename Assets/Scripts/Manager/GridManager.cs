using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void ClearUpHandler(int count);

/// <summary>
/// 数组表示俄罗斯方块的每个格子信息
//  以左下角为[0,0]点，往右x++，往上y++
/// </summary>
// [ExecuteInEditMode]
public class GridManager : UnitySingleton<GridManager> {

    // Setting
    public int column  = 10;
    public int maxRow  = 30;
    public int deadRow = 22;

    // 静态引用
    public Transform cellPrefab;
    // // 动态引用

    public Vector2Int SpwanOrigin;    // 生成的方块的原点
    private Transform[,] grids;

    private void CalcOrigin() {
        SpwanOrigin = new Vector2Int(column / 2 - 1, deadRow - 1);
    }

    private void Awake() {

        for (int i = transform.childCount - 1;i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        RenderGrid();
        CalcOrigin();
    }

    /// <summary>
    /// 生成棋盘格
    /// </summary>
    private void RenderGrid () {
        for (int y = 0; y < deadRow; y++) {
            GameObject go = new GameObject("Row" + y);
            go.transform.SetParent(transform);
            for (int x = 0; x < column; x++) {
                RenderCell (x, y,go);
            }
        }
    }

    private void RenderCell (int x, int y,GameObject go) {
        Transform cell = Instantiate(cellPrefab) as Transform;
        cell.name = x.ToString() + '_' +  y.ToString();
        cell.SetParent(go.transform);
        cell.localPosition = new Vector3(x, y, 0);
        // cell.hideFlags = Hide;
    }

}