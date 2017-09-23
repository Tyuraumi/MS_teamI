#region Head
/*
 * [OverView]	ファイル名定数クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class FileName {

	#region const_variable
	private const string _sceneDataFile = "SceneData";		// シーン情報名
	private const string _eventSystemFile = "EventSystem";	// イベントシステム名
	#endregion

	#region static_property
	public static string SceneDataFile {

		get {

			return _sceneDataFile;
		}
	}

	public static string EventSystemFile {

		get {

			return _eventSystemFile;
		}
	}
	#endregion
}