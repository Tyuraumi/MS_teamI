using UnityEngine;

#region Head
/*
 * [OverView]	基礎フェードクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class BasicFade : BasicBehaviour {

	#region serialize_variable
	[SerializeField, Header("フェード名")]
	FadeEnum.ID _fadeID = FadeEnum.ID.NONE;	// フェード情報
	[SerializeField, Header("処理時間")]
	float _fadeTime = 0.0f;					// 処理時間
	#endregion

	#region variable
	private bool _fadeMode = false;			// 実行状態
	private float _playTime = 0.0f;			// 実行時間
	#endregion

	#region property
	public FadeEnum.ID FadeID {

		get {

			return _fadeID;
		}
	}

	public virtual bool IsEnd {

		get {
			
			return (_playTime >= _fadeTime);
		}
	}
	#endregion

	#region function
	// 起動
	public override void Startup()
	{
		// 処理時間設定
		_fadeTime = Mathf.Max(0.0f, _fadeTime);
	}

	// 準備
	public override void Ready()
	{
		// 時間初期化
		_playTime = 0.0f;
	}

	// 実行
	public override void Run()
	{
		// 時間更新
		_playTime = Mathf.Min(_fadeTime, _playTime + Time.deltaTime);

		// フェード実行
		Fade();
	}

	// フェード実行
	protected abstract void Fade();
	#endregion
}