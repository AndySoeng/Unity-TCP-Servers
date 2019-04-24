using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public static class EncodeTools {



	public static byte[] GetMessageData(ref List<byte> cache) {
		//四个字节 构成一个int长度 不能构成一个完整的消息
		if (cache.Count < 4) {
			return null;
		}

		//使用MemoryStream进行内存的读取
		using (MemoryStream ms = new MemoryStream(cache.ToArray())) {
			using (BinaryReader br = new BinaryReader(ms)) {
				//取到前4个字节内，存储的数据长度
				int dataLength = br.ReadInt32();

				//ms.Length是MemoryStream中cache的长度，  ms.Position是当前的读到的位置
				int remainLength = (int)(ms.Length - ms.Position);
				//数据长度不够包头约定的长度 不能构成一个完整的消息
				if (dataLength > remainLength) {
					return null;
				}
				//读取完整消息
				byte[] data = br.ReadBytes(dataLength);
				//更新一下数据缓存
				cache.Clear();
				//读取剩余没有被读取的长度
				cache.AddRange(br.ReadBytes(remainLength-dataLength));
				return data;


			}
		}
	}



	//读取指定路径文件内信息
	public static string ReadAllText(string textPath) {
		if (!File.Exists(textPath)) {
			File.CreateText(textPath);
		}
		string fileContent = File.ReadAllText(textPath, Encoding.UTF8);
		return fileContent;
	}


	//把内容写入指定文件
	public static bool WriteAllText(string textPath, string content) {
		if (!File.Exists(textPath)) {
			File.CreateText(textPath);
		}
		File.WriteAllText(textPath, content);
		return true;
	}




	/// <summary>
	/// 字节流压缩
	/// </summary>
	/// <param name="sendData"></param>
	/// <returns></returns>
	public static byte[] Compress(byte[] sendData) {
		Debug.Log("[发送压缩前]：" + sendData.Length);
		using (MemoryStream outStream = new MemoryStream()) {
			using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true)) {
				zipStream.Write(sendData, 0, sendData.Length);
				zipStream.Close();

				byte[] compressData = outStream.ToArray();
				Debug.Log("[发送压缩后]：" + compressData.Length);
				return compressData;
			}
		}

	}

	/// <summary>
	/// 字节流解压
	/// </summary>
	/// <param name="inputBytes"></param>
	/// <returns></returns>
	public static byte[] Decompress(byte[] inputBytes) {
		using (MemoryStream inputStream = new MemoryStream(inputBytes)) {
			using (MemoryStream outStream = new MemoryStream()) {
				using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress, true)) {
					zipStream.CopyTo(outStream);
					zipStream.Close();
					return outStream.ToArray();
				}
			}
		}
	}


	/// <summary>
	/// 把一个object类型转换成byte[]
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static byte[] EncodeObj(object value) {
		using (MemoryStream ms = new MemoryStream()) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, value);
			byte[] valueBytes = new byte[ms.Length];
			Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
			return valueBytes;
		}
	}



	/// <summary>
	/// 反序列化对象
	/// </summary>
	/// <param name="valueBytes"></param>
	/// <returns></returns>
	public static object DecodeObj(byte[] valueBytes) {
		using (MemoryStream ms = new MemoryStream(valueBytes)) {
			BinaryFormatter bf = new BinaryFormatter();
			object value = bf.Deserialize(ms);
			return value;
		}
	}

}
