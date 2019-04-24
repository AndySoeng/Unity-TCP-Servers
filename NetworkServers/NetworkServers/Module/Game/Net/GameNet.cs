using NetworkServers.Base;
using NetworkServers.Module.Game.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkServers.Module.Game.Net {
	class GameNet : Singleton<GameNet> {
		public GameNet() {
			//将操作码以及事件添加到MessageCenter中的字典
			MessageCenter.instance.AddObserver(OperateCode.DRAWONECARD_REQ, DRAWONECARD_REQ);
			MessageCenter.instance.AddObserver(OperateCode.DRAWONECARD_REP, DRAWONECARD_REP);
		}

		

		public  void DRAWONECARD_REQ(object data, Socket client) {
			MessageCenter.dealCount++;

			//事件实现
			Console.WriteLine("----------------------------------------------");
			Console.WriteLine("GateServers处理模块:From" + client.RemoteEndPoint + "[" + data.ToString() + "] Send to DrawOneCardServers");
			Console.WriteLine("----------------------------------------------");
			//MessageCenter.instance.Send()
			//将此消息随机发往一个GameServers
			Random random = new Random();
			int index = random.Next(0, ServersManager.allServers.Count);
			//生成数据模型
			Game_Dto game_Dto = new Game_Dto(client.RemoteEndPoint.ToString(), data);
			MessageCenter.instance.Send(OperateCode.DRAWONECARD_REQ, game_Dto, ServersManager.allServers[index].Client);
		}

		private void DRAWONECARD_REP(object data, Socket client) {
			MessageCenter.dealCount++;

			Game_Dto game_Dto = JsonConvert.DeserializeObject<Game_Dto>(data.ToString());
			//client是gameservers的地址，那我该怎么把信息发给客户端？？？
			MessageCenter.instance.Send(OperateCode.DRAWONECARD_REP, game_Dto.data, ServersManager.allClientsDic[game_Dto.ipProt].Client);
		}
	}
}
