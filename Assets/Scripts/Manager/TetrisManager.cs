using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// TODO
// 重构
// 合理的随机()
// UI功能
// 用池来管理Block的生成和摧毁
// 游戏流程,开始结束,摄像机等

public delegate void ScoreChangeHandler(int score);

public enum DropStat{
	Spawning,
	Dropping,
	Delaying,
}


/// <summary>
/// 方块管理器,负责方块旋转,下落,消行等
/// </summary>
public class TetrisManager : UnitySingleton<TetrisManager> {

	// 游戏设置

	public float lockDelay = 0.5f;   // 下落延迟
	public int[] clearScores = new int[]{0,1,3,5,8};

	// 资源引用
    public Transform blockContainer; // 方块父物体



	public int[,] cells;
    // 动态引用
    public List<TetrisBlock> LockedBlocks;

	// 私有全局变量
	// private GameStat stat;				// 游戏状态

    public int currentShapeID = 0;		// 随机生成的块
	public int nextShapeID = 0;		// 下一次的块ID，在UI框显示

	private bool isDropOver = true;    	// 是否一次掉落完毕，可以继续生成下一个块

	// private bool GameOverStat = false;

    // private bool GameStart = false;
	// 游戏数据
	public int score;

	// 事件
	public event EventHandler ScoreChange;   // 得分事件
	public event ClearUpHandler ClearUp;  // 消行
    public event EventHandler GameOver;      // 游戏结束事件



	public Color[] colors = new Color[]{
		Color.cyan,
		Color.yellow,   // 黄色O
		Color.red,      // 红色Z
		Color.green,    // 绿色S
		Color.blue,     // 蓝色J
		new Color(1f,165f/255f,0),
        new Color(0.5f,0,0.5f)
	};



	private int column {
		get{return GridManager.Instance.column;}
	}
	private int maxRow {
		get{return GridManager.Instance.maxRow;}
	}

    public void RefreshCells() {
        cells = new int[column, maxRow];
        foreach (var item in LockedBlocks) {
            cells[item.X, item.Y] = 1;
        }
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
        ClearUp(rows.Length);
    }

    /// <summary>
    /// Clear Line 后的其他行的下落
    /// </summary>
    /// <param name="rows">消失的行号</param>
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

	private void Awake() {
		ClearUp += OnClearUp;
		LockedBlocks = new List<TetrisBlock>();
        cells = new int[column, maxRow];
        // grids = new Transform[column, maxRow];
	}

	// 开始游戏
	public void StartGame(){
        SpawnNewTetris();

    }




	void Update () {
		if(dropStat == DropStat.Dropping){
			nextDroptime += Time.deltaTime;

			if (Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Space)) {
				ActivateShapeManager.Instance.Rotation ();
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				ActivateShapeManager.Instance.Move (Vector2Int.left);
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ActivateShapeManager.Instance.Move (Vector2Int.right);
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)){
				ActivateShapeManager.Instance.QuickDropDown();
			}

			ActivateShapeManager.Instance.DropDown();
		}
	}



	public void OnClearUp(int count){
		AddScore(clearScores[count]);
		SpawnNewTetris();
	}

	public void AddScore(int s){

		score += s;
        Debug.Log(score);
        ScoreChange(this,EventArgs.Empty);
    }

	public void SpawnNewTetris(){
		dropStat = DropStat.Spawning;
        currentShapeID = UnityEngine.Random.Range(0, 7);
        ActivateShapeManager.Instance.SpawnTetris(currentShapeID);
		dropStat = DropStat.Dropping;
	}

	/// <summary>
    /// 检测坐标是否在边界内部
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <returns></returns>
    public bool InBound(Vector2Int pos){
        if(pos.x<0 || pos.x>=column) return false;
        if(pos.y<0 || pos.y>=maxRow) return false;
        return true;
    }

}