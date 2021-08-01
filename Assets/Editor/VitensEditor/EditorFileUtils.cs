using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Created by Vitens on 2021/7/31 1:18:36
/// 
/// Description : 
///     编辑器下使用的文件操作相关工具类
/// </summary>
namespace Vitens.Editor
{
    public class EditorFileUtils
    {
        
        #region 对外接口
        
        //打开文件 文件夹直接打开目录, 文件只用选中即可
        public static void ShowInExplorer(string path){
            path = path.Replace(@"/", @"\");
            FileType fileType = CheckFile(path);
            if(fileType == FileType.None){
                Debug.LogError("文件不存在:"+path);
                return;
            }
            
            if (fileType == FileType.File)
            {
                path = "/select," + path;
            }
            Debug.Log("open: "+path); 
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        //打开脚本
        public static void OpenScripts(string assetPath, int line){
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath), line);
        }

        //通过路径获取文件名
        public static string GetFileName(string path){
            string[] temp = path.Split('/');
            return temp[temp.Length - 1];
        }

        #endregion 对外接口

        #region 内部方法
            
        //文件类型 是否为文件或文件夹,或者不存在
        enum FileType{None, File, Dir}
        
        //检测文件类型
        static FileType CheckFile(string path){
            if(File.Exists(path)){
                return FileType.File;
            }
            else if(Directory.Exists(path)){
                return FileType.Dir;
            }
            else{
                return FileType.None;
            }
        }

        #endregion 内部方法

    }
}
