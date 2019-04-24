using GameServers.Base;
using GameServers.Module.Game.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Module.Game.Net {
	class GameNet : Singleton<GameNet> {
		public GameNet() {
			//将操作码以及事件添加到MessageCenter中的字典
			MessageCenter.instance.AddObserver(OperateCode.DRAWONECARD_REQ, DRAWONECARD_REQ);
		}

		public void DRAWONECARD_REQ(object data, Socket client) {
			MessageCenter.dealCount++;


			Game_Dto game_Dto = JsonConvert.DeserializeObject<Game_Dto>(data.ToString());
			//事件实现
			Console.WriteLine("----------------------------------------------");
			Console.WriteLine("GameServers处理模块：[" + game_Dto.data.ToString() + "] From GateServers:"+client.RemoteEndPoint);
			Console.WriteLine("----------------------------------------------");
			game_Dto.data = "送你一个孙科孙科孙科孙科孙科";
			//MessageCenter.instance.Send()
			MessageCenter.instance.Send(OperateCode.DRAWONECARD_REP, game_Dto, client);
		}
	}
}
