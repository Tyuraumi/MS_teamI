using UnityEngine;

#region Head
/*
 * [OverView]	基礎動作クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class BasicBehaviour : MonoBehaviour {

	#region function
	// 起動
	public virtual void Startup()
	{
		// none
	}

	// 準備
	public virtual void Ready()
	{
		// none
	}

	// 実行
	public virtual void Run()
	{
		// none
	}
	#endregion
}