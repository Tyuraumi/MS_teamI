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

	#region class
	private class SceneData : ScriptableObject {

		#region class
		[System.SerializableAttribute]
		public class Param {

			public int No;			// シーン番号
			public string Name;		// シーン名
		}
		#endregion

		#region variable
		public List<Param> list = new List<Param>();	// シーン情報
		#endregion
	}
	#endregion

	#region variable
	private const string TARGET_DIRECTORY_NAME = "ProjectSettings";		// 監視ディレクトリ
	private const string COMMAND_NAME = "Tools/Create/Setting Class";   // コマンド名
	#endregion

	#region function
	// 監視ディレクトリに変更があれば自動実行
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		// 確認情報
		List<string[]> assetsList = new List<string[]>(){
			importedAssets
		};

		// 監視ディレクトリ
		List<string> targetDirectoryNameList = new List<string>(){
			TARGET_DIRECTORY_NAME
		};

		// 変更確認
		if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList)) {

			// 作成
			Create();
		}
	}

	// 作成
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
	}

	// タグ情報作成
	private static void CreateTag()
	{
		// タグ情報取得
		Dictionary<string, string> tagDictionary = InternalEditorUtility.tags.ToDictionary(value => value);

		// スクリプト作成
		ConstantsCreator.Create("TagName", "タグ名クラス", tagDictionary);
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
		ConstantsCreator.Create("SceneName", "シーン名クラス", scenesNameDictionary);
		ConstantsCreator.Create("SceneNo", "シーン番号クラス", scenesNoDictionary);

		// シーンデータ書出し
		{
			// ファイル情報設定
			string dataPath = "Assets/Resources/SceneData.asset";

			//SceneDataを新規作成、既にあるものは削除
			SceneData data = ScriptableObject.CreateInstance<SceneData>();
			AssetDatabase.DeleteAsset(dataPath);
			AssetDatabase.CreateAsset((ScriptableObject)data, dataPath);

			data.hideFlags = HideFlags.NotEditable;

			foreach (KeyValuePair<string, int> valuePair in scenesNoDictionary) {

				SceneData.Param param = new SceneData.Param();
				param.Name = valuePair.Key;
				param.No = valuePair.Value;

				data.list.Add(param);
			}

			//変更を反映
			ScriptableObject obj = AssetDatabase.LoadAssetAtPath(dataPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty(obj);
		}
	}

	// レイヤー情報作成
	private static void CreateLayer()
	{
		// レイヤー番号取得
		Dictionary<string, int> layerNoDictionary = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => LayerMask.NameToLayer(layer));
		
		// レイヤーマスク番号取得
		Dictionary<string, int> layerMaskNoDictionary = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => 1 << LayerMask.NameToLayer(layer));
		
		// スクリプト作成
		ConstantsCreator.Create("LayerNo", "レイヤー番号クラス", layerNoDictionary);
		ConstantsCreator.Create("LayerMaskNo", "レイヤーマスク番号クラス", layerMaskNoDictionary);
	}

	// ソーティングレイヤー情報作成
	private static void CreateSortingLayer()
	{
		// ソーティングレイヤー情報取得
		string[] sortingLayerName;
		{
			Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			sortingLayerName = (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}

		// ソーティングレイヤー名取得
		Dictionary<string, string> sortingLayerDictionary = sortingLayerName.ToDictionary(value => value);

		// スクリプト作成
		ConstantsCreator.Create("SortingLayerName", "ソーティングレイヤー名クラス", sortingLayerDictionary);
	}
	#endregion
}