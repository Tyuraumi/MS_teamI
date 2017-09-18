using UnityEditor;
using System.Collections.Generic;
using System.Linq;

#region Head
/*
 * [OverView]	ファイル変更監視クラス
 * [ Author ]	SUZUKI YUZI
*/
#endregion
public class AssetPostProcesserEx : AssetPostprocessor {

	#region function
	// 特定ディレクトリ内の特定ファイルを検出
	protected static bool ExistsDirectoryInAssets(List<string[]> assetsList, List<string> targetDirectoryNameList)
	{
		return assetsList
				.Any(assets => assets										// 入力assetListから
				.Select(asset => System.IO.Path.GetDirectoryName(asset))	// assetListのファイルパス取得
				.Intersect(targetDirectoryNameList)							// 確認ファイルパスと比較
				.Count() > 0);												// 一致数確認
	}
	#endregion
}