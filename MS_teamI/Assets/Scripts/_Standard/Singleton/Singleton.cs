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
			// NULLチェック
			if (_instance == null) {

				// インスタンス取得
				_instance = (T)FindObjectOfType(typeof(T));

				// 取得失敗したら
				if (_instance == null) {

					// メッセージ出力
					print(typeof(T) + "is Nothing.");
				}
			}

			// 実体を返す
			return _instance;
		}
	}
	#endregion

	#region property
	private bool IsSingleton {

		get {

			return (_instance == null || _instance == this);
		}
	}
	#endregion

	#region function
	// 準備
	private void Awake()
	{
		// シングルトンか確認
		if(!IsSingleton) {

			// 削除
			Destroy(gameObject);
			return;
		}

		// 登録
		_instance = (T)this;
		DontDestroyOnLoad(gameObject);

		// 実行準備
		Standby();
	}

	// 最遅更新
	private void LateUpdate()
	{
		// 実行
		Run();
	}

	// 実行準備
	abstract protected void Standby();

	// 実行
	abstract protected void Run();
	#endregion
}