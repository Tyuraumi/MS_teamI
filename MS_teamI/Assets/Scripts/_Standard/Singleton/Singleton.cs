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
		get {

			// インスタンス確認
			if (_instance == null) {

				// 情報探索
				_instance = (T)FindObjectOfType(typeof(T));

				// 探索に失敗したらメッセージ表示
				if (_instance == null)
					Debug.LogError(typeof(T) + " is Nothing.");
			}

			return _instance;
		}
	}
	#endregion

	#region function
	// 準備
	private void Awake()
	{
		// まだインスタンスが登録されていなければ
		if (_instance == null) {

			// インスタンス登録
			_instance = (T)this;
		}
		else {

			// インスタンスと異なっていれば
			if(_instance != this) {

				// 削除
				Destroy(gameObject);
				return;
			}
		}

		// シーン間引継設定
		DontDestroyOnLoad(gameObject);

		// 初期化
		Initialize();
	}

	// 初期化
	abstract protected void Initialize();
	#endregion
}