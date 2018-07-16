using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIType {
	Normal,
	Fixed,
	Popup,
	None
}

public enum UIMode{
	DoNothing,
	HideOther,
	NeedBack,
	NoNeedBack
}

public abstract class BaseUIWindow : MonoBehaviour {

	public UIType type = UIType.Normal;
	public UIMode mode = UIMode.DoNothing;


	public object data;

	// private UIWindowManager ui_mgr;
	private bool isAwake = false;
	// 初始化
	// 添加事件监听

	void Awake(){
		Initialize();
	}
	public virtual void Initialize(){
		isAwake = true;
		Debug.Log(Name() + " Initialize");
	}
	// 激活
	public virtual void Show(){
		if(!isAwake) Initialize();
		gameObject.SetActive(true);
		Debug.Log(Name() + " Show");
		Refresh();
		UIWindowManager.PopNode(this);
	}

	public virtual bool isActive(){
		return gameObject!=null&&gameObject.activeSelf;
	}

	// 关闭
	public virtual void Hide(){
		gameObject.SetActive(false);
		data = null;
		Debug.Log(Name() + " Hide");
	}
	public virtual void Refresh(){
		Debug.Log(Name() + " Refresh");
	}

	public string Name(){
		return gameObject.name ;
	}

}
