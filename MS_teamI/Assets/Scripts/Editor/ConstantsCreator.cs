using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

#region Head
/*
 * [OverView]	定数作成クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public abstract class ConstantsCreator<T> {

	#region enum
	protected enum ValueID {

		NONE = 0,	// 不正値
		STRING,		// 文字
		INT,		// 整数
		FLOAT		// 実数
	}
	#endregion

	#region class
	protected class ValueData {

		#region variable
		public ValueID ID = ValueID.NONE;	// 番号
		public string Name = "";			// 名前
		#endregion
	}
	#endregion

	#region readonly_variable
	protected readonly string _saveDirectory =  DirectoryPath.ScriptPath + "Standard/Constants/";	// 保存パス
	private readonly char _delimiter = '_';			// 区切り文字
	private readonly string[] _invaludChars =		// 無効文字
	{
		" ", "!", "\"", "#", "$",
		"%", "&", "\'", "(", ")",
		"-", "=", "^",  "~", "\\",
		"|", "[", "{",  "@", "`",
		"]", "}", ":",  "*", ";",
		"+", "/", "?",  ".", ">",
		",", "<"
	};
	#endregion

	#region variable
	protected ValueData _valueData = null;				// 利用型情報
	protected StringBuilder _stringBuilder = null;		// ファイル情報
	private string _className = "";						// 作成クラス名
	private string _classOverView = "";					// 作成クラス概要
	protected Dictionary<string, T> _dictionary = null;	// 利用定数情報
	#endregion

	#region function
	// 作成
	public void Create(string className, string classOverView, Dictionary<string, T> valueDictionary)
	{
		// 型情報作成
		CreateValueData();

		// 対応外・利用外の型なら処理しない
		if (_valueData.ID == ValueID.NONE || !CheckValueType())
			return;

		// ファイル情報初期化
		_stringBuilder = new StringBuilder();

		// 変数作成
		ConvertClassString(className, classOverView, out _className, out _classOverView);

		// 定数最適化
		CompactString(valueDictionary, out _dictionary);

		// クラス作成
		CreateClass();

		// クラス出力
		ExportClass();
	}

	// 型情報作成
	private void CreateValueData()
	{
		// 初期化
		_valueData = new ValueData();

		// 文字列なら
		if (typeof(T) == typeof(string)) {

			// 情報登録
			_valueData.ID = ValueID.STRING;
			_valueData.Name = "string";
			return;
		}

		// 整数なら
		if (typeof(T) == typeof(int)) {

			// 情報登録
			_valueData.ID = ValueID.INT;
			_valueData.Name = "int";
			return;
		}

		// 実数なら
		if (typeof(T) == typeof(float)) {

			// 登録
			_valueData.ID = ValueID.FLOAT;
			_valueData.Name = "float";
			return;
		}
	}

	// クラス用文字列作成
	protected abstract void ConvertClassString(string nameIn, string overviewIn, out string nameOut, out string overviewOut);

	// 型情報利用確認
	protected abstract bool CheckValueType();

	// 定数名最適化
	private void CompactString(Dictionary<string, T> oldDictionary, out Dictionary<string, T> newDictionary)
	{
		// 初期化
		newDictionary = new Dictionary<string, T>();

		// 最適化
		foreach (KeyValuePair<string, T> valuePair in oldDictionary) {

			// 無効文字列の削除
			string valueKey = valuePair.Key;
			foreach (string val in _invaludChars)
				valueKey = valueKey.Replace(val, string.Empty);

			// 区切り文字設定
			string conversionKey = "";
			for (int strNo = 0; strNo < valueKey.Length; conversionKey += valueKey.ToUpper()[strNo++]) {

				//最初には設定しない
				if (strNo == 0)
					continue;

				//小文字か数字なら設定しない
				if (char.IsLower(valueKey[strNo]) || char.IsNumber(valueKey[strNo]))
					continue;

				// 連続大文字の時は設定しない
				if (char.IsUpper(valueKey[strNo - 1]) && !char.IsNumber(valueKey[strNo]))
					continue;

				// 区切り文字の時は設定しない
				if (valueKey[strNo] == _delimiter || valueKey[strNo - 1] == _delimiter)
					continue;

				// 区切り文字設定
				conversionKey += _delimiter;
			}

			// キー再登録
			newDictionary[conversionKey] = valuePair.Value;
		}
	}

	// クラス作成
	private void CreateClass()
	{
		// コメント文入力
		CreateComment();

		// クラス上部作成
		CreateClassOpening();

		// 定数作成
		CreateConstants();

		// クラス下部作成
		CreateClassEnding();
	}

	// クラス出力
	private void ExportClass()
	{
		// 保存パス作成
		string exportPath = _saveDirectory + _className + Extension.CSharp;

		// ディレクトリ確認
		string directoryName = Path.GetDirectoryName(exportPath);
		if (!Directory.Exists(directoryName)) {

			// ディレクトリ作成
			Directory.CreateDirectory(directoryName);
		}

		// ファイル書き出し
		File.WriteAllText(exportPath, _stringBuilder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

		// デバッグメッセージ表示
		//Debug.Log(_className + Extension.CSharp + "の作成が完了しました");
	}
	
	// クラス情報作成
	private void CreateComment()
	{
		// コメント入力
		_stringBuilder.AppendLine("#region Head");
		_stringBuilder.AppendLine("/*");
		_stringBuilder.Append(" * [OverView]").Append("\t").AppendLine(_classOverView);
		_stringBuilder.Append(" * [ Author ]").Append("\t").AppendLine("SUZUKI YUZI");
		_stringBuilder.AppendLine("*/");
		_stringBuilder.AppendLine("#endregion");
	}

	// クラス上部作成
	protected virtual void CreateClassOpening()
	{
		// クラス名入力
		_stringBuilder.Append("public static class ").Append(_className).AppendLine(" {");
		_stringBuilder.AppendLine();
	}

	// 定数作成
	protected abstract void CreateConstants();

	// クラス下部作成
	protected virtual void CreateClassEnding()
	{
		// クラスを閉じる
		_stringBuilder.Append("}");
	}
	#endregion
}