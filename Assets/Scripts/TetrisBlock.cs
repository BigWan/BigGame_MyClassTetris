using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour {

    public Vector2Int pos;

	public bool isLock = false; // move or lock
    public Color color;

	private void Update () {
	    transform.localPosition = new Vector3 (pos.x, pos.y, 0);
	}

    public int X {
        get { return (pos.x); }
    }
    public int Y {
        get { return (pos.y); }
    }

    public Vector2Int Coord {
        get {
            return new Vector2Int(X, Y);
        }
    }

    public void DropDown() {
        pos = pos + Vector2Int.down;
    }
    // 爆炸
    public void Explosion(){
		// 播放效果
		GameObject.DestroyImmediate(gameObject);
	}
}