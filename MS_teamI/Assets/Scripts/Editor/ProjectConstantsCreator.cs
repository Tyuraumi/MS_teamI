using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#region Head
/*
 * [OverView]	プロジェクト内定数管理クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class ProjectSettingsCreator : AssetPostProcesserEx {

	#region static_function
	// 監視ディレクトリに変更があれば自動実行
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		// 確認情報
		List<string[]> assetsList = new List<string[]>() {

			importedAssets
		};

		// 監視ディレクトリ
		List<string> targetDirectoryNameList = new List<string>() {

			DirectoryPath.ProjectSettingPath
		};

		// 変更確認
		if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList)) {

			// 作成
			Create();
		}
	}

	// 作成
	[MenuItem("Tools/Create/ProjectConstants &p")]
	public static void Create()
	{
		// タグ作成
		CreateTag();

		// シーン情報作成
		CreateScene();

		// レイヤー情報作成
		CreateLayer();

		//ソーティングレイヤー情報作成
		CreateSortingLayer();

		// デバッグメッセージ表示
		Debug.Log("Complete execution [CreateConstants]");
	}

	// タグ情報作成
	private static void CreateTag()
	{
		// タグ情報取得
		Dictionary<string, string> tagDictionary = InternalEditorUtility.tags.ToDictionary(value => value);

		// スクリプト作成
		ConstantsNameCreator<string> nameCreator = new ConstantsNameCreator<string>();
		nameCreator.Create("Tag", "タグ", tagDictionary);
	}

	// シーン情報作成
	private static void CreateScene()
	{
		// シーン情報取得
		Dictionary<string, string> scenesNameDictionary = new Dictionary<string, string>();
		Dictionary<string, int> scenesNoDictionary = new Dictionary<string, int>();
		for (int i = 0; i < EditorBuildSettings.scenes.Count(); i++) {

			string sceneName = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
			scenesNameDictionary[sceneName] = sceneName;
			scenesNoDictionary[sceneName] = i;
		}

		// スクリプト作成
		ConstantsNameCreator<string> nameCreator = new ConstantsNameCreator<string>();
		nameCreator.Create("Scene", "シーン", scenesNameDictionary);
		ConstantsEnumCreator<int> enumCreator = new ConstantsEnumCreator<int>();
		enumCreator.Create("Scene", "シーン", scenesNoDictionary);

		// シーンデータ書出し
		{
			// SceneDataパス作成
			string dataPath = DirectoryPath.ResourcePath + FileName.SceneDataFile + Extension.AssetUnity;

			// SceneDataを新規作成、既にあるものは削除
			SceneData data = ScriptableObject.CreateInstance<SceneData>();
			AssetDatabase.DeleteAsset(dataPath);
			AssetDatabase.CreateAsset(data, dataPath);
			data.hideFlags = HideFlags.NotEditable;

			// データ登録
			foreach (KeyValuePair<string, int> valuePair in scenesNoDictionary) {

				// 新規作成
				SceneData.Param param = new SceneData.Param();
				param.Name = valuePair.Key;
				param.No = valuePair.Value;

				// 追加
				data.list.Add(param);
			}

			// 変更を反映
			ScriptableObject obj = AssetDatabase.LoadAssetAtPath(dataPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty(obj);
		}
	}

	// レイヤー情報作成
	private static void CreateLayer()
	{
		// レイヤー情報取得
		string[] IEUlayers = InternalEditorUtility.layers;

		// レイヤー番号取得
		Dictionary<string, int> layerNoDictionary = IEUlayers.ToDictionary(layer => layer, layer => LayerMask.NameToLayer(layer));
		
		// レイヤーマスク番号取得
		Dictionary<string, int> layerMaskNoDictionary = IEUlayers.ToDictionary(layer => layer, layer => 1 << LayerMask.NameToLayer(layer));

		// スクリプト作成
		ConstantsEnumCreator<int> enumCreator = new ConstantsEnumCreator<int>();
		enumCreator.Create("Layer", "レイヤー", layerNoDictionary);
		enumCreator.Create("LayerMask", "レイヤーマスク", layerMaskNoDictionary);
	}

	// ソーティングレイヤー情報作成
	private static void CreateSortingLayer()
	{
		// ソーティングレイヤー情報取得
		string[] sortingLayerName;
		{
			Type IEUType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = IEUType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			sortingLayerName = (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}

		// ソーティングレイヤー名取得
		Dictionary<string, string> sortingLayerDictionary = sortingLayerName.ToDictionary(value => value);

		// スクリプト作成
		ConstantsNameCreator<string> nameCreator = new ConstantsNameCreator<string>();
		nameCreator.Create("SortingLayer", "ソーティングレイヤー", sortingLayerDictionary);
	}
	#endregion
}