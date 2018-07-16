using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum  GameStat {
	Ready,				// UI界面
	Playing,			// 进行中
	End					// 结束
}

public class GameManager : UnitySingleton<GameManager> {

	[Header("Manager")]
	public TetrisManager tetrisMgr;
	public SoundManager soundMgr;

	void Awake(){
		Debug.Assert(tetrisMgr!=null && soundMgr!=null,"管理器空引用");
		// UIWindowManager.mgr.ShowPage();
	}

	public void StartGame(){
		Debug.Log("开始游戏");
		// UIWindowManager.mgr.ShowPage();
	}

}


