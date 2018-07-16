using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour {
	public object data;

	// 初始化
	public virtual void Initialize(){
		Debug.Log(myName + " Initialize");
	}
	// 激活
	public virtual void Activate(){
		gameObject.SetActive(true);
		Debug.Log(myName + " Activate");
	}
	// 关闭
	public virtual void Hide(){
		gameObject.SetActive(false);
		Debug.Log(myName + " Hide");
	}
	public virtual void Refresh(){
		Debug.Log(myName + " Refresh");
	}

	public string myName{
		get{return gameObject.name;}
	}
}
