using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum TShape { I, O, Z, S, J, L, T }

public class TetrisShape : MonoBehaviour {

	// I, O, Z, S, J, L, T
	private float[, ] posdata = new float[, ] {
		// xy,xy,xy,xy
		{ 3, 21, 4, 21, 5, 21, 6, 21 },
		{ 4, 21, 5, 21, 4, 20, 5, 20 },
		{ 3, 21, 4, 21, 4, 20, 5, 20 },
		{ 4, 21, 5, 21, 3, 20, 4, 20 },
		{ 3, 21, 4, 21, 5, 21, 5, 20 },
		{ 3, 21, 4, 21, 5, 21, 3, 20 },
		{ 3, 21, 4, 21, 5, 21, 4, 20 }
	};

	private float [,] pivotdata = new float[,]{
		{4.5f,20.5f},
		{4.5f,20.5f},
		{4f,21f},
		{4f,21f},
		{4f,21f},
		{4f,21f},
		{4f,21f}
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
	public Transform pivot; // 旋转中心
	public TetrisBlock[] blocks; // 块的引用

	public void CreateTetris(int index){
		// blocks = new TetrisBlock[4];
		pivot.localPosition = new Vector3 (pivotdata[index,0], pivotdata[index,1]);
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].pos.x = posdata[index,i * 2];
			blocks[i].pos.y = posdata[index,i * 2 + 1];
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
		foreach (var item in blocks) {
			item.pos = item.pos + Vector2.down;
		}
		pivot.localPosition = pivot.localPosition + Vector3.down;
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
			Lock ();
		}
	}

	// 到位
	public void Lock () {
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].isLock = true;
		}
		SpawnNewTetris();
	}

	public void SpawnNewTetris(){

	}

}