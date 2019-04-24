using System;

public class SocketMessage {
	/// <summary>
	/// 操作码
	/// </summary>
	public short OpCode { get; set; }

	/// <summary>
	/// 数据
	/// </summary>
	public object Data { get; set; }

	public SocketMessage() {
	}

	public SocketMessage(short opCode, object data) {
		OpCode = opCode;
		Data = data ?? throw new ArgumentNullException(nameof(data));
	}
	
}
