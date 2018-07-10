using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TetrisManager : MonoBehaviour {



	private int[][] posdata = {
		new int[]{ 3, 21, 4, 21, 5, 21, 6, 21 }, // I
		new int[]{ 4, 21, 5, 21, 4, 20, 5, 20 }, // O
		new int[]{ 3, 21, 4, 21, 4, 20, 5, 20 }, // Z
		new int[]{ 4, 21, 5, 21, 3, 20, 4, 20 }, // S
		new int[]{ 3, 21, 4, 21, 5, 21, 5, 20 }, // J
		new int[]{ 3, 21, 4, 21, 5, 21, 3, 20 }, // L
		new int[]{ 3, 21, 4, 21, 5, 21, 4, 20 }  // T
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
		Color.cyan,
		Color.yellow,   // 黄色O
		Color.red,      // 红色Z
		Color.green,    // 绿色S
		Color.blue,     // 蓝色J
		new Color(1f,165f/255f,0),
        new Color(0.5f,0,0.5f)
    };

	public float speed = 0.25f;
	private float currentTime;
	private int currentType = 0;


    public Transform blockContainer ;

	public TetrisBlock blockPrefab;
	public Transform pivot;          // 旋转中心
	public Playfield field; 	     // 地图
	public TetrisBlock[] blocks;     // 块的引用

	public void CreateTetris(int index){
		int rnk = 4;
		blocks = new TetrisBlock[rnk];
		pivot.localPosition = new Vector3 (pivotdata[index][0], pivotdata[index][1]);
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i] = GameObject.Instantiate<TetrisBlock>(blockPrefab) as TetrisBlock;
            blocks[i].GetComponent<MeshRenderer>().material.color = colors[index];
            blocks[i].pos = new Vector2Int(posdata[index][i * 2], posdata[index][i * 2 + 1]);
		}
	}

	public void Rotation () {
		Vector2Int[] newpos = new Vector2Int[4];
		// 能不能转
		for (int i = 0; i < blocks.Length; i++) {
			newpos[i] = Vector2Util.RotateClockWise(blocks[i].pos, pivot.localPosition);
		}
		if(!field.CanAction(newpos)) return;

		// 能转
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].pos = newpos[i];
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
        }
	}

    void DropDown() {
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
            LockShape();
            return;
        }
    }

	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime < speed){

		} else{
			currentTime -= speed;
            DropDown();
		}
		if (Input.GetKeyDown (KeyCode.UpArrow))     { Rotation ();		        }
		if (Input.GetKeyDown (KeyCode.LeftArrow))   { Move (Vector2Int.left);		}
		if (Input.GetKeyDown (KeyCode.RightArrow))  { Move (Vector2Int.right);		}
		if (Input.GetKeyDown (KeyCode.Space))       { Rotation ();		        }
		if (Input.GetKeyDown (KeyCode.DownArrow))   { SpawnNewTetris();		}
		//if (Input.GetKeyDown (KeyCode.C))           { LockShape ();		}
	}

	// 到位
	public void LockShape () {
        Debug.Log("Lock a Shape");
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].isLock = true;
            blocks[i].transform.SetParent(blockContainer);
            //field.cells[blocks[i].X, blocks[i].Y] = currentType+1;
            field.SetCell(blocks[i]);
            blocks[i] = null;
		}
        field.RefreshCells();
        field.ClearFullRow();// ClearFullRow();
		SpawnNewTetris();
	}


	public void SpawnNewTetris(){
		Debug.Log("SpawnNewTetris");
        currentType = Random.Range(1, 7);

        CreateTetris(currentType);
	}

}