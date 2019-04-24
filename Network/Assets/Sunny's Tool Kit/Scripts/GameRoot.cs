using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameRoot : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		Init();
	}

	// Update is called once per frame
	void Update() {

	}

	public ClientPeer client;

	public void Init() {

		//配置管理单例初始化
		ConfigManager.Init();

		//初始化UI工具类单例

		//UI初始化
		UIManager.Init();
		//打开第一个界面
		UIManager.Instance.OpenUI(UIFroms.DrawOneCardView);


		//网络初始化
		MessageCenter.CreateInstance();
		//每个界面网络事件处理的实例化
		//AnnouncementNet.Init();
		GameNet.Init();

		//连接Gate服务器
		client = new ClientPeer(ConfigManager.Instance.GateServers.Address.ToString(), ConfigManager.Instance.GateServers.Port);

	}

	private void OnDestroy() {
		//销毁时关闭客户端连接
			client.Close();
	}
}
