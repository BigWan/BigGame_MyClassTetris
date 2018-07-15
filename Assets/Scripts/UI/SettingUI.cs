using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour {

	public Button btnClose; // 关闭按钮
	public Slider sldSound; // 音量滑块

	// public AudioSource audio;
	void Awake(){
		btnClose.onClick.AddListener(OnbtnClose_Click);
		sldSound.onValueChanged.AddListener(OnsldSound_Change);
	}

	void OnbtnClose_Click(){
		btnClose.onClick.RemoveAllListeners();
		sldSound.onValueChanged.RemoveAllListeners();
		this.gameObject.SetActive(false);
	}

	void OnsldSound_Change(float value){
		// audio.volume = sldSound.value;
	}
}
