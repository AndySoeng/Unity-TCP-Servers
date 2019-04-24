using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITools
{
	

	//获取指定目录下的所有sprite
	public static  Sprite[] GetSprites(string path) {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
		return sprites;
	}

	//获取指定目录下的名字为name的sprite
	public static Sprite GetSprite(string path) {
        Sprite sprite = Resources.Load<Sprite>(path);
        return sprite;
	}

	//在指定物体下创建子预制物并返回出来
	public static GameObject CreateGameObjectInParent(GameObject _parent,GameObject childPrefab) {
        GameObject gameObject = GameObject.Instantiate(childPrefab,_parent.transform);
		return gameObject;
	}


	/// <summary>
	/// UI添加EventTrigger事件触发器,并绑定回调事件
	/// </summary>
	/// <param name="insObject">UI</param>
	/// <param name="eventType">事件触发器类型</param>
	/// <param name="callback">回调事件</param>
	public static void AddEventTrigger(GameObject insObject, EventTriggerType eventType, UnityAction<BaseEventData> unityAction) {
		//获取实例化按钮下的EventTrigger组件,准备为其添加交互事件
		EventTrigger eventTrigger = insObject.GetComponent<EventTrigger>();
		if (eventTrigger == null) {
			eventTrigger = insObject.AddComponent<EventTrigger>();
		}
		//判断事件入口注册的方法数量,实例化delegates
		if (eventTrigger.triggers.Count == 0) {
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}

		//实例事件入口,定义所要绑定的事件类型 
		EventTrigger.Entry entry = new EventTrigger.Entry {
			//设置监听事件类型
			eventID = eventType
		};
		//定义回调函数
		UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(unityAction);
		//设置回调函数
		entry.callback.AddListener(callback);
		//将事件入口添加给EventTrigger组件
		eventTrigger.triggers.Add(entry);
	}


	/// <summary>
	/// 清除UI上EventTrigger组件的所有事件入口
	/// </summary>
	/// <param name="insObject"></param>
	public void RemoveAllEventTriggers(Transform insObject) {
		if (insObject.GetComponent<EventTrigger>()) {
			EventTrigger eventTrigger = insObject.GetComponent<EventTrigger>();
			eventTrigger.triggers.RemoveAll(p => true);
		}
	}
}
