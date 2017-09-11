using UnityEngine;

#region Head
/*
 * [OverView]	基礎フェードクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
abstract public class BasicFade : MonoBehaviour {

	#region serialize_variable
	[SerializeField, Header("[true]IN [false]OUT")]
	bool _fadeMode = false;				// 実行状態
	[SerializeField, Header("処理時間")]
	float _fadeTime = 0.0f;				// フェード処理時間
	#endregion

	#region variable
	private float _playTime = 0.0f;		// 実行時間
	#endregion
}