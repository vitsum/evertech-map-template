#if UNITY_EDITOR

using System.Collections;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class AssetBundleCreator {
    private static string AndroidPath = "AssetBundles/Android";
    private static string WindowsPath = "AssetBundles/Windows64";
    private static string IOSPath = "AssetBundles/iOS";

    [MenuItem("Custom Tools/Build Map for all platforms", false, 0)]
    public static void BuildAllPlatforms()
    {
        ClearFolders();
        BuildBundlesAndroid();
        BuildBundlesIOS();
        BuildBundlesWindows64();
    }

    // NOTE: every "only" build clears ALL platform output folders first, so any
    // previously built platforms are wiped — only the chosen one is rebuilt.

    [MenuItem("Custom Tools/Build Map for Android only (clears other platforms)", false, 20)]
    public static void BuildAndroid(){
        ClearFolders();
        BuildBundlesAndroid();
    }

    [MenuItem("Custom Tools/Build Map for Windows only (clears other platforms)", false, 21)]
    public static void BuildWindows(){
        ClearFolders();
        BuildBundlesWindows64();
    }

    [MenuItem("Custom Tools/Build Map for iOS only (clears other platforms)", false, 22)]
    public static void BuildIOS(){
        ClearFolders();
        BuildBundlesIOS();
    }
    
    private static void BuildBundlesAndroid()
    {
        BuildPipeline.BuildAssetBundles("Assets/" + AndroidPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    private static void BuildBundlesIOS()
    {
        BuildPipeline.BuildAssetBundles("Assets/" + IOSPath, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    private static void BuildBundlesWindows64()
    {
        BuildPipeline.BuildAssetBundles("Assets/" + WindowsPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    private static void ClearFolder(string path)
    {
        Debug.Log("path: " + path);
        if (Directory.Exists(path)) {
            Debug.Log("delete");
            Directory.Delete(path, true);
        }
        Debug.Log("sleep");
        Thread.Sleep(1000);
        Debug.Log("create dir");
        Directory.CreateDirectory(path);
    }

    private static void ClearFolders()
    {
        ClearFolder(Application.dataPath + "/" + AndroidPath);
        ClearFolder(Application.dataPath + "/" + WindowsPath);
        ClearFolder(Application.dataPath + "/" + IOSPath);
    }
}

#endif