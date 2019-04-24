using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameServers.Base;

namespace GameServers {
	class Program {
		public static  ClientPeer client;
		static void Main(string[] args) {

			ServersManager servers = new ServersManager();



			client = new ClientPeer("127.0.0.1",10001);
			//给Gate服务器发送自己是服务器的消息
			MessageCenter.instance.Send(OperateCode.CONNECTGATE_REQ,"我是GameServers",client.Client);
		}
	}
}
