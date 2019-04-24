using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace NetworkServers.Base {
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
					cache.AddRange(br.ReadBytes(remainLength - dataLength));
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


	}

}
