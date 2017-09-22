using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	基礎フェードクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class BasicFade : BasicBehaviour {

	#region serialize_variable
	[SerializeField, Header("フェード名")]
	FadeEnum.ID _fadeID = FadeEnum.ID.NONE;					// フェード情報
	[SerializeField, Header("標準処理時間")]
	float _defaultFadeTime = 0.0f;							// 標準処理時間
	#endregion

	#region variable
	private Queue<float> _fadeTime = new Queue<float>();	// フェード時間
	private FadeEnum.Mode _fadeMode = FadeEnum.Mode.OUT;	// 実行状態
	private float _playTime = 0.0f;							// 実行時間
	#endregion

	#region property
	public FadeEnum.ID FadeID {

		get {

			return _fadeID;
		}
	}

	public FadeEnum.Mode FadeMode {

		protected get {

			return _fadeMode;
		}

		set {

			_fadeMode = value;
		}
	}
	
	public float FadeTime {

		protected get {

			if (_fadeTime.Count == 0)
				return _defaultFadeTime;

			return _fadeTime.Peek();
		}

		set {

			if (value <= 0.0f)
				return;

			_fadeTime.Enqueue(value);
		}
	}

	public virtual bool IsEnd {

		get {
			
			return (_playTime >= FadeTime);
		}
	}

	protected virtual float PlayPercent {

		get {

			return (_playTime / FadeTime);
		}
	}
	#endregion

	#region function
	// 起動
	public override void Startup()
	{
		// 標準処理時間設定
		_defaultFadeTime = Mathf.Max(0.0f, _defaultFadeTime);
	}

	// 準備
	public override void Ready()
	{
		// 前回使用していれば
		if(_playTime > 0.0f) {

			// 時間情報破棄
			_fadeTime.Dequeue();
		}

		// 時間初期化
		_playTime = 0.0f;
	}

	// 実行
	public override void Run()
	{
		// 時間更新
		_playTime = Mathf.Min(FadeTime, _playTime + Time.deltaTime);

		// フェード実行
		Fade();
	}

	// フェード実行
	protected abstract void Fade();
	#endregion
}