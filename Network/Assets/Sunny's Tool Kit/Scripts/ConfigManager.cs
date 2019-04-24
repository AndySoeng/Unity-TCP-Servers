using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;


//配置类
public class ConfigManager :Singleton<ConfigManager>
{
	public IPEndPoint GateServers;

	

	public ConfigManager() {
		//初始化所有服务器ip端口
		GateServers = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);
	}

	
}
