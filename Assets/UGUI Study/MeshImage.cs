using System;
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
    Vector3[] _vertices;
    Vector2[] _uvs;
    Color[] _colors;
    int[] _triangles;


    protected override void OnPopulateMesh(VertexHelper vh)
    {
        
        if(mesh == null){
            base.OnPopulateMesh(vh);
        }
        else{
            if(_vertices == null){
                //注意:Mesh中的这些数组属性返回的都是副本,所以访问需谨慎,不然会产生大量GC,特别是顶点数非常多有在遍历中访问的情况!!!
                _vertices = mesh.vertices;
                _uvs = mesh.uv;
                _triangles = mesh.triangles;
                _colors = mesh.colors;
            }

            vh.Clear();
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                // vh.AddVert(mesh.vertices[i], color, mesh.uv[i]);
                Color col = color;
                if (_colors.Length != 0 && _colors.Length > i)
                    col = _colors[i] * color;
                vh.AddVert(_vertices[i], col, _uvs[i]);
            }

            for (int i = 0; i < mesh.triangles.Length; i+=3)
            {
                // vh.AddTriangle(mesh.triangles[i], mesh.triangles[i+1], mesh.triangles[i+2]);
                vh.AddTriangle(_triangles[i], _triangles[i+1], _triangles[i+2]);
            }
        }
    }

    //刷新网格
    public void RefreshMesh(){
        _vertices = null;
        _uvs = null;
        _colors = null;
        _triangles = null;
        SetAllDirty();
    }
}
