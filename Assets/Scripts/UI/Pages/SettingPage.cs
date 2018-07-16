using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 设置窗口
public class SettingPage : PageBase {

	public Button btnClose; // 关闭按钮
	public Slider sldSound; // 音量滑块

	public override void Show(){
		btnClose.onClick.AddListener(OnbtnClose_Click);
		sldSound.onValueChanged.AddListener(OnsldSound_Change);
	}

	public override void Initialize(){

	}

	public override void Hide(){
		base.Hide();

	}

	void OnbtnClose_Click(){
		// btnClose.onClick.RemoveAllListeners();
		// sldSound.onValueChanged.RemoveAllListeners();
		this.gameObject.SetActive(false);
	}

	void OnsldSound_Change(float value){
		// audio.volume = sldSound.value;
	}
}
