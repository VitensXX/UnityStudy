using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2021/10/19 20:53:24
/// 
/// Description : 
///     透视效果的图片
/// </summary>
public class ImagePerspective : BaseMeshEffect
{
    public float offset;

    public override void ModifyMesh(VertexHelper vh)
    {
        UIVertex v = new UIVertex();
        //左上角的顶点
        vh.PopulateUIVertex(ref v, 1);
        //向右偏移
        v.position.x += offset;
        vh.SetUIVertex(v, 1);

        //右上角的顶点
        vh.PopulateUIVertex(ref v, 2);
        //向左偏移
        v.position.x -= offset;
        vh.SetUIVertex(v, 2);
    }
}
