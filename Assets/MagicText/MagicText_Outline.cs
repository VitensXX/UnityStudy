using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 描边效果
/// </summary>
public class MagicText_Outline : Shadow
{
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        var start = 0;
        var end = 0;

        //和自带的Outline的差异就在这里,分别向四个角落方向扩展四份网格,弥补之前的不足
        //相应的带来了额外的消耗主要在OverDraw上.
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i != 0) && (j != 0))
                {
                    start = end;
                    end = verts.Count;
                    ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, i * effectDistance.x * 0.7f, j * effectDistance.y * 0.7f);
                }
            }
        }

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -effectDistance.x, 0);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, effectDistance.x, 0);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, -effectDistance.y);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, effectDistance.y);

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}
