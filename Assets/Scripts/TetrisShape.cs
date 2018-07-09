using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum TShape { I, O, Z, S, J, L, T }

public class TetrisShape : MonoBehaviour {

	// I, O, Z, S, J, L, T
	private float[][] posdata ={
		new float[]{ 3, 21, 4, 21, 5, 21, 6, 21 }, // I
		new float[]{ 4, 21, 5, 21, 4, 20, 5, 20 }, // O
		new float[]{ 3, 21, 4, 21, 4, 20, 5, 20 }, // Z
		new float[]{ 4, 21, 5, 21, 3, 20, 4, 20 }, // S
		new float[]{ 3, 21, 4, 21, 5, 21, 5, 20 }, // J
		new float[]{ 3, 21, 4, 21, 5, 21, 3, 20 }, // L
		new float[]{ 3, 21, 4, 21, 5, 21, 4, 20 }  // T
	};

	private float [][] pivotdata = {
		new float[]{4.5f,20.5f},
		new float[]{4.5f,20.5f},
		new float[]{4f,21f},
		new float[]{4f,21f},
		new float[]{4f,21f},
		new float[]{4f,21f},
		new float[]{4f,21f}
	};

	public Color[] colors= new Color[]{
		Color.red,
		Color.green,
		Color.cyan,
		Color.black,
		Color.yellow,
		Color.gray,
		Color.white
	};

	public float speed;
	private float currentTime;
	public int currentType = 0;

	public TetrisBlock blockPrefab;
	public Transform pivot; // 旋转中心
	public Playfield field; 	// 地图
	public TetrisBlock[] blocks; // 块的引用

	public void CreateTetris(int index){
		int rnk = 4;
		blocks = new TetrisBlock[rnk];
		pivot.localPosition = new Vector3 (pivotdata[index][0], pivotdata[index][1]);
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i] = GameObject.Instantiate<TetrisBlock>(blockPrefab) as TetrisBlock;
			blocks[i].pos.x = posdata[index][i * 2];
			blocks[i].pos.y = posdata[index][i * 2 + 1];
		}
	}

	public void Rotation () {
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].pos = blocks[i].pos.RotateClockWise (pivot.localPosition);
		}
	}

	void MoveLeft () {
		foreach (var item in blocks) {
			item.pos = item.pos + Vector2.left;
		}
		pivot.localPosition = pivot.localPosition + Vector3.left;
	}

	void MoveRight () {
		foreach (var item in blocks) {
			item.pos = item.pos + Vector2.right;
		}
		pivot.localPosition = pivot.localPosition + Vector3.right;
	}

	void MoveDown () {
		if (CheckIsBottom(Vector2.down)){
			LockShape();
			return;
		}
		foreach (var item in blocks) {
			item.pos = item.pos + Vector2.down;
		}
		pivot.localPosition = pivot.localPosition + Vector3.down;
	}


	bool CheckIsBottom(Vector2 dir){
		for (int i = 0; i < blocks.Length; i++) {
			float x = blocks[i].pos.x + dir.x;
			float y = blocks[i].pos.y+dir.y;
			if(field.cells[Mathf.RoundToInt(x),Mathf.RoundToInt(y)]>0){
				return true;
			}
		}
		return false;
	}


	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime < speed){

		} else{
			currentTime -= speed;
			MoveDown();
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			Rotation ();
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			CreateTetris (currentType);
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			LockShape ();
		}
	}

	// 到位
	public void LockShape () {
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].isLock = true;
			blocks[i] = null;
		}
		CheckRow();
		SpawnNewTetris();
	}


	public void CheckRow(){

	}
	public void SpawnNewTetris(){

	}

}