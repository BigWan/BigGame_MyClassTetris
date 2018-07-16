using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum  GameStat {
	Ready,				// UI界面
	Playing,			// 进行中
	End					// 结束
}

public class GameManager : MonoBehaviour {

	[Header("Manager")]
	public UIWindowManager uiMgr;
	public TetrisManager tetrisMgr;
	public SoundManager soundMgr;

	void Awake(){
		Debug.Assert(uiMgr!=null && tetrisMgr!=null && soundMgr!=null,"管理器空引用");
		uiMgr.ShowMainUI();
	}

	public void StartGame(){
		Debug.Log("开始游戏");
		uiMgr.ShowTetrisUI();
	}

}


