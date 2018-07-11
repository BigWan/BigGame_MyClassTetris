using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// TODO: 数据配置化(数组里配也算ok)
// 快速下落功能(ok)
// 旋转踢墙(勉强ok)
// 合理的随机()
// UI功能
// 用池来管理Block的生成和摧毁
// 游戏流程,开始结束,摄像机等


public enum  GameStat {
	Ready,				// UI界面
	Begin,				// 开始游戏
    CutDown,			// 倒计时
	Playing,			// 进行中
	End					// 结束
}

/// <summary>
/// 方块管理器,负责方块旋转,下落,消行等
/// </summary>
public class TetrisManager : MonoBehaviour {

	// 游戏设置
    public float   speed = 0.25f;      // 速度
	private float  lockDelay = 0.5f;   // 下落延迟

	// 资源引用
    public Transform blockContainer; // 方块父物体
    public TetrisBlock blockPrefab;  // 方块预制体
    public Transform pivot;          // 旋转点
    public Playfield field;          // 地图

	// 私有全局变量
	private GameStat stat;				// 游戏状态
    private float nextDroptime; 		// 下次降落时间
    private int currentShapeID = 0;		// 随机生成的块
	private int nextShapeID = 0;		// 下一次的块ID，在UI框显示
    private TetrisBlock[] blocks;     	// 块的引用
	private bool isDropOver = true;    	// 是否一次掉落完毕，可以继续生成下一个块

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

	private float [][] pivotdata = {
		new float[]{0.5f,-0.5f},
		new float[]{0.5f,-0.5f},
		new float[]{0,0},
		new float[]{0,0},
		new float[]{0,0},
		new float[]{0,0},
		new float[]{0,0}
	};

	public Color[] colors = new Color[]{
		Color.cyan,
		Color.yellow,   // 黄色O
		Color.red,      // 红色Z
		Color.green,    // 绿色S
		Color.blue,     // 蓝色J
		new Color(1f,165f/255f,0),
        new Color(0.5f,0,0.5f)
	};


	// 踢墙算法检测的5个点
	Vector2Int[] checkpoint = new Vector2Int[]{
		Vector2Int.zero,
		Vector2Int.left,
		Vector2Int.right,
		Vector2Int.up,Vector2Int.down
	};

	// 初始化游戏
	void InitGame(){
		stat = GameStat.Ready;
		
	}

	// 生成方块
	public void SpawnTetris(int index){
		Vector2Int origin = field.SpwanOrigin;
		int rnk = 4;					// TODO 支持其他类型的block
		blocks = new TetrisBlock[rnk];
		pivot.localPosition = new Vector3 (pivotdata[index][0]+origin.x, pivotdata[index][1]+origin.y);
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i] = Instantiate<TetrisBlock>(blockPrefab) as TetrisBlock;
            blocks[i].color = colors[index];
            blocks[i].pos = new Vector2Int(posdata[index][i * 2], posdata[index][i * 2 + 1])+origin;
		}
	}

	/// <summary>
	/// 方块旋转,踢墙和踢地板
	/// </summary>
	public void Rotation () {
		Vector2Int[] newpos = new Vector2Int[4];
		// 计算原地旋转后的坐标
		for (int i = 0; i < blocks.Length; i++) {
			newpos[i] = Vector2Util.RotateClockWise(blocks[i].pos, pivot.localPosition);
		}

		for (int j = 0; j < checkpoint.Length; j++) {
			Vector2Int[] kickpos = new Vector2Int[4];
			for (int i = 0; i < blocks.Length; i++) {
				kickpos[i] = newpos[i]+Vector2Int.up;
			}

			if(field.CanAction(newpos)) {				// 能转
				nextDroptime = 0f;
				for (int i = 0; i < blocks.Length; i++) {
					blocks[i].pos = newpos[i];
				}
				return;
			}
		}
	}

	void Move(Vector2Int dir){
		Vector2Int[] newpos = new Vector2Int[4];
		// 能不能移
		for (int i = 0; i < blocks.Length; i++) {
			newpos[i] = blocks[i].Coord + dir;
		}

        if (field.CanAction(newpos)) {
            for (int i = 0; i < blocks.Length; i++) {
                blocks[i].pos = newpos[i];
            }
            pivot.localPosition = pivot.localPosition + new Vector3(dir.x,dir.y);
			nextDroptime = 0f;
        }
	}

	/// <summary>
	/// 正常下落功能，到达终点后延迟0.5s
	/// </summary>
    void DropDown() {
		if (nextDroptime >= speed && isDropOver){
			nextDroptime -= speed;
			Vector2Int[] newpos = new Vector2Int[4];
			// 能不能移
			for (int i = 0; i < blocks.Length; i++) {
				newpos[i] = blocks[i].pos + Vector2Int.down;
			}

			if (field.CanAction(newpos)) {
				// 移
				for (int i = 0; i < blocks.Length; i++) {
					blocks[i].pos = newpos[i];
				}
				pivot.localPosition = pivot.localPosition + Vector3.down;

			} else {
				StartCoroutine(LockShape());
				return;
			}
		}
    }

	/// <summary>
	/// 快速下落功能，快捷键向下箭头
	/// </summary>
	void QuickDropDown(){
		int dis = ClacGhostDistance();
		for (int i = 0; i < 4; i++) {
			blocks[i].pos = blocks[i].pos + Vector2Int.down*dis;
		}
		pivot.localPosition = pivot.localPosition + Vector3.down*dis;
		StartCoroutine(LockShape());
	}

	/// <summary>
	/// 计算幽灵的位置
	/// </summary>
	int ClacGhostDistance(){
		// 计算每个方块最大还能移动的距离
		int[] maxdis = new int[4];
		for (int i = 0; i < maxdis.Length; i++) { 	// 遍历blocks
			for (int j = blocks[i].Y-1; j >= 0; j--) { // 计算每列和当前block的高度差,
				if(field.cells[blocks[i].X,j]==0){
					maxdis[i]+=1;
				}else{
					break;
				}
			}
		}
		return  maxdis.Min();						// 高度差的最小值就是方块能移动的最大距离
	}

	private void Start() {
		SpawnNewTetris();
	}

	void Update () {
		nextDroptime += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.UpArrow))     { Rotation ();		        }
		if (Input.GetKeyDown (KeyCode.LeftArrow))   {
			Move (Vector2Int.left);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow))  {
			Move (Vector2Int.right);
		}
		if (Input.GetKeyDown (KeyCode.Space))       { Rotation ();		        }
		if (Input.GetKeyDown (KeyCode.F1))   { 	SpawnNewTetris();	}
		if (Input.GetKeyDown (KeyCode.DownArrow))           { QuickDropDown();	}
		DropDown();
	}

	// 停止
	public IEnumerator LockShape () {
        Debug.Log("Lock a Shape");
		isDropOver = false;
		nextDroptime = 0;
		for (int i = 0; i < blocks.Length; i++) {
			//blocks[i].isLock = true;
            blocks[i].transform.SetParent(blockContainer);
            //field.cells[blocks[i].X, blocks[i].Y] = currentType+1;
            field.SetCell(blocks[i]);
            blocks[i] = null; 			// TODO:如果这里Move()的话,会报空引用
		}
        field.RefreshCells();
        field.ClearFullRow();// ClearFullRow();
		yield return new WaitForSeconds(lockDelay);
		isDropOver = true;
		SpawnNewTetris();

	}


	public void SpawnNewTetris(){
		Debug.Log("SpawnNewTetris");
		// nextDroptime = 0;
        currentShapeID = Random.Range(1, 7);

        SpawnTetris(currentShapeID);
	}

}