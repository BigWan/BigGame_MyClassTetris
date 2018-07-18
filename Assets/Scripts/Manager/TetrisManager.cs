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


    public TetrisBlock blockPrefab;  // 方块预制体
    private TetrisBlock[] blocks;       // 块的引用

    // 踢墙算法检测的5个点
    Vector2Int[] TickOffsetPoint = new Vector2Int[]{
        Vector2Int.zero,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down
    };
    public DropStat dropStat;
    // 数据配置
    private int[][] posdata = {
        new int[]{ -1, 0, 0, 0, 1, 0, 2, 0 },   // I
		new int[]{ 0, 0, 1, 0, 0, -1, 1, -1 },  // O
		new int[]{ -1, 0, 0, 0, 0, -1, 1, -1 }, // Z
		new int[]{ 0, 0, 1, 0, -1, -1, 0, -1 }, // S
		new int[]{ -1, 0, 0, 0, 1, 0, 1, -1 },  // J
		new int[]{ -1, 0, 0, 0, 1, 0, -1, -1 }, // L
		new int[]{ -1, 0, 0, 0, 1, 0, 0, -1 }    // T
	};

    private float[][] pivotdata = {
        new float[]{0.5f,-0.5f},
        new float[]{0.5f,-0.5f},
        new float[]{0,0},
        new float[]{0,0},
        new float[]{0,0},
        new float[]{0,0},
        new float[]{0,0}
    };
    public Transform pivot;          // 旋转点
                                     // 生成方块
    public void SpawnTetris(int index) {
        if (dropStat != DropStat.Spawning) return;
        Vector2Int origin = GridManager.Instance.SpwanOrigin;
        int rnk = 4;                    // TODO 支持其他类型的block
        blocks = new TetrisBlock[rnk];
        pivot.localPosition = new Vector3(pivotdata[index][0] + origin.x, pivotdata[index][1] + origin.y);
        for (int i = 0; i < blocks.Length; i++) {
            blocks[i] = Instantiate<TetrisBlock>(blockPrefab) as TetrisBlock;
            blocks[i].GetComponent<SpriteRenderer>().sprite = blocks[i].colors[index];
            blocks[i].Coord = new Vector2Int(posdata[index][i * 2], posdata[index][i * 2 + 1]) + origin;
            if (!TetrisManager.Instance.CanAction(blocks[i].Coord)) {
                blocks = null;
                UIManager.Instance.ShowPage("MainPage");
            }
        }
    }

    void Clearll() {

    }

    /// <summary>
    /// 方块旋转,踢墙和踢地板
    /// </summary>
    public void Rotation() {
        if (dropStat != DropStat.Dropping) return;
        Vector2Int[] newpos = new Vector2Int[4];
        // 计算原地旋转后的坐标
        for (int i = 0; i < blocks.Length; i++) {
            newpos[i] = Vector2Util.RotateClockWise(blocks[i].Coord, pivot.localPosition);
        }
        //踢墙检测
        for (int j = 0; j < TickOffsetPoint.Length; j++) {
            Vector2Int[] kickpos = new Vector2Int[4];
            for (int i = 0; i < blocks.Length; i++) {
                kickpos[i] = newpos[i] + Vector2Int.up;
            }

            if (CanAction(newpos)) {             // 能转
                nextDroptime = 0f;
                for (int i = 0; i < blocks.Length; i++) {
                    blocks[i].Coord = newpos[i];
                }
                return;
            }
        }
    }

    /// <summary>
    /// 移动功能
    /// </summary>
    /// <param name="dir">左右方向</param>
    public void Move(Vector2Int dir) {
        if (dropStat != DropStat.Dropping) return;
        Vector2Int[] newpos = new Vector2Int[4];

        for (int i = 0; i < blocks.Length; i++) {
            newpos[i] = blocks[i].Coord + dir;
        }

        if (TetrisManager.Instance.CanAction(newpos)) {
            for (int i = 0; i < blocks.Length; i++) {
                blocks[i].Coord = newpos[i];
            }
            pivot.localPosition = pivot.localPosition + new Vector3(dir.x, dir.y);
            nextDroptime = 0f;
        }
    }
    public float speed = 0.25f;      // 速度
    public float nextDroptime;      // 下次降落时间
                                    /// <summary>
                                    /// 正常下落功能
                                    /// </summary>
                                    ///
                                    ///
    void DropDown() {
        if (nextDroptime >= speed && dropStat == DropStat.Dropping) {
            nextDroptime -= speed;
            Vector2Int[] newpos = new Vector2Int[4];
            // 能不能移
            for (int i = 0; i < blocks.Length; i++) {
                newpos[i] = blocks[i].Coord + Vector2Int.down;
            }

            if (TetrisManager.Instance.CanAction(newpos)) {
                for (int i = 0; i < blocks.Length; i++) {
                    blocks[i].Coord = newpos[i];
                }
                pivot.localPosition = pivot.localPosition + Vector3.down;
            } else {
                LockShape();
                return;
            }
        }
    }


    /// <summary>
    /// 快速下落功能，快捷键向下箭头
    /// </summary>
    public void QuickDropDown() {
        if (dropStat != DropStat.Dropping) return;
        int dis = ClacGhostDistance();
        for (int i = 0; i < 4; i++) {
            blocks[i].Coord = blocks[i].Coord + Vector2Int.down * dis;
        }
        pivot.localPosition = pivot.localPosition + Vector3.down * dis;
        dropStat = DropStat.Delaying;
        LockShape();
    }

    /// <summary>
    /// 计算幽灵的距离
    /// </summary>
    int ClacGhostDistance() {
        // 计算每个方块最大还能移动的距离
        int[] maxdis = new int[4];
        for (int i = 0; i < maxdis.Length; i++) {   // 遍历blocks
            for (int j = blocks[i].Y - 1; j >= 0; j--) { // 计算每列和当前block的高度差,
                if (TetrisManager.Instance.cells[blocks[i].X, j] == 0) {
                    maxdis[i] += 1;
                } else {
                    break;
                }
            }
        }
        return maxdis.Min();
    }

    // 停止
    public void LockShape() {
        dropStat = DropStat.Delaying;
        nextDroptime = 0;
        for (int i = 0; i < blocks.Length; i++) {
            blocks[i].transform.SetParent(TetrisManager.Instance.blockContainer);
            TetrisManager.Instance.LockedBlocks.Add(blocks[i]);
            blocks[i] = null;
        }
        TetrisManager.Instance.RefreshCells();
        TetrisManager.Instance.ClearFullRow();
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
				Rotation ();
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Move (Vector2Int.left);
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Move (Vector2Int.right);
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)){
				QuickDropDown();
			}

			DropDown();
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
        SpawnTetris(currentShapeID);
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