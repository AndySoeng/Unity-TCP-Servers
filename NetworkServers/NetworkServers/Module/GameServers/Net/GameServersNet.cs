using NetworkServers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkServers.Module.GameServers.Net {
	class GameServersNet : Singleton<GameServersNet> {
		public  GameServersNet() {
			//将操作码以及事件添加到MessageCenter中的字典
			MessageCenter.instance.AddObserver(OperateCode.CONNECTGATE_REQ, CONNECTGATE_REQ);
		}

		public  void CONNECTGATE_REQ(object data, Socket client) {
			MessageCenter.dealCount++;

			//找到发消息的GameServers存入allServers
			for (int i = 0; i < ServersManager.allClients.Count; i++) {
				if (ServersManager.allClients[i].Client.RemoteEndPoint.ToString()==client.RemoteEndPoint.ToString()) {
					ClientPeer tempClientPeer = ServersManager.allClients[i];
					//将服务器添加进服务器列表
					ServersManager.allServers.Add(tempClientPeer);
					Console.WriteLine("----------------------------------------------");
					Console.WriteLine("已将"+ tempClientPeer.Client.RemoteEndPoint.ToString() + "添加进服务器列表");
					//并从用户列表和用户字典删除
					ServersManager.allClients.Remove(tempClientPeer);
					ServersManager.allClientsDic.Remove(tempClientPeer.Client.RemoteEndPoint.ToString());
					Console.WriteLine("已将" + tempClientPeer.Client.RemoteEndPoint.ToString() + "从用户列表和用户字典移除");
					Console.WriteLine("----------------------------------------------");
				}
			}
			
			
		}
	}

}
