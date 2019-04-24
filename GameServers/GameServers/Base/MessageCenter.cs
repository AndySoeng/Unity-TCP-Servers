using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace GameServers.Base {
	public delegate void ProtocolBindEvent(object data, Socket client);


	public class MessageCenter : Singleton<MessageCenter> {

		public static int receiveCount = 0;

		public static int dealCount = 0;

		public static int sendCount = 0;


		//存入消息和要执行的事件   协议号，委托事件
		public Dictionary<short, ProtocolBindEvent> protocolBindEvent = new Dictionary<short, ProtocolBindEvent>();

		/// <summary>
		/// 发送消息的队列
		/// 当有消息要发送时，存入该队列
		/// 在程序中对该队列进行轮询，有数据就发送
		/// </summary>
		public Queue<ClientMessage> sendQueue = new Queue<ClientMessage>();

		/// <summary>
		/// 存储接收到的消息的队列
		/// 当接收到消息后，存入该队列
		/// 在程序中，对该队列进行轮询，有数据就处理
		/// </summary>
		public Queue<ClientMessage> receiveQueue = new Queue<ClientMessage>();

		public MessageCenter() {

			//轮询：一直调用 处理接收到的消息
			Thread handleReceiveQueue = new Thread(HandleReceiveQueue);
			handleReceiveQueue.Start();
		}



		/// <summary>
		/// 添加一个监听事件
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddObserver(short key, ProtocolBindEvent value) {
			//如果字典中已存在key，多添加一个事件
			if (protocolBindEvent.ContainsKey(key)) {
				protocolBindEvent[key] += value;
			}
			//如果字典中不存在key，添加一个事件
			else {
				protocolBindEvent[key] = value;
			}
		}


		public void Send(short opCode, object data, Socket client) {
			SocketMessage socketMessage = new SocketMessage(opCode, data);
			sendQueue.Enqueue(new ClientMessage(client, socketMessage));
		}


		public void HandleReceiveQueue() {
			while (true) {
				if (receiveQueue.Count > 0) {
					//取出一条消息
					ClientMessage clientMessage = receiveQueue.Dequeue();
					try {
						//System.Console.WriteLine(clientMessage.Message.Data);
						if (protocolBindEvent.ContainsKey(clientMessage.Message.OpCode)) {
							protocolBindEvent[clientMessage.Message.OpCode](clientMessage.Message.Data, clientMessage.Client);
						}
					}
					catch (System.Exception e) {

						System.Console.WriteLine(e.Message);
					}
				}
			}
		}
	}

}
