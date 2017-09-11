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

	#region function
	// 準備
	virtual protected void Awake()
	{
		// シングルトンか確認
		CheckInstance();
	}

	// 後更新
	abstract protected void LateUpdate();

	// シングルトン情報確認
	protected bool CheckInstance()
	{
		// 異なる情報なら
		if (_instance != null && _instance != this) {

			// 自身を削除
			Destroy(gameObject);
			return false;
		}

		// 登録
		_instance = (T)this;
		return true;
	}
	#endregion
}