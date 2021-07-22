using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DepthTextureWindow : EditorWindow
{
    [MenuItem("Tools/Depth Texture Window")]
    public static void GetWindow()
    {
        DepthTextureWindow window = (DepthTextureWindow)EditorWindow.GetWindow(typeof(DepthTextureWindow), false, "DepthTexture");
        window.Show(true);
        window.Init();
    }

    RenderTexture _rt;
    void Init(){
        _rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
        Debug.LogError("!!! "+_rt);
        // depthTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/RawArt/512.png");
    }
    
    Texture depthTexture;
    private void OnGUI() {
        // Debug.LogError("!");
        Camera.main.targetTexture = _rt;
        // depthTexture = _rt as Texture;
        // Debug.LogError(_rt);
        // depthTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/RawArt/test.renderTexture");
        GUI.DrawTexture(new Rect(100,100, 200, 200), _rt);
        // Camera.main.targetTexture = null;
        // depthTexture = EditorGUILayout.ObjectField(depthTexture, typeof(Texture), true) as Texture;
    }

    private void Update() {
        // Camera.main.targetTexture = _rt;
        // depthTexture = _rt;
        Repaint();
    }

    private void OnDestroy() {
        RenderTexture.ReleaseTemporary(_rt);
        _rt = null;
    }

}
