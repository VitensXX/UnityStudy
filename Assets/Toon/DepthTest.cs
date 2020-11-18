using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthTest : MonoBehaviour
{
    //private Material postEffectMat = null;
    private Camera currentCamera = null;

    void Awake()
    {
        currentCamera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        //if (postEffectMat == null)
        //    postEffectMat = new Material(Shader.Find("Unlit/OutlineLight"));
        currentCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnDisable()
    {
        //currentCamera.depthTextureMode &= ~DepthTextureMode.Depth;
    }

    //void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    if (postEffectMat == null)
    //    {
    //        Graphics.Blit(source, destination);
    //    }
    //    else
    //    {
    //        Graphics.Blit(source, destination, postEffectMat);
    //    }
    //}
}