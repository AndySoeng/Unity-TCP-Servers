## ��UI���
* UIFroms:�洢����UIFormd��·����Ϣ�����ڴ�Resources�ж�̬���ء�
* UIType:�洢UIPrefab��·��������
* BaseView������UI�ĸ�����Ϊ��OnOpen��OnPause��OnResume��OnExit
* UIManager:UI��ܵ���Ҫ�߼�������������UI��ִ��UIManager.Init();��Ϊ����

���ʹ�÷�����  
1. ����GameRoot�ű�����Awake()��Start()�е��ã�
```c#
		//��ʼ��UIManager����
		UIManager.Init();
		//������һ������
		UIManager.Instance.OpenUI(UIFroms.panel_1);
```
2. ����UIFroms�еĽ����Ԥ���壬��������Ӧ��XXXView�ű����̳�BaseView��������������дBaseView�ķ���
```c#
public class PanelView : BaseView
{
	//���˷������ڸý���İ�ť��
	public void OpenPlane2() {
		UIManager.Instance.OpenUI(UIFroms.panel_2);
	}
}
```

## ʹ��
1.�����ڿ�������
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

2.�����ڽ���Ԥ������
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