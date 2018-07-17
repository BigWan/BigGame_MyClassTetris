using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivateShapeManager : UnitySingleton<ActivateShapeManager> {

	public TetrisBlock blockPrefab;  // 方块预制体
	private TetrisBlock[] blocks;     	// 块的引用

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

	private float [][] pivotdata = {
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
	public void SpawnTetris(int index){
		if(dropStat!=DropStat.Spawning) return;
		Vector2Int origin = GridManager.Instance.SpwanOrigin;
		int rnk = 4;					// TODO 支持其他类型的block
		blocks = new TetrisBlock[rnk];
		pivot.localPosition = new Vector3 (pivotdata[index][0]+origin.x, pivotdata[index][1]+origin.y);
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i] = Instantiate<TetrisBlock>(blockPrefab) as TetrisBlock;
            blocks[i].GetComponent<SpriteRenderer>().sprite = blocks[i].colors[index];
            blocks[i].Coord = new Vector2Int(posdata[index][i * 2], posdata[index][i * 2 + 1])+origin;
			if(!TetrisManager.Instance.CanAction(blocks[i].Coord)){return;}
		}
	}

	/// <summary>
	/// 方块旋转,踢墙和踢地板
	/// </summary>
	public void Rotation () {
		if(dropStat!=DropStat.Dropping) return;
		Vector2Int[] newpos = new Vector2Int[4];
		// 计算原地旋转后的坐标
		for (int i = 0; i < blocks.Length; i++) {
			newpos[i] = Vector2Util.RotateClockWise(blocks[i].Coord, pivot.localPosition);
		}
		//踢墙检测
		for (int j = 0; j < TickOffsetPoint.Length; j++) {
			Vector2Int[] kickpos = new Vector2Int[4];
			for (int i = 0; i < blocks.Length; i++) {
				kickpos[i] = newpos[i]+Vector2Int.up;
			}

			if(TetrisManager.Instance.CanAction(newpos)) {				// 能转
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
	public void Move(Vector2Int dir){
		if(dropStat!= DropStat.Dropping) return;
		Vector2Int[] newpos = new Vector2Int[4];

		for (int i = 0; i < blocks.Length; i++) {
			newpos[i] = blocks[i].Coord + dir;
		}

        if (TetrisManager.Instance.CanAction(newpos)) {
            for (int i = 0; i < blocks.Length; i++) {
                blocks[i].Coord = newpos[i];
            }
            pivot.localPosition = pivot.localPosition + new Vector3(dir.x,dir.y);
			nextDroptime = 0f;
        }
	}
public float speed = 0.25f;      // 速度
public float nextDroptime; 		// 下次降落时间
	/// <summary>
	/// 正常下落功能
	/// </summary>
	///
	///
    void DropDown() {
		if (nextDroptime >= speed && dropStat == DropStat.Dropping){
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
	public void QuickDropDown(){
		if(dropStat != DropStat.Dropping) return;
		int dis = ClacGhostDistance();
		for (int i = 0; i < 4; i++) {
			blocks[i].Coord = blocks[i].Coord + Vector2Int.down*dis;
		}
		pivot.localPosition = pivot.localPosition + Vector3.down*dis;
		dropStat = DropStat.Delaying;
		LockShape();
	}

	/// <summary>
	/// 计算幽灵的距离
	/// </summary>
	int ClacGhostDistance(){
		// 计算每个方块最大还能移动的距离
		int[] maxdis = new int[4];
		for (int i = 0; i < maxdis.Length; i++) { 	// 遍历blocks
			for (int j = blocks[i].Y-1; j >= 0; j--) { // 计算每列和当前block的高度差,
				if(TetrisManager.Instance.cells[blocks[i].X,j]==0){
					maxdis[i]+=1;
				}else{
					break;
				}
			}
		}
		return maxdis.Min();
	}

		// 停止
	public void LockShape () {
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

}
