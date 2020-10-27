using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOutlineModifyMesh : IModifyMesh
{
    void IModifyMesh.ModifyMesh(VertexHelper vh)
    {
        //if (!IsActive())
        //{
        //    return;
        //}

        //List<UIVertex> verts = new List<UIVertex>();
        //vh.GetUIVertexStream(verts);

        //int initialVertexCount = verts.Count;

        //var start = 0;
        //var end = 0;

        //for (int i = -1; i <= 1; i++)
        //{
        //    for (int j = -1; j <= 1; j++)
        //    {
        //        if ((i != 0) && (j != 0))
        //        {
        //            start = end;
        //            end = verts.Count;
        //            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, i * effectDistance.x * 0.707f, j * effectDistance.y * 0.707f);
        //        }
        //    }
        //}

        //start = end;
        //end = verts.Count;
        //ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, 0);

        //start = end;
        //end = verts.Count;
        //ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, 0);


        //start = end;
        //end = verts.Count;
        //ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, -effectDistance.y);

        //start = end;
        //end = verts.Count;
        //ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, effectDistance.y);


        //if (GetComponent<Text>().material.shader == Shader.Find("Text Effects/Fancy Text"))
        //{
        //    for (int i = 0; i < verts.Count - initialVertexCount; i++)
        //    {
        //        UIVertex vert = verts[i];
        //        vert.uv1 = new Vector2(0, 0);
        //        verts[i] = vert;
        //    }
        //}

        //vh.Clear();
        //vh.AddUIVertexTriangleStream(verts);
    }
}
