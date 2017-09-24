#region Head
/*
 * [OverView]	シーン切替情報クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class SceneChangeData {

	#region class
	private class Param {

		#region variable
		public FadeEnum.ID fadeID = FadeEnum.ID.NONE;	// 使用フェード情報
		public float fadeTime = 0.0f;					// 実行フェード時間
		#endregion
	}
	#endregion

	#region variable
	private SceneEnum.ID _nextID = SceneEnum.ID.NONE;	// 次シーン情報
	private Param _fadeOut = new Param();				// フェードアウト情報
	private Param _fadeIn = new Param();				// フェードイン情報
	#endregion

	#region property
	public SceneEnum.ID NextID {

		get {

			return _nextID;
		}

		set {

			if (value == SceneEnum.ID.NONE)
				return;

			_nextID = value;
		}
	}

	public FadeEnum.ID FadeInID {

		get {

			return _fadeIn.fadeID;
		}

		set {

			if (value == FadeEnum.ID.NONE)
				return;

			_fadeIn.fadeID = value;
		}
	}

	public FadeEnum.ID FadeOutID {

		get {

			return _fadeOut.fadeID;
		}

		set {

			if (value == FadeEnum.ID.NONE)
				return;

			_fadeOut.fadeID = value;
		}
	}

	public float FadeInTime {

		get {

			return _fadeIn.fadeTime;
		}

		set {

			if (value <= 0.0f)
				return;

			_fadeIn.fadeTime = value;
		}
	}

	public float FadeOutTime {

		get {

			return _fadeOut.fadeTime;
		}

		set {

			if (value <= 0.0f)
				return;

			_fadeOut.fadeTime = value;
		}
	}

	public FadeEnum.ID FadeID {
		
		set {
			
			FadeInID = value;
			FadeOutID = value;
		}
	}

	public float FadeTime {

		set {
			
			FadeInTime = value;
			FadeOutTime = value;
		}
	}
	#endregion
}