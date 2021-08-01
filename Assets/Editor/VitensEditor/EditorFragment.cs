using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Created by Vitens on 2021/7/31 1:43:13
/// 
/// Description : 
///     编辑器自定义窗口的控件封装
/// </summary>
namespace Vitens.Editor{

    public class EditorFragment
    {
        //数据的保存于文件目录的打开
        public static void AssetSaveAndOpen(string assetPath, Action saveAction = null){
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("数据存储在 : "+ EditorFileUtils.GetFileName(assetPath));
            if (GUILayout.Button("打开文件夹", GUILayout.Height(20), GUILayout.Width(80))){
                EditorFileUtils.ShowInExplorer(assetPath);
            }
            if(saveAction != null){
                if (GUILayout.Button("保存",  GUILayout.Height(20), GUILayout.Width(80))){
                    saveAction();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //搜索条
        public static string Search(string searchInput, Action cancel = null){
            EditorGUILayout.BeginHorizontal();
            searchInput = EditorGUILayout.TextField(searchInput, EditorGUIStyle.SearchTextField);
            GUIStyle style = string.IsNullOrEmpty(searchInput) ? EditorGUIStyle.SearchCancelButtonEmpty : EditorGUIStyle.SearchCancelButton;
            if (GUILayout.Button("Cancel", style))
            {
                searchInput = string.Empty;
                cancel?.Invoke();
            }
            EditorGUILayout.EndHorizontal();

            return searchInput;
        }

#region 按钮
    
        //添加按钮 +
        public static void BtnAdd(Action onClick, string desc = "", int width = 20){
            if(GUILayout.Button("", EditorGUIStyle.OL_Pluse, GUILayout.Width(20))){
                onClick?.Invoke();
            }
        }

        //删除按钮 -
        public static void BtnDelete(Action onClick, string desc = "", int width = 20){
            if(GUILayout.Button("", EditorGUIStyle.OL_Minus, GUILayout.Width(20))){
                onClick?.Invoke();
            }
        }

        // static TextEditor textEditor;
        // //复制到剪切板按钮 
        // public static void BtnCopyString(string str){
        //     if(textEditor == null){
        //         textEditor = new TextEditor();
        //     }
        //     textEditor.text = str;
        //     textEditor.OnFocus();
        //     textEditor.Copy();
        // }

        // public static string BtnPastString(){
        //     if(textEditor != null){
        //         return textEditor.text;
        //     }
        //     else{
        //         Debug.Log("need copy first");
        //         return "";
        //     }
        // }

#endregion 按钮

        //尾部延伸
        public static void Extend(){
            EditorGUILayout.LabelField("");
        }

    }
}
