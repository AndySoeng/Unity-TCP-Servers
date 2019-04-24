using System;
using System.Text;
using System.Web;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// <para>　</para>
/// 　常用工具类——加密解密类
/// <para>　-------------------------------------------------</para>
/// <para>　StringEncode：返回 HTML 字符串的编码结果</para>
/// <para>　StringDecode：返回 HTML 字符串的解码结果</para>
/// <para>　UrlEncode：返回 URL 字符串的编码结果</para>
/// <para>　UrlDecode：返回 URL 字符串的解码结果</para>
/// <para>　DESEncrypt：DES加密</para>
/// <para>　DESDecrypt：DES解密</para>
/// <para>　MD5：MD5函数</para>
/// <para>　SHA256：SHA256函数</para>
/// </summary>
public class EncyptHelper {
	/// <summary>
	/// 32位Key值：
	/// </summary>
	public static byte[] DESKey = new byte[] { 0x03, 0x0B, 0x13, 0x1B, 0x23, 0x2B, 0x33, 0x3B, 0x43, 0x4B, 0x9B, 0x93, 0x8B, 0x83, 0x7B, 0x73, 0x6B, 0x63, 0x5B, 0x53, 0xF3, 0xFB, 0xA3, 0xAB, 0xB3, 0xBB, 0xC3, 0xEB, 0xE3, 0xDB, 0xD3, 0xCB };

	#region 返回 HTML 字符串的编码结果
	/// <summary>
	/// 返回 HTML 字符串的编码结果
	/// </summary>
	/// <param name="str">字符串</param>
	/// <returns>编码结果</returns>
	public static string StringEncode(string str) {
		return HttpUtility.HtmlEncode(str);
	}
	#endregion

	#region 返回 HTML 字符串的解码结果
	/// <summary>
	/// 返回 HTML 字符串的解码结果
	/// </summary>
	/// <param name="str">字符串</param>
	/// <returns>解码结果</returns>
	public static string StringDecode(string str) {
		return HttpUtility.HtmlDecode(str);
	}
	#endregion

	#region 返回 URL 字符串的编码结果
	/// <summary>
	/// 返回 URL 字符串的编码结果
	/// </summary>
	/// <param name="str">字符串</param>
	/// <returns>编码结果</returns>
	public static string UrlEncode(string str) {
		return HttpUtility.UrlEncode(str);
	}
	#endregion

	#region 返回 URL 字符串的解码结果
	/// <summary>
	/// 返回 URL 字符串的解码结果
	/// </summary>
	/// <param name="str">字符串</param>
	/// <returns>解码结果</returns>
	public static string UrlDecode(string str) {
		return HttpUtility.UrlDecode(str);
	}
	#endregion

	#region DES加密
	/// <summary>
	/// DES加密
	/// </summary>
	/// <param name="strSource">待加密字串</param>
	/// <returns>加密后的字符串</returns>
	public static string DESEncrypt(string strSource) {
		return DESEncrypt(strSource, DESKey);
	}
	/// <summary>
	/// DES加密
	/// </summary>
	/// <param name="strSource">待加密字串</param>
	/// <param name="key">Key值</param>
	/// <returns>加密后的字符串</returns>
	public static string DESEncrypt(string strSource, byte[] key) {
		SymmetricAlgorithm sa = Rijndael.Create();
		sa.Key = key;
		sa.Mode = CipherMode.ECB;
		sa.Padding = PaddingMode.Zeros;
		MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
		byte[] byt = Encoding.Unicode.GetBytes(strSource);
		cs.Write(byt, 0, byt.Length);
		cs.FlushFinalBlock();
		cs.Close();
		return Convert.ToBase64String(ms.ToArray());
	}
	#endregion

	#region DES解密
	/// <summary>
	/// DES解密
	/// </summary>
	/// <param name="strSource">待解密的字串</param>
	/// <returns>解密后的字符串</returns>
	public static string DESDecrypt(string strSource) {
		return DESDecrypt(strSource, DESKey);
	}
	/// <summary>
	/// DES解密
	/// </summary>
	/// <param name="strSource">待解密的字串</param>
	/// <param name="key">32位Key值</param>
	/// <returns>解密后的字符串</returns>
	public static string DESDecrypt(string strSource, byte[] key) {
		SymmetricAlgorithm sa = Rijndael.Create();
		sa.Key = key;
		sa.Mode = CipherMode.ECB;
		sa.Padding = PaddingMode.Zeros;
		ICryptoTransform ct = sa.CreateDecryptor();
		byte[] byt = Convert.FromBase64String(strSource);
		MemoryStream ms = new MemoryStream(byt);
		CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
		StreamReader sr = new StreamReader(cs, Encoding.Unicode);
		return sr.ReadToEnd();
	}
	#endregion

	#region MD5函数
	/// <summary>
	/// MD5函数,需引用：using System.Security.Cryptography;
	/// </summary>
	/// <param name="str">原始字符串</param>
	/// <returns>MD5结果</returns>
	public static string MD5(string str) {
		byte[] b = Encoding.Default.GetBytes(str);
		b = new MD5CryptoServiceProvider().ComputeHash(b);
		string ret = "";
		for (int i = 0; i < b.Length; i++)
			ret += b[i].ToString("x").PadLeft(2, '0');
		return ret;
	}
	#endregion

	#region SHA256函数
	/// <summary>
	/// SHA256函数
	/// </summary>
	/// /// <param name="str">原始字符串</param>
	/// <returns>SHA256结果</returns>
	public static string SHA256(string str) {
		byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
		SHA256Managed Sha256 = new SHA256Managed();
		byte[] Result = Sha256.ComputeHash(SHA256Data);
		return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
	}
	#endregion
}