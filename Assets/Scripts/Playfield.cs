using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 数组表示俄罗斯方块的每个格子信息
//  以左下角为[0,0]点，往右x++，往上y++
/// </summary>
public class Playfield : MonoBehaviour {

    public int column = 10;
    public int maxRow = 30;
    public int deadRow = 22;
    public Transform cellPrefab;


    public Transform[,] grids;
    //public Transform[,] debugrids;
    //public TetrisRow[] cells;

    // cells 0 表示没有块,
    // 其他int值表示颜色索引

    private int[,] cells;

    public List<TetrisBlock> LockedBlocks;

    private void Awake() {
        LockedBlocks = new List<TetrisBlock>();
        InitCell();
        RenderGrid();
    }

    public void SetCell(TetrisBlock tb) {
        // set cells
        //cells[x, y] = index;
        LockedBlocks.Add(tb);
        // set blocks
    }

    public void RefreshCells() {
        cells = new int[column, maxRow];
        foreach (var item in LockedBlocks) {
            cells[item.X, item.Y] = 1;
        }
    }

    private void InitCell () {
        cells = new int[column, maxRow];
        grids = new Transform[column, maxRow];
        //debugrids = new Transform[column, maxRow];
    }

    public void RenderGrid () {
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
        //Transform debugcell = Instantiate(cellPrefab) as Transform;

        //debugcell.localPosition = new Vector3(x, y, 0) + Vector3.right * 10;

        //debugrids[x, y] = debugcell;
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
        RefreshCells();
        //Debug.Break();
    }

    public void ClearFullRow() {
        RefreshCells();
        int[] rows = CheckRows();
        ClearRows(rows);
        DownOthers(rows);
    }

    public void DownOthers(int[] rows) {
        for (int r = rows.Length-1; r >=0 ; r--) {
            foreach (var block in LockedBlocks) {
                if (block.Y > rows[r]) {
                    block.DropDown();
                    //cells[block.X, block.Y-1] = cells[block.X, block.Y];
                }
            }
        }
        RefreshCells();
    }

    // 边界检测
    private bool InBoundCheck(Vector2Int pos){
        if(pos.x<0 || pos.x>=column) return false;
        if(pos.y<0 || pos.y>=deadRow) return false;
        return true;
    }

    // 碰撞检测
    private bool NoBlockCheck(Vector2Int pos){
        return (cells[pos.x,pos.y] == 0);
    }

    public bool CanAction(Vector2Int pos){
        // 边界
        return (InBoundCheck(pos) && NoBlockCheck(pos));
    }

    // 检查坐标是否合法
    public bool CanAction(Vector2Int[] pos){
        //Debug.Assert(pos.Length == 4, "数据长度不够");
        bool r = true;
        for (int i = 0; i < pos.Length; i++) {
            r = r && CanAction(pos[i]);
        }
        return r;
    }
    //public Color[] colors = new Color[]{
    //    Color.gray,
    //    Color.green,
    //    Color.cyan,
    //    Color.black,
    //    Color.yellow,
    //    Color.red,
    //    Color.white
    //};

    //private void Update() {
    //    for (int y = 0; y < deadRow; y++) {
    //        for (int x = 0; x < column; x++) {
    //            debugrids[x, y].GetComponent<MeshRenderer>().material.color = colors[cells[x, y]];
    //        }
    //    }
    //}
}