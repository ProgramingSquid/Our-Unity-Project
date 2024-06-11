using UnityEditor;
using UnityEngine;

public static class AssetUtilities
{
    public static string GetAssetFolder(string assetPath)
    {
        // Get the directory part of the asset path
        string directory = System.IO.Path.GetDirectoryName(assetPath);
        return directory;
    }
}