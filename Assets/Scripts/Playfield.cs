using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void ClearUpHandler(int count);

/// <summary>
/// 数组表示俄罗斯方块的每个格子信息
//  以左下角为[0,0]点，往右x++，往上y++
/// </summary>
public class Playfield : MonoBehaviour {

    // Setting
    public int column  = 10;
    public int maxRow  = 30;
    public int deadRow = 22;

    // 静态引用
    public Transform cellPrefab;
    // 动态引用
    public List<TetrisBlock> LockedBlocks;
    public Vector2Int SpwanOrigin;    // 生成的方块的原点
    private Transform[,] grids;

    // 棋盘数据
    public int[,] cells;

    // 事件
    public event ClearUpHandler ClearUp;  // 消行结束

    private void CalcOrigin() {
        SpwanOrigin = new Vector2Int(column / 2 - 1, deadRow - 1);
    }

    private void Awake() {
        LockedBlocks = new List<TetrisBlock>();
        cells = new int[column, maxRow];
        grids = new Transform[column, maxRow];
        RenderGrid();
        CalcOrigin();
    }

    public void RefreshCells() {
        cells = new int[column, maxRow];
        foreach (var item in LockedBlocks) {
            cells[item.X, item.Y] = 1;
        }
    }

    /// <summary>
    /// 生成棋盘格
    /// </summary>
    private void RenderGrid () {
        for (int y = 0; y < deadRow; y++) {
            for (int x = 0; x < column; x++) {
                RenderCell (x, y);
            }
        }
    }

    private void RenderCell (int x, int y) {
        Transform cell = Instantiate(cellPrefab) as Transform;
        cell.SetParent(transform);
        cell.localPosition = new Vector3(x, y, 0);
    }

    private int[] CheckRows () {
        List<int> rows = new List<int>();

        for (int y = 0; y < maxRow; y++) {
            int s =1;

            for (int x = 0; x < column; x++) {
                s *= cells[x, y];
            }

            if (s > 0) {
                rows.Add(y);
            }
        }
        return rows.ToArray();
    }


    private void ClearRows(int[] rows) {
        if(rows.Length==0) return;
        for (int r = 0; r < rows.Length; r++) {
            for (int  i = LockedBlocks.Count - 1;i>=0 ; i--) {
                var item = LockedBlocks[i];
                if (item.Y == rows[r]) {
                    item.Explosion();
                    LockedBlocks.RemoveAt(i);
                }
            }
        }
        ClearUp(rows.Length);
        RefreshCells();
        //Debug.Break();
    }

    /// <summary>
    /// 检测是否要消行
    /// </summary>
    public void ClearFullRow() {
        RefreshCells();
        int[] rows = CheckRows();
        if(rows.Length > 0) {
            ClearRows(rows);
            DownOthers(rows);
        }
    }

    /// <summary>
    /// Clear Line 后的其他行的下落
    /// </summary>
    /// <param name="rows"></param>
    public void DownOthers(int[] rows) {
        for (int r = rows.Length-1; r >=0 ; r--) {
            foreach (var block in LockedBlocks) {
                if (block.Y > rows[r]) {
                    block.DropDown();
                }
            }
        }
        RefreshCells();
    }

    /// <summary>
    /// 检测坐标是否在边界内部
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <returns></returns>
    private bool InBound(Vector2Int pos){
        if(pos.x<0 || pos.x>=column) return false;
        if(pos.y<0 || pos.y>=deadRow) return false;
        return true;
    }

    /// <summary>
    /// 检测目标点是否为空
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <returns></returns>
    private bool IsEmpty(Vector2Int pos){
        return (cells[pos.x,pos.y] == 0);
    }

    /// <summary>
    /// 该坐标是否能移动过去
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <returns></returns>
    public bool CanAction(Vector2Int pos){
        return (InBound(pos) && IsEmpty(pos));
    }

    /// <summary>
    /// 批量检测坐标合法性
    /// </summary>
    /// <param name="pos">坐标数组</param>
    /// <returns></returns>
    public bool CanAction(Vector2Int[] pos){
        //Debug.Assert(pos.Length == 4, "数据长度不够");
        bool r = true;
        for (int i = 0; i < pos.Length; i++) {
            r = r && CanAction(pos[i]);
        }
        return r;
    }

}