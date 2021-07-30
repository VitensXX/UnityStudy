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
        public static void AssetSaveAndOpenFragment(string assetPath, Action saveAction = null){
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

        static string searchTerm = string.Empty;
        public static string SearchFragment(){
            EditorGUILayout.BeginHorizontal();
            searchTerm = EditorGUILayout.TextField(searchTerm, EditorGUIStyle.searchField);
            GUIStyle style = string.IsNullOrEmpty(searchTerm) ? EditorGUIStyle.searchFieldCancelButtonEmpty : EditorGUIStyle.searchFieldCancelButton;
            if (GUILayout.Button("Cancel", style))
            {
                searchTerm = string.Empty;
            }
            EditorGUILayout.EndHorizontal();
            return searchTerm;
        }
        
    }
}
