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

	#region property
	public bool IsFade {

		get {

			return (_fadeData != null);
		}
	}
	#endregion

	#region function
	// 初期化
	protected override void Initialize()
	{
		// シーンデータ取得
		_sceneData = Resources.Load<SceneData>(FileName.SceneDataFile);

		// フェード情報取得・初期化
		_fadeList = GetComponentsInChildren<BasicFade>().ToList();
		foreach(BasicFade fadeInfo in _fadeList) {

			// 起動
			fadeInfo.Startup();

			// 準備
			fadeInfo.Ready();
		}
	}

	// 更新
	private void Update()
	{
		// フェード情報が存在するなら
		if (_fadeData != null) {

			// フェード処理
			Fade();
		}
		else {
		
			// シーン処理
			Run();
		}
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
		if (_fadeData.FadeOut != null) {

			// フェードアウトの実行
			_fadeData.FadeOut.Run();

			// 終了確認
			if (_fadeData.FadeOut.IsEnd) {

				// シーン切替
				LoadScene(_fadeData.NextID, LoadSceneMode.Single);

				// フェードアウト情報削除
				_fadeData.FadeOut = null;

				// フェードイン実行準備
				_fadeData.FadeIn.FadeMode = FadeEnum.Mode.IN;
				_fadeData.FadeIn.Ready();
			}
		}

		// フェードイン情報があれば
		if (_fadeData.FadeIn != null) {

			// フェードインの実行
			_fadeData.FadeIn.Run();

			// フェードインが終わったら
			if (_fadeData.FadeIn.IsEnd) {

				// フェード終了
				_fadeData = null;
			}
		}
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
		// フェード中は処理しない
		if (IsFade)
			return;

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

		// 削除確認用
		int sceneCount = _sceneList.Count;
		bool remove = false;
		
		// 選択シーン削除
		while(_sceneList.Count > 0) {

			// 退避
			BasicScene basicScene = _sceneList.Pop();

			// 選択シーンなら
			if (basicScene.SceneID == sceneID && sceneCount > 0) {

				// 削除通知
				remove = true;
				break;
			}

			// 確保
			backupList.Push(basicScene);
		}

		// シーン再構成
		while (backupList.Count > 0)
			_sceneList.Push(backupList.Pop());

		// 削除確認
		if (!remove)
			return;

		// シーン名の取得
		string sceneName = "";
		if (!ConvertScene(sceneID, out sceneName))
			return;

		// シーン削除
		SceneManager.UnloadSceneAsync(sceneName);
	}

	// シーン切替
	public void ChangeScene(SceneChangeData changeData)
	{
		// フェード中は処理しない
		if (IsFade)
			return;

		// NULLチェック
		if (changeData == null)
			return;

		// シーン情報初期化
		_sceneList.Clear();

		// フェード情報作成
		_fadeData = new FadeData();

		// フェード情報取得
		foreach(BasicFade fadeInfo in _fadeList) {

			// フェードアウト情報の登録
			if (changeData.FadeOutID == fadeInfo.FadeID)
				_fadeData.FadeOut = fadeInfo;

			// フェードイン情報の登録
			if (changeData.FadeInID == fadeInfo.FadeID)
				_fadeData.FadeIn = fadeInfo;

			// フェード情報の取得が終わったら
			if (_fadeData.FadeOut != null && _fadeData.FadeIn != null) {

				// フェード時間登録
				_fadeData.FadeOutTime = changeData.FadeOutTime;
				_fadeData.FadeInTime = changeData.FadeInTime;

				// フェードアウト実行準備
				_fadeData.FadeOut.FadeMode = FadeEnum.Mode.OUT;
				_fadeData.FadeOut.Ready();

				// 次シーンを登録して終了
				_fadeData.NextID = changeData.NextID;
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
		if (!ConvertScene(sceneID, out sceneName))
			return;

		// シーン切替
		SceneManager.LoadScene(sceneName, sceneMode);
	}

	// シーン情報変換
	private bool ConvertScene(SceneEnum.ID sceneID, out string sceneName)
	{
		// シーン情報探索
		sceneName = "";
		for (int i = 0; i < _sceneData.list.Count; i++) {

			// 番号確認
			if (_sceneData.list[i].No != (int)sceneID)
				continue;

			// シーン名取得
			sceneName = _sceneData.list[i].Name;
			break;
		}

		// 取得結果を返す
		return (sceneName != "");
	}
	#endregion
}