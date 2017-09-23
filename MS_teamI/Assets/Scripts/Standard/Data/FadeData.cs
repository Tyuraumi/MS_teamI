#region Head
/*
 * [OverView]	フェード情報クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class FadeData {

	#region variable
	private SceneEnum.ID _nextID = SceneEnum.ID.NONE;	// 次シーン情報
	private BasicFade _fadeOut = null;					// フェードアウト情報
	private BasicFade _fadeIn = null;					// フェードイン情報
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

	public BasicFade FadeIn {

		get {

			return _fadeIn;
		}

		set {

			_fadeIn = value;
		}
	}

	public BasicFade FadeOut {

		get {

			return _fadeOut;
		}

		set {

			_fadeOut = value;
		}
	}

	public float FadeInTime {

		set {

			if (_fadeIn == null)
				return;

			_fadeIn.FadeTime = value;
		}
	}

	public float FadeOutTime {

		set {

			if (_fadeOut == null)
				return;

			_fadeOut.FadeTime = value;
		}
	}
	#endregion
}