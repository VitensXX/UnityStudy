using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2022/5/14 19:15:06
/// 
/// Description : 
///     UI网格的错切变换
/// </summary>
public class UIMeshShearTransform : BaseMeshEffect
{
    [Range(-1000,1000)]
    public float offset = 1;
    public override void ModifyMesh(VertexHelper vh)
    {
        Debug.LogError(vh.currentVertCount);
        var vertex = default(UIVertex);
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            //序号1,2对应上面两个顶点
            if(i % 4 == 0 || i % 4 == 1){
                float posX = vertex.position.x; 
                vertex.position.x = posX + offset;
                vertex.color.a *= 0;
            }

            vh.SetUIVertex(vertex, i);
        }
    }
}
