using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour {

    private Vector2Int pos;
    public Vector2Int Coord {
        get {
            return pos;
        }
        set{
            pos = value;
        }
    }

    public Color color;

	private void Update () {
	    transform.localPosition = new Vector3 (pos.x, pos.y, 0);
        // transform.GetComponent<MeshRenderer>().material.color = color;
	}

    public int X {
        get { return (pos.x); }
    }
    public int Y {
        get { return (pos.y); }
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