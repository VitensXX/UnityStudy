using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2021/10/14 21:06:10
/// 
/// Description : 
///     自定义网格图片
/// </summary>
public class MeshImage : RawImage
{
    public Mesh mesh;
 
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if(mesh == null){
            base.OnPopulateMesh(vh);
        }
        else{
            vh.Clear();
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                vh.AddVert(mesh.vertices[i], color, mesh.uv[i]);
            }

            for (int i = 0; i < mesh.triangles.Length; i+=3)
            {
                vh.AddTriangle(mesh.triangles[i], mesh.triangles[i+1], mesh.triangles[i+2]);
            }
        }
    }
}