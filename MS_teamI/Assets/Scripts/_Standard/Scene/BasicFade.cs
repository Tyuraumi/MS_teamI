using UnityEngine;

#region Head
/*
 * [OverView]	基礎フェードクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
abstract public class BasicFade : BasicScript {

	#region serialize_variable
	[SerializeField, Header("切替種別 : [true]IN [false]OUT")]
	bool _fadeMode = false;				// 実行状態
	[SerializeField, Header("処理時間")]
	float _fadeTime = 0.0f;				// フェード処理時間
	#endregion

	#region variable
	private float _playTime = 0.0f;		// 実行時間
	#endregion

	#region property
	public bool IsEnd {

		get {
			
			return (_playTime >= _fadeTime);
		}
	}

	protected bool FadeMode {

		get {

			return _fadeMode;
		}
	}

	virtual protected float PlayPercent {

		get {

			return Mathf.Clamp01(_playTime / _fadeTime);
		}
	}
	#endregion

	#region function
	// 実行準備
	public override void Standby()
	{
		// 処理時間設定
		_fadeTime = Mathf.Max(0.0f, _fadeTime);

		// 実行時間初期化
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
	abstract protected void Fade();
	#endregion
}