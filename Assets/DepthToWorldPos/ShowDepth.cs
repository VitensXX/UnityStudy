using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2021/7/20 11:35:53
/// 
/// Description : 
///     深度图测试
/// </summary>
[ExecuteInEditMode]
public class ShowDepth : MonoBehaviour
{
    private Material postEffectMat = null;
    private Camera currentCamera = null;

    void Awake()
    {
        currentCamera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        if (postEffectMat == null)
            postEffectMat = new Material(Shader.Find("Test/ShowDepth"));
        currentCamera.depthTextureMode |= DepthTextureMode.Depth;
    }

    void OnDisable()
    {
        currentCamera.depthTextureMode &= ~DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (postEffectMat == null)
        {
            Graphics.Blit(source, destination);
        }
        else
        {
            Graphics.Blit(source, destination, postEffectMat);
        }
    }
}