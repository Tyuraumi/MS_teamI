using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#region Head
/*
 * [OverView]	シーン管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ProjectSceneManager : Singleton<ProjectSceneManager> {

	#region private_class
	[System.Serializable]
	private class FadeObject {

		public ProjectDefine.FadeID fadeID = ProjectDefine.FadeID.NONE;		// フェード番号
		public BasicFade script = null;										// スクリプト情報
	}

	private class FadeInfo {

		public ProjectDefine.SceneID nextID = ProjectDefine.SceneID.NONE;	// 次シーン情報
		public BasicFade fadeIn = null;										// フェードイン情報
		public BasicFade fadeOut = null;									// フェードアウト情報
	}
	#endregion

	#region serialize_variable
	[SerializeField, Header("シーン切替情報")]
	FadeInfo _fadeInfo = null;												// シーン切替用
	[SerializeField, Header("フェード情報")]
	List<FadeObject> _fadeList = new List<FadeObject>();					// 使用フェード
	#endregion

	#region variable
	private Stack<BasicScene> _sceneList = new Stack<BasicScene>();			// 実行シーン
	#endregion

	#region function
	// 実行準備
	protected override void Standby()
	{
		// シーンが未登録なら
		if (_sceneList.Count == 0) {

			// シーンを取得して登録
			BasicScene basicScene = FindObjectOfType<BasicScene>();
			EntryScene(basicScene);
		}
	}

	// 実行
	protected override void Run()
	{
		// フェード情報が存在するなら
		if(_fadeInfo != null) {

			// フェード処理
			RunFade();

			// フェードが終了していなければ実行終了
			if (_fadeInfo != null)
				return;
		}

		// シーン処理
		RunScene();
	}

	// フェード実行
	private void RunFade()
	{
		// フェードアウト情報があれば
		if (_fadeInfo.fadeOut != null) {

			// フェードアウトの実行
			_fadeInfo.fadeOut.Run();

			// 終了確認
			if (_fadeInfo.fadeOut.IsEnd) {

				// シーン切替
				LoadScene(_fadeInfo.nextID, LoadSceneMode.Single);

				// フェードアウト情報削除
				_fadeInfo.fadeOut = null;
			}

			// 処理を抜ける
			return;
		}

		// フェードイン情報があれば
		if (_fadeInfo.fadeIn != null) {

			// フェードインの実行
			_fadeInfo.fadeIn.Run();

			// フェードインが終わっていなければ
			if (!_fadeInfo.fadeIn.IsEnd) {

				// 処理を抜ける
				return;
			}
		}

		// フェード終了
		_fadeInfo = null;
	}

	// シーン実行
	private void RunScene()
	{
		// シーン情報があれば
		if (_sceneList.Count > 0) {

			// シーン更新
			_sceneList.Peek().Run();
		}
	}

	// シーン登録
	public void EntryScene(BasicScene basicScene)
	{
		// NULLチェック
		if (basicScene == null)
			return;

		// 重複確認
		if (_sceneList.Contains(basicScene))
			return;

		// シーン準備
		basicScene.Standby();

		// 追加
		_sceneList.Push(basicScene);
	}

	// シーン読込
	private void LoadScene(ProjectDefine.SceneID sceneID, LoadSceneMode sceneMode)
	{
		// TODO : シーン名の取得
		string sceneName = "";
		///////////////////////

		// シーン切替
		SceneManager.LoadScene(sceneName, sceneMode);
	}

	// シーン切替
	public void ChangeScene(ProjectDefine.SceneID sceneID, ProjectDefine.FadeID fadeID)
	{
		// フェード処理を統一して実行
		ChangeScene(sceneID, fadeID, fadeID);
	}

	// シーン切替
	public void ChangeScene(ProjectDefine.SceneID sceneID, ProjectDefine.FadeID fadeInID, ProjectDefine.FadeID FadeOutID)
	{
		// シーン情報初期化
		_sceneList.Clear();

		// 切替情報作成
		_fadeInfo = new FadeInfo();
		_fadeInfo.nextID = sceneID;

		// フェード情報取得
		for(int i = 0; i < _fadeList.Count; i ++) {

			// フェードイン情報の登録
			if (fadeInID == _fadeList[i].fadeID)
				_fadeInfo.fadeIn = _fadeList[i].script;

			// フェードアウト情報の登録
			if (fadeInID == _fadeList[i].fadeID)
				_fadeInfo.fadeOut = _fadeList[i].script;

			// フェード情報の取得が終わったら処理を抜ける
			if (_fadeInfo.fadeIn != null && _fadeInfo.fadeOut != null)
				return;
		}

		// フェード情報の取得に失敗
		_fadeInfo.fadeIn = null;
		_fadeInfo.fadeOut = null;
	}

	// シーン追加
	public void AddScene(ProjectDefine.SceneID sceneID)
	{
		// TODO : ここで例外処理
		///////////////////////

		// シーン読込
		LoadScene(sceneID, LoadSceneMode.Additive);
	}

	// シーン削除
	public void RemoveScene()
	{
		// シーン情報削除
		_sceneList.Pop();

		// シーン破棄
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
	}
	#endregion
}