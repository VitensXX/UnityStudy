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
    void Start()
    {
        _mat = new Material(shader);
    }

    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        _mat.SetFloat("_Factor", factor * strength);
        _mat.SetFloat("_Strength", strength);
        _mat.SetVector("_Center", center);
        _mat.SetColor("_bgColor", color);
        Graphics.Blit (src, _mat); 
    }
}
