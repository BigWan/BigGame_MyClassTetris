using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI管理器 单例
public class UIManager :UnitySingleton<UIManager> {

	public Dictionary<string,PageBase> allPages;		// 所有注册的页面
	public List<PageBase> currentPageNodes;			// 当前的返回按钮逻辑

	// public Transform fixedRoot;
	// public Transform normalRoot;
	// public Transform pooupRoot;
	void Awake(){

	}

	public void PopNode(PageBase page){
		if(currentPageNodes == null)
			currentPageNodes = new List<PageBase>();
		if(CheckIfNeedBack(page)==false){
			return;
		}
		bool isfound = false;
		for (int i = 0; i < currentPageNodes.Count; i++) {
			if(currentPageNodes[i].Equals(page)){
				currentPageNodes.RemoveAt(i);
				currentPageNodes.Add(page);
				isfound = true;
				break;
			}
		}

		if (!isfound){
			currentPageNodes.Add(page);
		}

		HideOldNodes();
	}

	public void HideOldNodes(){
		int count  = currentPageNodes.Count;
		if(count<1) return;
		PageBase page = currentPageNodes[count-1];
		if(page.mode == UIMode.HideOther){
			for (int i =  count-2; i>=0;i--) {
				if(currentPageNodes[i].isActive())
					currentPageNodes[i].Hide();
			}
		}
	}

	public  bool CheckIfNeedBack(PageBase page){
		if(page.type == UIType.Fixed || page.type == UIType.Popup || page.type == UIType.None){
			return false;
		}else if(page.mode == UIMode.NoNeedBack || page.mode == UIMode.DoNothing){
			return false;
		}else{
			return true;
		}
	}

	public void ShowPage(PageBase page){
		page.Show();
	}



}
