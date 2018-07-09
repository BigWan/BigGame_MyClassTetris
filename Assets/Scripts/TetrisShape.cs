using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 形状
/// </summary>
public class TetrisShape : MonoBehaviour {

	public Transform[] cubes; 		// 块
	public Vector3Int[] positions;	// 位置
	public Transform pivot; 		// 旋转中心
	public Playfield field;			// 格子

	/// <summary>
	/// 顺时针旋转每个块
	/// </summary>
	public void Rotation () {
		// rotate cubes
		foreach (var item in cubes) {
			item.RotateAround (pivot.localPosition, Vector3.back, 90f);
		}
		// get v3 pos
		for (int i = 0; i < positions.Length; i++) {
			positions[i] = Vector3Int.RoundToInt( cubes[i].localPosition);

		}
	}

	void MoveRight () {
		Move (Vector3.right);
	}

	void MoveLeft () {
		Move (Vector3.left);
	}

	/// <summary>
	/// 朝一个方向移动
	/// </summary>
	/// <param name="dir">移动方向</param>
	void Move (Vector3 dir) {
		foreach (var item in cubes) {
			item.localPosition = item.localPosition + dir;
		}
		pivot.localPosition = pivot.localPosition + dir;
	}

	void MoveDown () {
		Move (Vector3.down);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Rotation ();
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDown ();
		}
		if(Input.GetKeyDown(KeyCode.C)){
			Stop();
		}
	}

	public bool TryMove () {
		return false;
	}

	// 停止
	public void Stop(){
		foreach (var item in cubes) {
			item.SetParent(field.transform,false);
		}

	}
}