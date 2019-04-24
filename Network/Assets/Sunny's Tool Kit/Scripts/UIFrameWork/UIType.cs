public class UIType {
	/// <summary>
	/// 每个字段，都有两个权限：读和写
	/// </summary>
	public string Path { get; set; }

	public string Name { get; private set; }

	public UIType(string _Path) {
		Path = _Path;
		Name = Path.Substring(Path.LastIndexOf('/') + 1);
	}
}