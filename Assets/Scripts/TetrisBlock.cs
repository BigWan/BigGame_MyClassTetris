using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour {

	public Vector2 pos;
	public bool isLock = false;   // move or lock

	private void Update() {
		transform.localPosition = new Vector3(pos.x,pos.y,0);
	}

	public Vector2Int GridPos(){
		return new Vector2Int(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y));
	}
}
