using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI管理器
public  class UIWindowManager :MonoBehaviour {

	public  Dictionary<string,BaseUIWindow> allPages;		// 所有注册的页面
	public  List<BaseUIWindow> currentPageNodes;			// 当前的返回按钮逻辑

	// public Transform fixedRoot;
	// public Transform normalRoot;
	// public Transform pooupRoot;

	public  void PopNode(BaseUIWindow page){
		if(currentPageNodes == null)
			currentPageNodes = new List<BaseUIWindow>();
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

	public  void HideOldNodes(){
		int count  = currentPageNodes.Count;
		if(count<1) return;
		BaseUIWindow page = currentPageNodes[count-1];
		if(page.mode == UIMode.HideOther){
			for (int i =  count-2; i>=0;i--) {
				if(currentPageNodes[i].isActive())
					currentPageNodes[i].Hide();
			}
		}

	}

	public  bool CheckIfNeedBack(BaseUIWindow page){
		if(page.type == UIType.Fixed || page.type == UIType.Popup || page.type == UIType.None){
			return false;
		}else if(page.mode == UIMode.NoNeedBack || page.mode == UIMode.DoNothing){
			return false;
		}else{
			return true;
		}
	}

	public void ShowPage(BaseUIWindow page){
		page.Show();
	}



}
