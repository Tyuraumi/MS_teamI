using UnityEngine;

#region Head
/*
 * [OverView]	基礎シーンクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class BasicScene : BasicManager {

	#region function
	// 準備
	private void Awake()
	{
		// シーンマネージャ確認
		if(SceneManager.Instance != null) {

			// マネージャに登録
			SceneManager.Instance.Entry(this);
		}
	}
	#endregion
}