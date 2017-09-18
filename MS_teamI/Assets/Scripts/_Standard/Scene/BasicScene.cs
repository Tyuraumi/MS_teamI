using UnityEngine;

#region Head
/*
 * [OverView]	基礎シーンクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
abstract public class BasicScene : BasicBehaviour {

	#region serialize_variable
	[SerializeField, Header("シーン名")]
	ProjectDefine.SceneID _sceneID = ProjectDefine.SceneID.NONE;	// シーン情報
	[SerializeField, Header("実行種別")]
	ProjectDefine.RunID _runID = ProjectDefine.RunID.SINGLE;		// 実行種別
	#endregion

	#region property
	public ProjectDefine.SceneID SceneID {

		get {

			return _sceneID;
		}
	}

	public ProjectDefine.RunID RunID {

		get {

			return _runID;
		}
	}
	#endregion
}