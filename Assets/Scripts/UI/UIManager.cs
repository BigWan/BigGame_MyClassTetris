using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI管理器 单例
public class UIManager :UnitySingleton<UIManager> {

	 [ContextMenuItem ("自动引用", "GetAutoRef")]
	public PageBase[] allPages;

	public List<PageBase> currentPageNodes;			// 当前的返回按钮逻辑


	private Dictionary<string,PageBase> pageDic;		// 以字典形式存储的注册过的页面

	[ContextMenu ("自动引用")]
	private void GetAutoRef(){
		List<PageBase> pages = new List<PageBase>();
		GetComponentsInChildren<PageBase>(true,pages);
		Debug.Log("[UIManager] 在UIManager物体下找到" + pages.Count + "个Page元素！");
		allPages = pages.ToArray();
	}

	void Awake(){
		pageDic = new Dictionary<string, PageBase>();
		if(allPages.Length > 0){
			for (int i = 0; i < allPages.Length; i++) {
				pageDic.Add(
                    allPages[i].GetType().ToString(),
                    allPages[i]
                );
                allPages[i].Hide();
            }
            //HideAll();
        }
        
    }

	public void PopNode(PageBase page){
		if(currentPageNodes == null)
			currentPageNodes = new List<PageBase>();
		if(CheckIfNeedBack(page)==false){
            Debug.Log("不需要回退");
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
		int count = currentPageNodes.Count;
		if(count<1) return;
		PageBase page = currentPageNodes[count-1];
		if(page.mode == UIMode.HideOther){
			for (int i = count-2; i>=0;i--) {
				//if(currentPageNodes[i].isActiveAndEnabled)
					currentPageNodes[i].Hide();
			}
		}
	}

    //// 隐藏其他不再导航内的界面
    //public void HideOtherNodes() {
    //    for (int i = 0; i < allPages.Length; i++) {
    //        if (allPages[i].GetType().ToString() != "") {

    //        }
    //    }
    //}

	public bool CheckIfNeedBack(PageBase page){
        
		if(page.type == UIType.Fixed || page.type == UIType.Popup || page.type == UIType.None){
			return false;
		}else if(page.mode == UIMode.NoNeedBack || page.mode == UIMode.DoNothing){
			return false;
		}else{
			return true;
		}
	}

    /// <summary>
    /// 对注册过的页面调用Show方法
    /// </summary>
    /// <param name="pageName">page的物体的名字</param>
	public void ShowPage(string pageName){
		if(pageDic.ContainsKey(pageName)){
			pageDic[pageName].Show();
		}else{
			Debug.LogError(pageName + " UI 没有引用" );
		}
	}

    //public void HideAll() {
    //    for (int i = 0; i < allPages.Length; i++) {
    //        allPages[i].Hide();
    //    }
    //}

}
