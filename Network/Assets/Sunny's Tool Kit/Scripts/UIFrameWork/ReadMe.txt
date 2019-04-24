## 简单UI框架
* UIFroms:存储所有UIFormd的路径信息，用于从Resources中动态加载。
* UIType:存储UIPrefab的路径和名字
* BaseView：定义UI的各种行为，OnOpen，OnPause，OnResume，OnExit
* UIManager:UI框架的主要逻辑，创建和销毁UI，执行UIManager.Init();后为单例

框架使用方法：  
1. 创建GameRoot脚本，在Awake()或Start()中调用：
```c#
		//初始化UIManager单例
		UIManager.Init();
		//创建第一个界面
		UIManager.Instance.OpenUI(UIFroms.panel_1);
```
2. 创建UIFroms中的界面的预制体，并创建相应的XXXView脚本，继承BaseView，即可在其中重写BaseView的方法
```c#
public class PanelView : BaseView
{
	//将此方法绑定在该界面的按钮上
	public void OpenPlane2() {
		UIManager.Instance.OpenUI(UIFroms.panel_2);
	}
}
```

## 使用
1.挂在在空物体上
GameRoot.cs
```c#
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Init();
    }

	public void Init() {
		UIManager.Init();
		UIManager.Instance.OpenUI(UIFroms.panel_1);
	}
}
```

2.挂在在界面预制物上
PanelView.cs
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelView : BaseView {
	public void OpenPlane1() {
		UIManager.Instance.OpenUI(UIFroms.panel_1);
	}
}
```