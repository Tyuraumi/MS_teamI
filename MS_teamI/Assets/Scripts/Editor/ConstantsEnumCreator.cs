using System.Collections.Generic;

#region Head
/*
 * [OverView]	列挙型定数作成クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ConstantsEnumCreator<T> : ConstantsCreator<T> {

	#region function
	// 型情報利用確認
	protected override bool CheckValueType()
	{
		// 情報未作成なら失敗
		if (_valueData == null)
			return false;

		// IDで確認
		switch (_valueData.ID) {
			
			case ValueID.INT:

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
		_stringBuilder.Append("\t").AppendLine("#region enum");
	}

	// 定数作成
	protected override void CreateConstants()
	{
		// 列挙型名を入力
		_stringBuilder.Append("\t").Append("public enum ID ").AppendLine("{");
		_stringBuilder.AppendLine();

		// ループカウンタ初期化
		int count = 0;

		// 列挙型の書出し
		foreach (KeyValuePair<string, T> valuePair in _dictionary) {

			// データ利用カウント
			count++;

			// 文字列が存在しなければ次へ
			if (string.IsNullOrEmpty(valuePair.Key))
				continue;

			// データ列挙
			_stringBuilder.Append("\t\t").AppendFormat(@"{0} = {1}", valuePair.Key, valuePair.Value);
			
			// カンマ入力
			_stringBuilder.AppendLine(",");
		}
		
		// 空データ付与
		_stringBuilder.Append("\t\t").AppendLine("NONE = -1");

		// 列挙型を閉じる
		_stringBuilder.Append("\t").AppendLine("}");
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