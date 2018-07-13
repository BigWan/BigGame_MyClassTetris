using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 登陆界面窗口
public class MainUI : MonoBehaviour {
	[Header("Panle")]
	public Transform LogoPanel;
	public Transform ButtonsPanel;
	public Transform AnimPanel;

	[Header("GameObject")]
	public GameManager gameManager;

	[Header("Buttons")]
	public Button btnStart;
	public Button btnSetting;

	public void Show(){
		Debug.Log("显示主界面");
		this.gameObject.SetActive(true);
		btnStart.onClick.AddListener(OnbtnStart_Click);
		btnSetting.onClick.AddListener(OnbtnSetting_Click);
	}

	public void Hide(){
		this.gameObject.SetActive(false);
		btnStart.onClick.RemoveListener(OnbtnStart_Click);
	}

	public void OnbtnStart_Click(){
		gameManager.StartGame();
		this.Hide();
	}
	public void OnbtnSetting_Click(){

	}

}
