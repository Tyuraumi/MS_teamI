#region Head
/*
 * [OverView]	レイヤーマスク番号クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class LayerMaskEnum {

	#region enum
	public enum ID {

		DEFAULT = 1,
		TRANSPARENT_FX = 2,
		IGNORE_RAYCAST = 4,
		WATER = 16,
		UI = 32,
		NONE = -1
	}
	#endregion
}