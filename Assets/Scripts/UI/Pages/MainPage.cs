using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 登陆界面窗口
public class MainPage : PageBase {
	[Header("Panle")]
	public Transform LogoPanel;
	public Transform ButtonsPanel;
	public Transform AnimPanel;

	// [Header("GameObject")]
	// public GameManager gameManager;

	[Header("Buttons")]
	public Button btnStart;
	public Button btnSetting;

	private bool isInit;
	// 私有成员
	public override void Initialize(){
		base.Initialize();
		btnStart.onClick.AddListener(
			()=>{
				GameManager.Instance.StartGame();
			}
		);
		btnSetting.onClick.AddListener(()=>{});
		isInit = true;
	}

	public override void Show(){
		if(!isInit) Initialize();
		base.Show();
	}

	public override void Hide(){
		base.Hide();
	}



}
