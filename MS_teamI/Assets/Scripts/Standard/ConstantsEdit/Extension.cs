#region Head
/*
 * [OverView]	拡張子定数クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class Extension {

	#region const_variable
	private const string _cSharp = ".cs";			// C#
	private const string _assetUnity = ".asset";	// UnityAsset
	private const string _csv = ".csv";				// CSV
	#endregion

	#region static_property
	public static string CSharp {

		get {

		
			return _cSharp;
		}
	}

	public static string AssetUnity {

		get {

			return _assetUnity;
		}
	}

	public static string CSV {

		get {

			return _csv;
		}
	}
	#endregion
}