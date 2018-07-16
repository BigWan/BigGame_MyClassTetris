using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI管理器
public class UIWindowManager : MonoBehaviour {

	// view
	public UIWindow mainUI;
	public UIWindow tetrisUI;

	public void ShowMainUI(){
		mainUI.Activate();
		tetrisUI.Hide();
	}

	public void ShowTetrisUI(){
		mainUI.Hide();
		Debug.Log("SHowTetris");
		tetrisUI.Activate();
	}



}
