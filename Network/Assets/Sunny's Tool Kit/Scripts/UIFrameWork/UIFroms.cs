/// <summary>
/// 这个脚本中，存放了所有UIForm的路径信息，是一个数据类
/// 
/// 当我创建了一个Panel,就把这个panel的路径信息写进来
/// 
/// </summary>
public class UIFroms {
	//static 为了方便调用。 readonly为了防止数据被修改
	//public static readonly UIType panel_1 = new UIType("UIPrefabs/Panel1");
	//public static readonly UIType panel_2 = new UIType("UIPrefabs/Panel2");
	//提示界面路径
	public static readonly UIType NoticeView = new UIType("UIPrefabs/NoticeView");
	public static readonly UIType DrawOneCardView = new UIType("UIPrefabs/DrawOneCardView");
}