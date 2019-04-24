using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

class GameNet : Singleton<GameNet> {
	public GameNet() {
		//将操作码以及事件添加到MessageCenter中的字典
		MessageCenter.Instance.AddObserver(OperateCode.DRAWONECARD_REP, DRAWONECARD_REP);
	}


	public void DRAWONECARD_REP(object data) {
		//事件实现
		Debug.Log("From Gate Server：[" + data.ToString() + "]");

	}








}