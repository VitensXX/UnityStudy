using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2022/5/28 14:49:15
/// 
/// Description : 
///     基于后处理实现的转场效果
/// </summary>
[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class PostProcessSceneChange : MonoBehaviour
{
    public Color color = Color.white;
    [Range(0,1)]
    public float factor;
    public float strength = 10;
    public Vector2 center = new Vector2(0.5f, 0.5f);
    public Shader shader;
    Material _mat;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    void Start()
    {
        _mat = new Material(shader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        _mat.SetFloat("_Factor", factor * strength);
        _mat.SetFloat("_Strength", strength);
        _mat.SetVector("_Center", center);
        _mat.SetColor("_bgColor", color);
        src.wrapMode = TextureWrapMode.Mirror;
        Graphics.Blit (src, dest, _mat); 
    }

    //转场开始
    public void PlayForward(){
        // factor 从0->1
        // curve.Evaluate()
    }

    //转场结束
    public void PlayBack(){
        // factor 从1->0
        
        //结束时 让此脚本失效(enable false)

    }
}
