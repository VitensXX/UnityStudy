using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Created by Vitens on 2022/5/7 8:38:07
/// 
/// Description : 
///     SDF图片生成工具
/// </summary>
public class SDFMaker : EditorWindow
{
    Texture2D _sorceTexture;
    Object _dir;
    string _sdfName;
    const int TITLE_WIDTH = 60;

    [MenuItem("Tools/SDF Maker")]
    static void Create()
    {
        SDFMaker window = (SDFMaker)GetWindowWithRect(typeof(SDFMaker), new Rect(-1, -1, 320, 260), true, "SDF Maker");
        window.Show(true);
    }

    void OnGUI(){
        //选择图片
        SelectSorceTextureGUI();
        //选择生成目录
        SetDestFolderGUI();
        //设置名字
        SetDestName();
        
        if(GUILayout.Button("生成")){
            Maker();
        }
    }

    //选择原图片
    void SelectSorceTextureGUI(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("选择图片", GUILayout.Width(TITLE_WIDTH));
        _sorceTexture = EditorGUILayout.ObjectField(_sorceTexture, typeof(Texture2D), false) as Texture2D;
        EditorGUILayout.EndHorizontal();
    }

    //设置生成目录
    void SetDestFolderGUI(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("生成目录", GUILayout.Width(TITLE_WIDTH));
        _dir = EditorGUILayout.ObjectField(_dir, typeof(Object), false);
        if(_dir != null){
            if(!Directory.Exists(AssetDatabase.GetAssetPath(_dir))){
                ShowErrorTip("请选择文件夹");
            }
            else{
                ShowOKTip();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    //设置生成的名字
    void SetDestName(){
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("设置名字", GUILayout.Width(TITLE_WIDTH));
        _sdfName = GUILayout.TextField(_sdfName);
        if(string.IsNullOrEmpty(_sdfName) && _sorceTexture != null){
            _sdfName = _sorceTexture.name + "_SDF";
        }
        EditorGUILayout.EndHorizontal();
    }

    void Maker(){
        string path = AssetDatabase.GetAssetPath(_dir);
        Debug.LogError(path +"  "+ Directory.Exists(path));
    }

#region SDF生成算法

    // int distance = 10;
    // public void Test(){
    //     int width = _sorceTexture.width;
    //     int height = _sorceTexture.height;

    //     float[,] originAlpha = new float[width, height];
    //     for (int y = 0; y < height; y++)
    //     {
    //         for (int x = 0; x < width; x++)
    //         {
    //             originAlpha[x,y] = _sorceTexture.GetPixel(x,y).a;
    //         }
    //     }

    //     sdfPreviewTexture = new Texture2D(width, height);
    //     for (int y = 0; y < height; y++)
    //     {
    //         for (int x = 0; x < width; x++)
    //         {
    //             float minDis = Distance(x, y);
    //             sdfPreviewTexture.SetPixel(x,y, Color.white * (1 - minDis / distance));
    //         }
    //     }
    //     sdfPreviewTexture.Apply();


    //     Debug.LogError("Apply");
    //     // SDFImageMaker.GenerateSDF(sorce,dest,distance);
    //     SDFImageMaker.GenerateBinaryImage(sdfPreviewTexture);
    //     AssetDatabase.Refresh();
    // }

    // bool IsPixelSafe(int x, int y){
    //     return x < width && x >= 0 && y < height && y >= 0;  
    // }

    // float Distance(int x, int y){
    //     int halfDis = distance;
    //     float minDis = distance;
    //     for (int offsetX = -halfDis; offsetX < halfDis; offsetX++)
    //     {
    //         for (int offsetY = -halfDis; offsetY < halfDis; offsetY++)
    //         {
    //             int tempX = x + offsetX;
    //             int tempY = y + offsetY;

    //             if(!IsPixelSafe(tempX,tempY)){
    //                 continue;
    //             }
                
    //             if(originAlpha[tempX,tempY] >= AlphaThreshold){
    //                 float curDis = Mathf.Sqrt(Mathf.Abs(offsetX) * Mathf.Abs(offsetX) + Mathf.Abs(offsetY) * Mathf.Abs(offsetY));
    //                 if(minDis > curDis){
    //                     minDis = curDis;
    //                 }
    //             }
    //         }
    //     }

    //     return minDis;
    // }
        
#endregion

#region 工具方法

    //显示错误提示信息
    void ShowErrorTip(string tip){
        GUI.color = new Color(1, 0.5f, 0.5f);
        GUILayout.Label(tip);
        GUI.color = Color.white;
    }

    //显示参数正确提示
    void ShowOKTip(string tip = ""){
        GUI.color = Color.green;
        GUILayout.Label(tip == "" ? "√" : tip);
        GUI.color = Color.white;
    }

#endregion


}
