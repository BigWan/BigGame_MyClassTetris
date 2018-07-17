using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 设置窗口
public class SettingPage : PageBase {

	[Header("控件")]
	public Button btnClose; // 关闭按钮
	public Slider sldSound; // 音量滑块

    private void Awake() {
        btnClose.onClick.AddListener(()=> {
            Hide();
        });
        sldSound.onValueChanged.AddListener((float v) => {
            SoundManager.Instance.SetVolumn(v);
        });
    }


}
