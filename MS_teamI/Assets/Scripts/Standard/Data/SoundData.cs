using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	サウンド情報クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class SoundData : ScriptableObject {

	#region class
	[System.Serializable]
	public class Param {

		#region variable
		public int No = 0;							// シーン番号
		public string Name = "";					// シーン名
		#endregion
	}
	#endregion

	#region variable
	public List<Param> list = new List<Param>();	// シーン情報
	#endregion
}