using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#region Head
/*
 * [OverView]	サウンド定数管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class SoundConstantsCreator : AssetPostProcesserEx {
	
	#region static_function
	// 監視ディレクトリに変更があれば自動実行
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		// 確認情報
		List<string[]> assetsList = new List<string[]>() {

			importedAssets, deletedAssets, movedAssets, movedFromAssetPaths
		};

		// 監視ディレクトリ
		List<string> targetDirectoryNameList = new List<string>() {

			DirectoryPath.ResourcePath + DirectoryPath.BGMPath,
			DirectoryPath.ResourcePath + DirectoryPath.SEPath
		};

		// 変更確認
		if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList)) {

			// 作成
			Create();
		}
	}

	// 作成
	[MenuItem("Tools/Create/SoundConstants &s")]
	public static void Create()
	{
		// SoundDataファイルパス作成
		string dataPath = DirectoryPath.ResourcePath + FileName.SoundDataFile + Extension.AssetUnity;

		// SoundDataを新規作成、既にあるものは削除
		SoundData soundData = ScriptableObject.CreateInstance<SoundData>();
		AssetDatabase.DeleteAsset(dataPath);
		AssetDatabase.CreateAsset(soundData, dataPath);
		soundData.hideFlags = HideFlags.NotEditable;

		// BGM作成
		string folderPath = DirectoryPath.ResourcePath + DirectoryPath.BGMPath;
		CreateSound("BGM", folderPath, ref soundData);

		// SE作成
		folderPath = DirectoryPath.ResourcePath + DirectoryPath.SEPath;
		CreateSound("SE", folderPath, ref soundData);

		// 変更を反映
		ScriptableObject obj = AssetDatabase.LoadAssetAtPath(dataPath, typeof(ScriptableObject)) as ScriptableObject;
		EditorUtility.SetDirty(obj);

		// デバッグメッセージ表示
		Debug.Log("Complete execution [SoundConstants]");
	}

	// 音情報作成
	private static void CreateSound(string classValue, string folderPath, ref SoundData soundData)
	{
		// 音情報取得
		string[] soundList = Resources.LoadAll(folderPath).Select(obj => obj.name).ToArray();

		// Array(string) => Dictionary(string, int)
		Dictionary<string, int> soundDictionary = soundList.Select((s, i) => new { s, i }).ToDictionary(v => v.s, v => v.i);

		// 列挙番号修正
		foreach(string soundKey in soundDictionary.Keys)
			soundDictionary[soundKey] += soundData.list.Count;

		// スクリプト作成
		ConstantsEnumCreator<int> enumCreator = new ConstantsEnumCreator<int>();
		enumCreator.Create(classValue, classValue, soundDictionary);

		// データ登録
		foreach (KeyValuePair<string, int> valuePair in soundDictionary) {

			// 新規作成
			SoundData.Param param = new SoundData.Param();
			param.Name = valuePair.Key;
			param.No = valuePair.Value;

			// 追加
			soundData.list.Add(param);
		}
	}
	#endregion
}