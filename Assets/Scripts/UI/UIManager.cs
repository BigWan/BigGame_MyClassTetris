using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI窗口管理器
public class UIManager : MonoBehaviour {

	// view
	public MainUI mainUI;
	public Transform tetrisUI;

	public void ShowMainUI(){
		mainUI.Show();
		tetrisUI.gameObject.SetActive(false);
	}

	public void ShowTetrisUI(){
		mainUI.Hide();
		tetrisUI.gameObject.SetActive(false);
	}



}
