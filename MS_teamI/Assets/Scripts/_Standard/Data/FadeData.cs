using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	フェード情報クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class FadeData {

	#region variable
	public SceneEnum.ID nextID = SceneEnum.ID.NONE;	// 次シーン情報
	public BasicFade fadeIn = null;					// フェードイン情報
	public BasicFade fadeOut = null;				// フェードアウト情報
	#endregion
}