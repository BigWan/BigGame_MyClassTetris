using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数组表示俄罗斯方块的每个格子信息
//  以左下角为[0,0]点，往右x++，往上y++
/// </summary>
public class Playfield : MonoBehaviour {

    public int column = 10;
    public int maxRow = 22;
    public Transform cellPrefab;

    public int[, ] cells;

    public void SetField () {
        cells = new int[maxRow, column];
    }

    // public int[,] fields;
    public void InitField () {
        for (int y = 0; y < maxRow; y++) {
            for (int x = 0; x < column; x++) {
                cells[x, y] = 0;
            }
        }
    }

    public void RenderGrid () {
        for (int i = 0; i < maxRow; i++) {
            for (int j = 0; j < column; j++) {
                RenderCell (i, j);
            }
        }
    }
    public void RenderCell (int i, int j) {

    }
    public void CheckRows () {
        List<int> r = new List<int> ();
        for (int i = 0; i < maxRow; i++) {
            int s = 0;
            for (int j = 0; j < column; j++) {
                s += cells[i, j];
            }
            if (s >= column) { // 这行满了
                r.Add (i);
            }
        }
    }

    void CheckRow () {
        for (int i = 0; i < 22; i++) {
            CheckARow (i);
        }
    }

    void CheckARow (int rowIndex) {

    }
}