using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DrawOneCardView :BaseView
{
	Button DrawOneCardButton;
	public override void OnOpen(UIType _type) {
		base.OnOpen(_type);
		DrawOneCardButton = GetComponentInChildren<Button>();
		UITools.AddEventTrigger(DrawOneCardButton.gameObject, UnityEngine.EventSystems.EventTriggerType.PointerUp, DrawOneCard);
	}

	private void DrawOneCard(BaseEventData arg0) {
		MessageCenter.Instance.Send(OperateCode.DRAWONECARD_REQ, "我要抽卡");
		Debug.Log("向GateServers请求抽卡");
	}

	private void Update() {
		//MessageCenter.Instance.Send(OperateCode.DRAWONECARD_REQ, "我要抽卡");
		if (Input.GetKeyDown(KeyCode.A)) {
			MessageCenter.Instance.Send(OperateCode.DRAWONECARD_REQ, "我要抽卡");
		}
	}
}
