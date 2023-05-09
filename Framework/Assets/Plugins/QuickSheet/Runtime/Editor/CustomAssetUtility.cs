using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class CustomAssetUtility
{
    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }

    public static string GetUniqueAssetPathNameOrFallback(string filename)
    {
        string path;
        try
        {
            // Private implementation of a file naming function which puts the file at the selected path.
            Type assetdatabase = typeof(AssetDatabase);
            path = (string)assetdatabase.GetMethod("GetUniquePathNameAtSelectedPath",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(assetdatabase, new object[] { filename });
        }
        catch
        {
            // Protection against implementation changes.
            path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + filename);
        }

        return path;
    }
}