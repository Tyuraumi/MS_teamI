#region Head
/*
 * [OverView]	ファイルディレクトリ定数クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class DirectoryPath {

	#region const_variable
	private const string _projectSettingPath = "ProjectSettings";	// ProjectSettings
	private const string _resourcePath = "Assets/Resources/";		// Resources
	private const string _scriptPath = "Assets/Scripts/";			// Scripts
	private const string _bgmPath = "Sound/BGM/";					// BGM
	private const string _sePath = "Sound/SE/";						// SE
	#endregion

	#region static_property
	public static string ProjectSettingPath {

		get {

			return _projectSettingPath;
		}
	}

	public static string ResourcePath {

		get {

			return _resourcePath;
		}
	}

	public static string ScriptPath {

		get {

			return _scriptPath;
		}
	}

	public static string BGMPath {

		get {

			return _bgmPath;
		}
	}

	public static string SEPath {

		get {

			return _sePath;
		}
	}
	#endregion
}