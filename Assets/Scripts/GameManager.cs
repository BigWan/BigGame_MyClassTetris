using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum  GameStat {
	Ready,				// UI界面
	Begin,				// 开始游戏
    CutDown,			// 倒计时
	Playing,			// 进行中
	End					// 结束
}

public class GameManager : MonoBehaviour {
	public UIManager uiManager;
	public TetrisManager tetrisManager;
	private GameStat gs;

	void Awake(){
		uiManager.ShowMainUI();
		gs = GameStat.Ready;
	}
	public void StartGame(){
		Debug.Log("开始游戏拉祜");
	}
}


