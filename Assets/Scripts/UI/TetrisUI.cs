using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 游戏界面窗口
public class TetrisUI : UIWindow {

	// 控件
	public Playfield playGround;

	[Header("等级得分")]
	public Text score;
	public Text level;

	[Header("操作按钮")]
	public Button btn_pause;
	public Button btn_up;
	public Button btn_down;
	public Button btn_left;
	public Button btn_right;

	// 初始化
	public override void Initialize(){}
	// 激活
	public override void Activate(){
		base.Activate();
	}
	// 关闭
	public override void Hide(){
		base.Hide();
	}


}
