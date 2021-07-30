using UnityEngine;
using UnityEditor;

namespace Vitens.UnitEditor
{
    public class EditorUtils
    {
        //获取Project窗口当前选中的文件路径(多选的话只返回第一个)
        public static string GetOneSelectFilePath()
        {
            string[] guids = Selection.assetGUIDs;
            if (guids.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(guids[0]);
            }
            else
            {
                return "";
            }
        }

        
    }
}
