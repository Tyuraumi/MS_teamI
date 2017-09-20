using UnityEngine;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	基礎動作管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class BasicBehaviourManager : BasicBehaviour
{
	#region serialize_variable
	[SerializeField, Header("実行情報")]
	List<BasicBehaviour> _behaviourList = new List<BasicBehaviour>();	// 操作情報
	#endregion

	#region property
	protected List<BasicBehaviour> BahaviourList {

		get {

			return _behaviourList;
		}
	}
	#endregion

	#region function
	// 起動
	public override void Startup()
	{
		_behaviourList.ForEach(behaviour => behaviour.Startup());
	}

	// 準備
	public override void Ready()
	{
		_behaviourList.ForEach(behaviour => behaviour.Ready());
	}

	// 実行
	public override void Run()
	{
		_behaviourList.ForEach(behaviour => behaviour.Run());
	}

	// 登録
	public virtual void Entry(BasicBehaviour basicBehaviour)
	{
		_behaviourList.Add(basicBehaviour);
	}

	// 削除
	public virtual void Remove(BasicBehaviour basicBehaviour)
	{
		// 登録確認
		if (!_behaviourList.Contains(basicBehaviour))
			return;

		// 削除
		_behaviourList.Remove(basicBehaviour);
	}
	#endregion
}