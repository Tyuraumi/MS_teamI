using System.Collections.Generic;

#region Head
/*
 * [OverView]	基礎スクリプト管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class BasicScriptManager : BasicScript {

	#region variable
	private List<BasicScript> _scriptList = new List<BasicScript>();	// 操作情報
	#endregion

	#region property
	protected List<BasicScript> ScriptList {

		get {

			return _scriptList;
		}
	}
	#endregion

	#region function
	// 起動
	public override void Startup()
	{
		_scriptList.ForEach(script => script.Startup());
	}

	// 準備
	public override void Ready()
	{
		_scriptList.ForEach(script => script.Ready());
	}

	// 実行
	public override void Run()
	{
		_scriptList.ForEach(script => script.Run());
	}

	// 登録
	public virtual void Entry(BasicScript basicScript)
	{
		_scriptList.Add(basicScript);
	}

	// 削除
	public virtual void Remove(BasicScript basicScript)
	{
		// 登録確認
		if (!_scriptList.Contains(basicScript))
			return;

		// 削除
		_scriptList.Remove(basicScript);
	}
	#endregion
}