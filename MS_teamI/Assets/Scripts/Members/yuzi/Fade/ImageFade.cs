using UnityEngine;
using UnityEngine.UI;

#region Head
/*
 * [OverView]	画像フェードクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ImageFade : BasicFade {

	#region serialize_variable
	[SerializeField, Header("画像情報")]
	Image _image = null;	// 表示画像
	#endregion

	#region property
	protected override float PlayPercent {

		get {

			// フェードインなら
			if (FadeMode == FadeEnum.Mode.IN)
				return (1.0f - base.PlayPercent);

			return base.PlayPercent;
		}
	}
	#endregion

	#region function
	// 起動
	public override void Startup()
	{
		// 基底呼出
		base.Startup();

		// 画像情報がなければ
		if(_image == null) {

			// 新規取得
			_image = GetComponent<Image>();
		}
	}

	// フェード実行
	protected override void Fade()
	{
		// 透明度を求める
		Color color = Color.Lerp(Color.clear, Color.white, PlayPercent);

		// 画像更新
		_image.color = color;
	}
	#endregion
}