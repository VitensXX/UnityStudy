using UnityEditor;
using UnityEngine;

public class ShowAssetPath : Editor
{
    [MenuItem("Assets/LogPath")]
    public static void LogPath()
    {
        //支持多选
        string[] guids = Selection.assetGUIDs;
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            Debug.Log(assetPath);
        }
    }

    [MenuItem("Assets/LogPath", true)]
    public static bool ValidateLogPath()
    {
        return false;//返回true则为正常可点击状态 false代表变灰不可点击状态
    }
}
