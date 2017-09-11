using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	基礎情報管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
abstract public class BasicManager : BasicScript
{
	#region serialize_variable
	[SerializeField, Header("実行情報")]
	List<BasicScript> _scriptList = new List<BasicScript>();
	#endregion

	#region function
	// 準備
	override public void Standby()
	{
		_scriptList.ForEach(script => script.Standby());
	}

	// 実行
	override public void Run()
	{
		_scriptList.ForEach(script => script.Run());
	}

	// 登録
	virtual public void Entry(BasicScript basicScript)
	{
		_scriptList.Add(basicScript);
	}
	#endregion
}