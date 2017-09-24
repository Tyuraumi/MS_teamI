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

		#region serialize_variable
		[SerializeField]
		private int _no = 0;						// サウンド番号
		[SerializeField]
		private string _name = "";					// サウンド名
		[SerializeField]
		private float _volume = 0.0f;				// 音量
		#endregion

		#region property
		public int No {

			get {

				return _no;
			}

			set {

				_no = value;
			}
		}

		public string Name {

			get {

				return _name;
			}
			
			set {

				_name = value;
			}
		}

		public float Volume {

			get {

				return _volume;
			}

			set {

				_volume = Mathf.Clamp01(value);
			}
		}
		#endregion
	}
	#endregion

	#region variable
	public List<Param> list = new List<Param>();	// サウンド情報
	#endregion
}