using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// 存放UI框架的逻辑
/// </summary>
public class UIManager : Singleton<UIManager> {



	/// <summary>
	/// 生成的UI对象
	/// </summary>
	public Stack<BaseView> uiViewsStack = new Stack<BaseView>();

	/// <summary>
	/// 所有生成的UI物体
	/// </summary>
	public Dictionary<UIType, GameObject> allUIFormsDic = new Dictionary<UIType, GameObject>();

	private Transform canvas = null;

	public UIManager() {
		if (canvas == null) {
			canvas = GameObject.Find("Canvas").transform;
		}
	}
	/// <summary>
	/// 打开一个UI，入栈
	/// 伪代码：算法描述
	/// 如果栈中没有元素，那么调用自己的OnOpen
	/// 如果已经已经有了元素，先调用上一个元素的OnPause,再调用这个的OnOpen
	/// 入栈
	/// 
	/// </summary>
	public BaseView OpenUI(UIType uiType) {
		BaseView thisView = GetUI(uiType);
		if (uiViewsStack.Count != 0) {
			uiViewsStack.Peek().OnPause();
		}
		thisView.OnOpen(uiType);
		uiViewsStack.Push(thisView);
		return thisView;
		/*GB
		if (uiViewsStack.Count==0) {
			thisView.OnOpen(uiType);
			uiViewsStack.Push(thisView);
			return;
		}
		BaseView lastView = uiViewsStack.Peek();
		lastView.OnPause();
		thisView.OnOpen(uiType);
		uiViewsStack.Push(thisView);
		*/
	}

	/// <summary>
	/// 销毁一个UI，出栈
	/// 做一个安全性判断：
	/// 先判断，栈中是否有元素，没有，直接返回，不做操作
	/// 有元素,从栈中移除，执行OnExit()，销毁这个物体
	/// 判断栈中是否还有元素
	/// 如果没有，跳过
	/// 如果有,渠道栈顶元素，执行OnResume()
	/// </summary>
	public void CloseUI() {
		if (uiViewsStack.Count != 0) {
			BaseView firstView = uiViewsStack.Pop();
			firstView.OnExit();
			DestroyUI(firstView.Type); //销毁栈顶元素

		}
		if (uiViewsStack.Count != 0) {
			BaseView secondView = uiViewsStack.Peek();
			secondView.OnResume();
		}

		/*GB
		if (uiViewsStack.Count == 0) {
			return;
		}
		BaseView firstView = uiViewsStack.Pop();//要销毁的栈顶元素
		firstView.OnExit();
		DestroyUI(firstView.Type); //销毁栈顶元素
		if (uiViewsStack.Count == 0) {
			return;
		}
		BaseView secondView = uiViewsStack.Peek();
		secondView.OnResume();
		*/
	}

	/// <summary>
	/// 获取一个UI
	/// 从字典中检测这个类型的UI是否存在
	/// 如果不存在，生成一个新的，返回，并把这个UI存到字典中
	/// 如果存在，直接返回
	/// </summary>
	/// <param name="uiType"></param>
	/// <returns></returns>
	public BaseView GetUI(UIType uiType) {
		//不存在
		if (allUIFormsDic.ContainsKey(uiType) == false || allUIFormsDic[uiType] == null) {
			GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path), canvas);
			go.transform.localScale = Vector3.one;
			allUIFormsDic[uiType] = go;
			return go.GetComponent<BaseView>();
		}
		return allUIFormsDic[uiType].GetComponent<BaseView>();
	}


	/// <summary>
	/// 从字典中删除UI
	/// </summary>
	public void DestroyUI(UIType type) {
		if (allUIFormsDic.ContainsKey(type)) {
			if (allUIFormsDic[type] != null) {
				GameObject.Destroy(allUIFormsDic[type].gameObject);
			}
			//从字典中删除
			allUIFormsDic.Remove(type);
			return;
		}

	}



	#region UI组件工具方法

	/// <summary>
	/// Button绑定点击事件
	/// </summary>
	/// <param name="baseView">当前界面，直接使用gameobject即可</param>
	/// <param name="uiName">要绑定事件的ui名字</param>
	/// <param name="eventName">绑定的事件</param>
	public static void BindOnclickEvent(GameObject go, UnityAction eventName) {
		go.GetComponent<Button>().onClick.AddListener(eventName);
	}

	/// <summary>
	/// Toggle绑定OnValueChange事件
	/// </summary>
	/// <param name="go"></param>
	/// <param name="eventName"></param>
	public static void BindOnValueChangeEvent(GameObject go, UnityAction<bool> eventName) {
		go.GetComponent<Toggle>().onValueChanged.AddListener(eventName);
	}

	/// <summary>
	/// 根据类型返回UI组件
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T GetUIComponent<T>(GameObject go, string uiName) {
		return go.transform.GetChild(0).Find(uiName).GetComponent<T>();
	}



	/// <summary>
	/// 切换两个UI界面
	/// </summary>
	/// <param name="targetViewType">要切换的目标View</param>
	/// <param name="textName">如果是NoticeView，则显示content内容</param>
	public void SwitchView(UIType targetViewType, string content = "", string textName = "NoticeText") {
		//目标是noticeView时要修改显示内容
		if (targetViewType == UIFroms.NoticeView) {
			//生成一个提示界面 并且显示提示信息
			OpenUI(targetViewType).transform.Find("root/NoticeText").GetComponent<Text>().text = content;
		}
		else {
			OpenUI(targetViewType);
		}
	}

	/// <summary>
	/// 根据名字返回一个指定路径下名字相同的sprite
	/// </summary>
	public static Sprite GetOneSpriteWithName(string path, string name) {
		Sprite[] sprites = Resources.LoadAll<Sprite>(path);
		for (int i = 0; i < sprites.Length; i++) {
			if (sprites[i].name == name) {
				return sprites[i];
			}
		}
		return null;
	}

	/// <summary>
	/// 根据指定路径下的图片在指定的父物体下创建指定的Prefab，数量和路径下的相同
	/// </summary>
	/// <param name="parent">父物体</param>
	/// <param name="childPreab">子物体预制物</param>
	/// <param name="path">资源路径</param>
	/// <param name="pointObject">要设置选中状态的物体名字</param>
	/// <returns>创建的物体对象数组</returns>
	public static GameObject[] CreateScrollViewContent(GameObject parent, GameObject childPreab, string path, string pointObject = "") {
		Sprite[] sprites = Resources.LoadAll<Sprite>(path);
		GameObject[] gos = new GameObject[sprites.Length];
		for (int i = 0; i < sprites.Length; i++) {
			GameObject go = GameObject.Instantiate(childPreab, parent.transform);
			go.GetComponent<Image>().sprite = sprites[i];
			go.name = sprites[i].name;
			if (go.name == pointObject) {
				go.transform.GetChild(0).gameObject.SetActive(true);
			}
			gos[i] = go;
		}
		return gos;
	}


	#endregion

}
