using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

#region Head
/*
 * [OverView]	定数作成クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public static class ConstantsCreator {

	#region static_readonly_variable
	private static readonly string[] INVALUD_CHARS =
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

	#region enum
	private enum ValueID {

		NONE = 0,	// 不正値
		STRING,		// 文字
		INT,		// 整数
		FLOAT		// 実数
	}
	#endregion

	#region const_variable
	private const char DELIMITER = '_';													// 区切り文字
	private const string SAVE_DIRECTORY = "Assets/Scripts/_Standard/Editor/Constants/";	// 保存パス
	private const string SAVE_SCRIPT = ".cs";											// ファイル識別子
	#endregion

	#region static_function
	// 作成
	public static void Create<T>(string className, string classInfo, Dictionary<string, T> valueDictionary)
	{
		// 型情報作成
		ValueID valueID;
		string valueName;
		if (!CreateValueInfo<T>(out valueID, out valueName))
			return;

		// 定数文字列の最適化
		Dictionary<string, T> newValueDictionary;
		CompactString(valueDictionary, out newValueDictionary);

		// 定数の最大値を取得（空白設定）
		int keyLengthMax = 1 + newValueDictionary.Keys.Select(key => key.Length).Max();

		//コメント文とクラス名を入力
		StringBuilder builder = new StringBuilder();
		builder.AppendLine("#region Head");
		builder.AppendLine("/*");
		builder.AppendFormat(" *\t[OverView]\t{0}", classInfo).AppendLine();
		builder.AppendLine(" * [ Author ]	SUZUKI YUZI");
		builder.AppendLine("*/");
		builder.AppendLine("#endregion");
		builder.AppendFormat("public static class {0} {", className).AppendLine();

		// 定数の書出し
		foreach (KeyValuePair<string, T> valuePair in newValueDictionary) {

			// 文字列が存在しなければ次へ
			if (string.IsNullOrEmpty(valuePair.Key))
				continue;

			// 空白調整
			string EqualStr = String.Format("{0, " + (keyLengthMax - valuePair.Key.Length).ToString() + "}", "=");

			// 型と定数名を入力
			builder.Append("\t").AppendFormat(@"\tpublic const {0} {1} {2} ", valueName, valuePair.Key, EqualStr);

			// 型別設定
			switch (valueID) {

				case ValueID.STRING:
					builder.AppendFormat(@"""{0}"";", valuePair.Value).AppendLine();
					break;

				case ValueID.FLOAT:
					builder.AppendFormat(@"{0}f;", valuePair.Value).AppendLine();
					break;

				case ValueID.INT:
					builder.AppendFormat(@"{0};", valuePair.Value).AppendLine();
					break;

				default:
					break;
			}
		}

		// クラスを閉じる
		builder.AppendLine("}");

		// 保存パス作成
		string exportPath = SAVE_DIRECTORY + className + SAVE_SCRIPT;

		// ディレクトリ確認
		string directoryName = Path.GetDirectoryName(exportPath);
		if (!Directory.Exists(directoryName)) {

			// ディレクトリ作成
			Directory.CreateDirectory(directoryName);
		}

		// ファイル書き出し
		File.WriteAllText(exportPath, builder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

		Debug.Log(className + SAVE_SCRIPT + "の作成が完了しました");
	}

	// 型から情報作成
	private static bool CreateValueInfo<T>(out ValueID valueID, out string valueName)
	{
		// 初期化
		valueID = ValueID.NONE;
		valueName = "";

		// 文字列なら
		if (typeof(T) == typeof(string)) {

			// 正常終了
			valueID = ValueID.STRING;
			valueName = "string";
			return true;
		}

		// 整数なら
		if (typeof(T) == typeof(int)) {

			// 正常終了
			valueID = ValueID.INT;
			valueName = "int";
			return true;
		}

		// 実数なら
		if (typeof(T) == typeof(float)) {

			// 正常終了
			valueID = ValueID.FLOAT;
			valueName = "float";
			return true;
		}

		// 不正値
		return false;
	}

	// 定数名最適化
	private static void CompactString<T>(Dictionary<string, T> oldDictionary, out Dictionary<string, T> newDictionary)
	{
		// 初期化
		newDictionary = new Dictionary<string, T>();

		// 最適化
		foreach (KeyValuePair<string, T> valuePair in oldDictionary) {

			// 無効文字列の削除
			string valueKey = valuePair.Key;
			foreach (string val in INVALUD_CHARS)
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
				if (valueKey[strNo] == DELIMITER || valueKey[strNo - 1] == DELIMITER)
					continue;
				
				// 区切り文字設定
				conversionKey += DELIMITER.ToString();
			}

			// キー再登録
			newDictionary[conversionKey] = valuePair.Value;
		}
	}
	#endregion
}