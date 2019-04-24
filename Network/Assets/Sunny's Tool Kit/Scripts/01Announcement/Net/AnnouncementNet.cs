using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

class AnnouncementNet : Singleton<AnnouncementNet> {
	public AnnouncementNet() {
		//将操作码以及事件添加到MessageCenter中的字典
		//MessageCenter.Instance.AddObserver(OperateCode.ANNOUNCEMENT_REP, AnnouncementNet_Rep);
	}


	public void AnnouncementNet_Rep(object data) {
		//事件实现
		Debug.Log("From Server：[" + data.ToString() + "]");

	}








}