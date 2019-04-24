using NetworkServers.Module.Game.Net;
using NetworkServers.Module.GameServers.Net;
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

namespace NetworkServers.Base {

	class ServersManager {
		/// <summary>
		/// 异步接收消息的缓冲区
		/// </summary>
		public byte[] receiveBuffer = new byte[1024];

		/// <summary>
		/// 临时存储接收到的数据的存储区
		/// </summary>
		public List<byte> receiveCache = new List<byte>();

		/// <summary>
		/// 是否正在处理存储区
		/// </summary>
		public bool isHandleReceiveCache = false;




		//所有用户集合
		public static List<ClientPeer> allClients = new List<ClientPeer>();

		//Gate服务器与客户端收发消息使用
		public static Dictionary<string, ClientPeer> allClientsDic = new Dictionary<string, ClientPeer>();

		public static List<ClientPeer> allServers = new List<ClientPeer>();

		/// <summary>
		/// 服务器socket
		/// </summary>
		public static Socket serverSocket;

		public static Thread sendThread;



		public ServersManager() {
			MessageCenter.Init();
			GameServersNet.Init();
			GameNet.Init();
			

			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 10001);
			try {
				serverSocket.Bind(iPEndPoint);
				Console.WriteLine("GateServers，启动！");
				serverSocket.Listen(1000);
				//开启发送线程
				sendThread = new Thread(Send);
				sendThread.Start();
			}
			catch (Exception e) {

				Console.WriteLine(e.Message);
			}

			StartAccept(null);
			//while (true) {
			//	Socket client = serverSocket.Accept();
			//	Console.WriteLine("客户端：" + client.RemoteEndPoint + "连入成功");
			//	allClients.Add(new ClientPeer(client));

			//}
		}

		/// <summary>
		/// 接收客户端连接
		/// </summary>
		/// <param name="e">  表示异步套接字操作。</param>
		public static void StartAccept(SocketAsyncEventArgs e) {
			/*
			 * SocketAsyncEventArgs 对象的使用，首先要实例化
			 * 然后要绑定完成事件的回调
			 * 
			 */

			if (e == null) {
				e = new SocketAsyncEventArgs();
				e.Completed += AcceptComplete_CallBack;
			}
			bool result = serverSocket.AcceptAsync(e);

			//result为false的时候，才是接收完毕
			if (result == false) {
				HandleAccept(e);
			}
		}

		/// <summary>
		/// 接收客户端连接的回调,连接完成时调用
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void AcceptComplete_CallBack(object sender, SocketAsyncEventArgs e) {
			HandleAccept(e);
		}



		/// <summary>
		/// 处理接受
		/// </summary>
		/// <param name="e"></param>
		public static void HandleAccept(SocketAsyncEventArgs e) {
			Socket client = e.AcceptSocket;
			Console.WriteLine("客户端：" + client.RemoteEndPoint + "连入GateServers成功");
			ClientPeer clientPeer = new ClientPeer(client);
			//将连入添加入用户字典和用户队列，如果是服务器，则服务器会发消息让GateServers把自己从用户字典和用户队列中删除
			//Console.WriteLine("HandleAccept"+clientPeer.Client.RemoteEndPoint);
			allClients.Add(clientPeer);
			allClientsDic.Add(clientPeer.Client.RemoteEndPoint.ToString(), clientPeer);
			//重置接收
			e.AcceptSocket = null;
			StartAccept(e);
		}

		public static byte[] Compress(byte[] sendData) {
			using (MemoryStream outStream = new MemoryStream()) {
				using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true)) {
					zipStream.Write(sendData, 0, sendData.Length);
					zipStream.Close();

					byte[] compressData = outStream.ToArray();
					Console.WriteLine("[发送压缩前]：" + sendData.Length + "	[发送压缩后]：" + compressData.Length);
					return compressData;
				}
			}
		}

		public static void Send() {
			while (true) {
				if (MessageCenter.instance.sendQueue.Count > 0) {
					ClientMessage clientMessage = MessageCenter.instance.sendQueue.Dequeue();
					SocketMessage socketMessage = clientMessage.Message;
					string dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(socketMessage);
					byte[] dataBody = Encoding.UTF8.GetBytes(dataJson);
					byte[] compressBody = Compress(dataBody);
					using (MemoryStream ms = new MemoryStream()) {
						using (BinaryWriter bw = new BinaryWriter(ms)) {
							bw.Write(compressBody.Length);
							bw.Write(compressBody);

							byte[] wholePacket = new byte[(int)ms.Length];
							Buffer.BlockCopy(ms.GetBuffer(), 0, wholePacket, 0, (int)ms.Length);
							try {
								//clientMessage.Client.Send(wholePacket);
								clientMessage.Client.BeginSend(wholePacket, 0, wholePacket.Length, SocketFlags.None, null, null);
								Console.WriteLine("发送模块：[" + clientMessage.Client.RemoteEndPoint + "]-[" + socketMessage.OpCode + "]-[" + socketMessage.Data + "]");
								MessageCenter.sendCount++;
								Console.WriteLine("接收次数:" + MessageCenter.receiveCount + " 处理次数：" + MessageCenter.dealCount + " 发送次数" + MessageCenter.sendCount);
								Console.WriteLine("----------------------------------------------");

							}
							catch (Exception e) {
								Console.WriteLine(e.Message);
							}
						}
					}
				}
			}

		}
	}

}
