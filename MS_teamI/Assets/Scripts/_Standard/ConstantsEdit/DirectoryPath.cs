#region Head
/*
 * [OverView]	ファイルディレクトリ定数クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class DirectoryPath {

	#region static_variable
	private static string _resourcePath = "Assets/Resources/";	// Resources
	#endregion

	#region static_property
	public static string ResourcePath {

		get {

			return _resourcePath;
		}
	}
	#endregion
}