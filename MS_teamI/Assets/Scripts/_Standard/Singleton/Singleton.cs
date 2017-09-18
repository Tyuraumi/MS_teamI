using UnityEngine;

#region Head
/*
 * [OverView]	シングルトンクラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
abstract public class Singleton<T> : MonoBehaviour where T : Singleton<T> {

	#region static_variable
	private static T _instance = null;	// 実体情報
	#endregion

	#region static_property
	public static T Instance
	{
		get
		{
			return _instance;
		}
	}
	#endregion

	#region function
	// リセット
	private void Reset()
	{
		// 既にシングルトン化していたら処理しない
		if (_instance != null)
			return;

		// 実体登録
		_instance = (T)this;

		// 初期化
		Initialize();
	}

	// 準備
	private void Awake()
	{
		// シングルトンでなければ
		if(_instance != this) {

			// 削除
			Destroy(gameObject);
			return;
		}

		// シーン間で保持
		DontDestroyOnLoad(gameObject);
	}

	// 初期化
	abstract protected void Initialize();
	#endregion
}