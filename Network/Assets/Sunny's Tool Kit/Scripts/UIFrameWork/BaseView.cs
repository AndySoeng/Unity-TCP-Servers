using UnityEngine;

public class BaseView : MonoBehaviour {

	public UIType Type {
		get;
		private set;
	}


	/// <summary>
	/// 被打开时：生成后，入栈时调用
	/// </summary>
	public virtual void OnOpen(UIType _type) {
		Type = _type;

	}

	/// <summary>
	/// 被暂停：当有新的UI入栈时，被调用
	/// </summary>
	public virtual void OnPause() {
		CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
		if (cg == null) {
			cg.gameObject.AddComponent<CanvasGroup>();
		}
		cg.blocksRaycasts = false;
	}

	/// <summary>
	/// 重新被激活：当上层UI出栈时，被调用
	/// </summary>
	public virtual void OnResume() {
		CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
		if (cg == null) {
			cg.gameObject.AddComponent<CanvasGroup>();
		}
		cg.blocksRaycasts = true;
	}

	/// <summary>
	/// 当被销毁时：出栈时调用
	/// </summary>
	public virtual void OnExit() {
		Destroy(gameObject);
	}
}