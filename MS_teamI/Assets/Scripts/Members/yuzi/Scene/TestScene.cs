using UnityEngine;

#region Head
/*
 * [OverView]	テスト用シーンクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class TestScene : BasicScene {

	#region serialize_variable
	[SerializeField, Header("利用シーン情報")]
	SceneEnum.ID _workID = SceneEnum.ID.NONE;	// シーン情報
	[SerializeField, Header("フェード時間")]
	float _fadeTime = 0.0f;						// フェード時間
	#endregion

	#region function
	// 実行
	public override void Run()
	{
		// シーン名表示
		print(SceneID);
	}

	// 切替
	public void Change()
	{
		// 切替情報作成
		SceneChangeData changeData = new SceneChangeData();
		changeData.NextID = _workID;
		changeData.FadeID = FadeEnum.ID.IMAGE;
		changeData.FadeTime = _fadeTime;

		// 切替申請発行
		ProjectSceneManager.Instance.ChangeScene(changeData);
	}

	// 追加
	public void Entry()
	{
		// 追加申請発行
		ProjectSceneManager.Instance.EntryScene(_workID);
	}

	// 削除
	public void Remove()
	{
		// 削除申請発行
		ProjectSceneManager.Instance.RemoveScene(_workID);
	}
	#endregion
}