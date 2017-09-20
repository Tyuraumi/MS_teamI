#region Head
/*
 * [OverView]	拡張子定数クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class Extension {

	#region variable
	private static string _cSharp = ".cs";			// C#
	private static string _assetUnity = ".asset";	// UnityAsset
	private static string _csv = ".csv";			// CSV
	#endregion

	#region property
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