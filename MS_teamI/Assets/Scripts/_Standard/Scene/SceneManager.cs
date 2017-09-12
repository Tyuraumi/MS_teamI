using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	シーン管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class SceneManager : Singleton<SceneManager> {

	#region private_class
	[System.Serializable]
	private class FadeObject {

		public ProjectDefine.Fade fadeID = ProjectDefine.Fade.NONE;	// フェード番号
		public GameObject prefab = null;							// オブジェクト
	}
	#endregion

	#region serialize_variable
	[SerializeField, Header("フェード情報")]
	List<FadeObject> _fadeList = new List<FadeObject>();			// 使用フェード
	#endregion

	#region variable
	private Stack<BasicScene> _sceneList = new Stack<BasicScene>();	// 実行シーン
	#endregion

	#region function
	// 実行準備
	protected override void Standby()
	{
		// シーンが未登録なら
		if(_sceneList.Count == 0) {

			// シーンを取得して登録
			BasicScene basicScene = FindObjectOfType<BasicScene>();
			Entry(basicScene);
		}
	}

	// 実行
	protected override void Run()
	{
		// シーン情報があれば
		if(_sceneList.Count > 0) {

			// シーン更新
			_sceneList.Peek().Run();
		}
	}

	// 登録
	public void Entry(BasicScene basicScene)
	{
		// NULLチェック
		if (basicScene == null)
			return;

		// 重複確認
		if (_sceneList.Contains(basicScene))
			return;

		// 追加
		_sceneList.Push(basicScene);

		// シーン準備
		_sceneList.Peek().Standby();
	}

	// 削除
	public void Remove(BasicScene basicScene)
	{
		// NULLチェック
		if (basicScene == null)
			return;

		// 削除
		_sceneList.Pop();
	}
	#endregion
}