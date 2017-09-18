using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

#region Head
/*
 * [OverView]	シーン管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ProjectSceneManager : Singleton<ProjectSceneManager> {

	#region private_class
	private class FadeInfo {

		public ProjectDefine.SceneID nextID = ProjectDefine.SceneID.NONE;	// 次シーン情報
		public BasicFade fadeIn = null;										// フェードイン情報
		public BasicFade fadeOut = null;									// フェードアウト情報
	}
	#endregion

	#region serialize_variable
	[SerializeField, Header("シーン切替情報")]
	FadeInfo _fadeInfo = null;												// シーン切替用
	#endregion

	#region variable
	private Stack<BasicScene> _sceneList = new Stack<BasicScene>();			// 実行シーン
	private List<BasicFade> _fadeList = new List<BasicFade>();				// 実行フェード
	#endregion

	#region function
	// 初期化
	protected override void Initialize()
	{
		// シーンが未登録なら
		if (_sceneList.Count == 0) {

			// シーンを取得して登録
			BasicScene basicScene = FindObjectOfType<BasicScene>();
			Apply(basicScene);
		}
	}

	// 更新
	private void Update()
	{
		// フェード情報が存在するなら
		if (_fadeInfo != null) {

			// フェード処理
			Fade();

			// フェードが終了していなければ実行終了
			if (_fadeInfo != null)
				return;
		}

		// シーン処理
		Run();
	}

	// 有効化
	public void Apply(BasicScene basicScene)
	{
		// NULLチェック
		if (basicScene == null)
			return;

		// 重複確認
		if (_sceneList.Contains(basicScene))
			return;

		// シーン準備
		basicScene.Startup();

		// 追加
		_sceneList.Push(basicScene);
	}

	// フェード
	private void Fade()
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

	// 実行
	private void Run()
	{
		// シーン情報があれば
		if (_sceneList.Count > 0) {

			// 複数更新なら
			if (_sceneList.Peek().RunID == ProjectDefine.RunID.MULUTIPLE) {

				// 退避
				BasicScene scene = _sceneList.Pop();

				// 前シーン実行
				Run();

				// 復元
				_sceneList.Push(scene);
			}

			// シーン更新
			_sceneList.Peek().Run();
		}
	}

	// シーン登録
	public void EntryScene(ProjectDefine.SceneID sceneID)
	{
		// 重複確認
		if (_sceneList.Any(scene => scene.SceneID == sceneID))
			return;

		// シーン作成
		LoadScene(sceneID, LoadSceneMode.Additive);
	}

	// シーン削除
	public void RemoveScene(ProjectDefine.SceneID sceneID)
	{
		// 一時変数作成
		Stack<BasicScene> backupList = new Stack<BasicScene>();
		
		// 選択シーン削除
		while(_sceneList.Count > 0) {

			// 退避
			BasicScene basicScene = _sceneList.Pop();

			// 選択シーン以外なら
			if (basicScene.SceneID == sceneID || backupList.Count == 0 ) {

				// 確保
				backupList.Push(basicScene);
			}
		}

		// シーン再構成
		backupList.Reverse();
		_sceneList.CopyTo(backupList.ToArray(), backupList.Count);
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

		// フェード情報取得
		for(int i = 0; i < _fadeList.Count; i ++) {

			// フェードイン情報の登録
			if (fadeInID == _fadeList[i].FadeID) {

				_fadeInfo.fadeIn = _fadeList[i];
			}

			// フェードアウト情報の登録
			if (fadeInID == _fadeList[i].FadeID)
				_fadeInfo.fadeOut = _fadeList[i];

			// フェード情報の取得が終わったら
			if (_fadeInfo.fadeIn != null && _fadeInfo.fadeOut != null) {

				// 次シーンを登録して終了
				_fadeInfo.nextID = sceneID;
				return;
			}
		}

		// フェード情報の取得に失敗
		_fadeInfo.fadeIn = null;
		_fadeInfo.fadeOut = null;
	}

	// シーン読込
	private void LoadScene(ProjectDefine.SceneID sceneID, LoadSceneMode sceneMode)
	{
		// シーン名の取得
		string sceneName = ConvertSceneName(sceneID);

		// シーン切替
		SceneManager.LoadScene(sceneName, sceneMode);
	}

	// シーン名取得
	private string ConvertSceneName(ProjectDefine.SceneID sceneID)
	{
		// シーン名初期化
		string sceneName = "";

		// シーン名取得
		switch(sceneID) {

			default:
			case ProjectDefine.SceneID.TITLE:
				// TODO
				break;
		}

		// シーン名を返す
		return sceneName;
	}
	#endregion
}