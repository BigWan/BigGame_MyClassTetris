using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  GameStat {
	Ready,				// UI界面
	Playing,			// 进行中
	End					// 结束
}

public class GameManager : UnitySingleton<GameManager> {

	void Awake(){
		UIManager.Instance.ShowPage("MainPage");
	}

	public void StartGame(){
        UIManager.Instance.ShowPage("TetrisPage");
        TetrisManager.Instance.StartGame();
    }

}


