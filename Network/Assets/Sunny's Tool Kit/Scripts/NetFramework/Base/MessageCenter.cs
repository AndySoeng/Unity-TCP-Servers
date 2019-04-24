using System.Collections.Generic;

public delegate void ProtocolBindEvent(object data);

public delegate void test(int a);

public class MessageCenter : SingletonMonoBehaviour<MessageCenter>{






	//存入消息和要执行的事件   协议号，委托事件
	public Dictionary<short, ProtocolBindEvent> protocolBindEvent = new Dictionary<short, ProtocolBindEvent>();

	/// <summary>
	/// 发送消息的队列
	/// 当有消息要发送时，存入该队列
	/// 在程序中对该队列进行轮询，有数据就发送
	/// </summary>
	public Queue<SocketMessage> sendQueue = new Queue<SocketMessage>();

	/// <summary>
	/// 存储接收到的消息的队列
	/// 当接收到消息后，存入该队列
	/// 在程序中，对该队列进行轮询，有数据就处理
	/// </summary>
	public Queue<SocketMessage> receiveQueue = new Queue<SocketMessage>();

	void Update() {
		//轮询：一直调用
		HandleReceiveQueue();
	}

	/// <summary>
	/// 添加一个监听事件
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public void AddObserver(short key,ProtocolBindEvent value) {
		//如果字典中已存在key，多添加一个事件
		if (protocolBindEvent.ContainsKey(key)) {
			protocolBindEvent[key] += value;
		}
		//如果字典中不存在key，添加一个事件
		else {
			protocolBindEvent[key] = value;
		}
	}


	public void Send(short opCode,object data) {
		SocketMessage socketMessage = new SocketMessage(opCode, data);
		sendQueue.Enqueue(socketMessage);
	}

	
	public void HandleReceiveQueue() {
		if (receiveQueue.Count > 0) {
			//取出一条消息
			SocketMessage socketMessage = receiveQueue.Dequeue();
			if (protocolBindEvent.ContainsKey(socketMessage.OpCode)) {
				protocolBindEvent[socketMessage.OpCode](socketMessage.Data);
			}
		}
	}
}
