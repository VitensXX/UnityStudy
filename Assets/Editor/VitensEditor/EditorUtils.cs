using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Created by Vitens on 2021/7/31 15:24:45
/// 
/// Description : 
///     编辑器下的工具类
/// </summary>
namespace Vitens.Editor
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

        //拷贝文本
        static TextEditor textEditor;
        public static void CopyStr(string str){
            if(textEditor == null){
                textEditor = new TextEditor();
            }
            textEditor.text = str;
            textEditor.OnFocus();
            textEditor.Copy();
        }
        
        //粘贴文本
        public static string PastStr(){
            if(textEditor != null){
                return textEditor.text;
            }
            else{
                Debug.Log("Need copy first!");
                return "";
            }
        }

        static Color colorClipboard = Color.white;
        public static void CopyColor(Color color){
            colorClipboard = color;
        }

        public static Color PasteColor(){
            return colorClipboard;
        }
    }
}