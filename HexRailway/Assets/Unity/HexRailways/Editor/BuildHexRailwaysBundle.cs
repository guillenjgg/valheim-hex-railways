using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildHexRailwaysBundle
{
    [MenuItem("Assets/Build HexRailways Bundle")]
    public static void BuildBundle()
    {
        const string bundleName = "hexrailways";
        const string prefabPath = "Assets/HexRailways/Prefabs/hex_railways_wood_rail_straight_4m.prefab";

        const string outputPath = "Assets/HexRailways/AssetBundles";

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        AssetBundleBuild build = new AssetBundleBuild
        {
            assetBundleName = bundleName,
            assetNames = new[]
            {
                prefabPath
            }
        };

        BuildPipeline.BuildAssetBundles(
            outputPath,
            new[] { build },
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);

        Debug.Log($"Built asset bundle: {bundleName}");
    }
}