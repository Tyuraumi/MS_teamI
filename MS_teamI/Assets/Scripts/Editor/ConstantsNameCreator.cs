using System.Collections.Generic;
using System.Linq;

#region Head
/*
 * [OverView]	名前定数作成クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ConstantsNameCreator<T> : ConstantsCreator<T> {

	#region function
	// 型情報利用確認
	protected override bool CheckValueType()
	{
		// 情報未作成なら失敗
		if (_valueData == null)
			return false;

		// IDで確認
		switch (_valueData.ID) {

			case ValueID.STRING:
			case ValueID.INT:
			case ValueID.FLOAT:

				// 利用可能
				return true;

			default:
				break;
		}

		// 利用不可
		return false;
	}

	// クラス上部作成
	protected override void CreateClassOpening()
	{
		// 基底呼出
		base.CreateClassOpening();

		// region開始
		_stringBuilder.Append("\t").AppendLine("#region const_variable");
	}


	// 定数作成
	protected override void CreateConstants()
	{
		// 定数がなければ処理しない
		if (_dictionary.Count == 0)
			return;

			// 変数の最長を求める
			int keyLengthMax = 1 + _dictionary.Keys.Select(key => key.Length).Max();

		// 定数の書出し
		foreach (KeyValuePair<string, T> valuePair in _dictionary) {

			// 文字列が存在しなければ次へ
			if (string.IsNullOrEmpty(valuePair.Key))
				continue;

			// 空白調整
			string EqualStr = string.Format("{0, " + (keyLengthMax - valuePair.Key.Length).ToString() + "}", "=");

			// 型と定数名を入力
			_stringBuilder.Append("\t").AppendFormat(@"public const {0} {1} {2} ", _valueData.Name, valuePair.Key, EqualStr);

			// 型別設定
			switch (_valueData.ID) {

				case ValueID.STRING:
					_stringBuilder.AppendFormat(@"""{0}"";", valuePair.Value).AppendLine();
					break;

				case ValueID.FLOAT:
					_stringBuilder.AppendFormat(@"{0}f;", valuePair.Value).AppendLine();
					break;

				case ValueID.INT:
					_stringBuilder.AppendFormat(@"{0};", valuePair.Value).AppendLine();
					break;

				default:
					break;
			}
		}
	}

	// クラス下部作成
	protected override void CreateClassEnding()
	{
		// region終了
		_stringBuilder.Append("\t").AppendLine("#endregion");

		// 基底呼出
		base.CreateClassEnding();
	}
	#endregion
}