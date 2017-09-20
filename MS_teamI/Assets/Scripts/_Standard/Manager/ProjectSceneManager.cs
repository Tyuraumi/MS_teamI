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
	
	#region variable
	private SceneData _sceneData = null;							// シーン情報
	private FadeData _fadeData = null;								// フェード情報
	private Stack<BasicScene> _sceneList = new Stack<BasicScene>();	// 実行シーン
	private List<BasicFade> _fadeList = new List<BasicFade>();		// 実行フェード
	#endregion

	#region function
	// 初期化
	protected override void Initialize()
	{
		// シーンデータ取得
		_sceneData = Resources.Load<SceneData>("SceneData");
	}

	// 更新
	private void Update()
	{
		// フェード情報が存在するなら
		if (_fadeData != null) {

			// フェード処理
			Fade();

			// フェードが終了していなければ実行終了
			if (_fadeData != null)
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
		if (_fadeData.fadeOut != null) {

			// フェードアウトの実行
			_fadeData.fadeOut.Run();

			// 終了確認
			if (_fadeData.fadeOut.IsEnd) {

				// シーン切替
				LoadScene(_fadeData.nextID, LoadSceneMode.Single);

				// フェードアウト情報削除
				_fadeData.fadeOut = null;
			}

			// 処理を抜ける
			return;
		}

		// フェードイン情報があれば
		if (_fadeData.fadeIn != null) {

			// フェードインの実行
			_fadeData.fadeIn.Run();

			// フェードインが終わっていなければ
			if (!_fadeData.fadeIn.IsEnd) {

				// 処理を抜ける
				return;
			}
		}

		// フェード終了
		_fadeData = null;
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
	public void EntryScene(SceneEnum.ID sceneID)
	{
		// 重複確認
		if (_sceneList.Any(scene => scene.SceneID == sceneID))
			return;

		

		// シーン作成
		LoadScene(sceneID, LoadSceneMode.Additive);
	}

	// シーン削除
	public void RemoveScene(SceneEnum.ID sceneID)
	{
		// 一時変数作成
		Stack<BasicScene> backupList = new Stack<BasicScene>();
		
		// 選択シーン削除
		while(_sceneList.Count > 0) {

			// 退避
			BasicScene basicScene = _sceneList.Pop();

			// 選択シーンなら処理しない
			if (basicScene.SceneID == sceneID && backupList.Count > 0)
				continue;

			// 確保
			backupList.Push(basicScene);
		}

		// シーン再構成
		backupList.Reverse();
		_sceneList.CopyTo(backupList.ToArray(), backupList.Count);
	}

	// シーン切替
	public void ChangeScene(SceneEnum.ID sceneID, FadeEnum.ID fadeID)
	{
		// フェード処理を統一して実行
		ChangeScene(sceneID, fadeID, fadeID);
	}

	// シーン切替
	public void ChangeScene(SceneEnum.ID sceneID, FadeEnum.ID fadeInID, FadeEnum.ID fadeOutID)
	{
		// シーン情報初期化
		_sceneList.Clear();

		// 切替情報作成
		_fadeData = new FadeData();

		// フェード情報取得
		for(int i = 0; i < _fadeList.Count; i ++) {

			// フェードイン情報の登録
			if (fadeInID == _fadeList[i].FadeID)
				_fadeData.fadeIn = _fadeList[i];

			// フェードアウト情報の登録
			if (fadeOutID == _fadeList[i].FadeID)
				_fadeData.fadeOut = _fadeList[i];

			// フェード情報の取得が終わったら
			if (_fadeData.fadeIn != null && _fadeData.fadeOut != null) {

				// 次シーンを登録して終了
				_fadeData.nextID = sceneID;
				return;
			}
		}

		// フェード情報の取得に失敗
		_fadeData = null;
	}

	// シーン読込
	private void LoadScene(SceneEnum.ID sceneID, LoadSceneMode sceneMode)
	{
		// シーン名の取得
		string sceneName = "";
		for (int i = 0; i < _sceneData.list.Count; i++) {

			// 番号確認
			if (_sceneData.list[i].No != (int)sceneID)
				continue;

			// シーン名取得
			sceneName = _sceneData.list[i].Name;
			break;
		}

		// シーン確認
		if (sceneName == "")
			return;

		// シーン切替
		SceneManager.LoadScene(sceneName, sceneMode);
	}
	#endregion
}