using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
		// イベントシステム作成
		CreateEventSystem();

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

				// イベントシステム作成
				CreateEventSystem();

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
		// 選択シーンが存在しなければ処理しない
		if (!_sceneList.Any(scene => scene.SceneID == sceneID))
			return;

		// シーン情報が１つしかなければ処理しない
		if (_sceneList.Count == 1)
			return;

		// 一時変数作成
		Stack<BasicScene> backupList = new Stack<BasicScene>();

		// 選択シーン削除
		while(_sceneList.Count > 0) {

			// 退避
			BasicScene basicScene = _sceneList.Pop();

			// 選択シーンなら
			if (basicScene.SceneID == sceneID) {

				// シーン名の取得
				string sceneName = "";
				if (!ConvertScene(sceneID, out sceneName))
					return;

				// シーン削除
				SceneManager.UnloadSceneAsync(sceneName);
				break;
			}

			// 確保
			backupList.Push(basicScene);
		}

		// シーン再構成
		while (backupList.Count > 0)
			_sceneList.Push(backupList.Pop());
	}

	// シーン切替
	public void ChangeScene(SceneChangeData changeData)
	{
		// フェード中、または情報がなければ処理しない
		if (IsFade || changeData == null)
			return;

		// シーン情報初期化
		_sceneList.Clear();
		
		// フェード情報取得
		_fadeData = new FadeData();
		foreach (BasicFade fadeInfo in _fadeList) {

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
		// シーン名初期化
		sceneName = "";

		// シーン情報探索
		SceneData.Param sceneData = _sceneData.list.FirstOrDefault(scene => scene.No == (int)sceneID);

		// 利用できないシーン情報なら失敗
		if (sceneData == null)
			return false;

		// シーン名取得
		sceneName = sceneData.Name;
		return true;
	}

	// イベントシステム作成
	private void CreateEventSystem()
	{
		// イベントシステムが存在するなら処理しない
		if (EventSystem.current != null)
			return;

		// イベントシステム作成
		GameObject eventObject = new GameObject(FileName.EventSystemFile);
		EventSystem.current = eventObject.AddComponent<EventSystem>();

		// 入力モジュール作成
		eventObject.AddComponent<StandaloneInputModule>();
	}
	#endregion
}