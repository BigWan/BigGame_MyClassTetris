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

	//private bool isInit;
    // 私有成员
    public void Awake() {
        btnStart.onClick.AddListener(
            () => {
                GameManager.Instance.StartGame();
            }
        );
        btnSetting.onClick.AddListener(() => {
            UIManager.Instance.ShowPage("SettingPage");
        });
    }
 //   public override void Initialize(){
	//	base.Initialize();
	//	isInit = true;
	//	btnStart.onClick.AddListener(
	//		()=>{
 //               Debug.Log("这都是啥啊");
	//			//GameManager.Instance.StartGame();
	//		}
	//	);
	//	btnSetting.onClick.AddListener(()=>{});

	//}

	//public override void Show(){
	//	base.Show();
	//}

	//public override void Hide(){
	//	base.Hide();

	//}



}
