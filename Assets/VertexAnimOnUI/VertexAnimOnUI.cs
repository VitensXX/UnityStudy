using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2022/6/25 15:31:01
/// 
/// Description : 
///     在UI上播放网格的顶点动画（主要为了解决在UI上播放骨骼动画）
/// </summary>
public class VertexAnimOnUI : MeshImage
{
    public Texture animTex;
    protected override void OnValidate() {
        base.OnValidate();
        Debug.LogError("onvalidata");
        RefreshMesh();
    }
   
    void Update()
    {
        
    }
}
