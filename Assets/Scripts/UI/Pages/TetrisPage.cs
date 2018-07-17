using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 游戏界面窗口
public class TetrisPage : PageBase {

	// Ref
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

    public void Awake() {
        TetrisManager.Instance.ScoreChange += OnScoreChange;
    }

    void OnScoreChange(object o,System.EventArgs e) {
        score.text = (o as TetrisManager).score.ToString();
    }

}
