using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 登陆界面窗口
public class MainUI : UIWindow {
	[Header("Panle")]
	public Transform LogoPanel;
	public Transform ButtonsPanel;
	public Transform AnimPanel;

	[Header("GameObject")]
	public GameManager gameManager;

	[Header("Buttons")]
	public Button btnStart;
	public Button btnSetting;

	private bool isInit;
	// 私有成员
	public override void Initialize(){
		base.Initialize();
		btnStart.onClick.AddListener(OnbtnStart_Click);
		btnSetting.onClick.AddListener(OnbtnSetting_Click);
		isInit = true;
	}

	public override void Activate(){
		if(!isInit) Initialize();
		base.Activate();
	}

	public override void Hide(){
		base.Hide();
	}

	public void OnbtnStart_Click(){
		gameManager.StartGame();
	}
	public void OnbtnSetting_Click(){

	}

}
