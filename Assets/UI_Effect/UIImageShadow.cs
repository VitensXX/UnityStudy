using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitens.UIExtension
{
    /// <summary>
    /// Created by Vitens on 2022/5/14 21:05:12
    /// 
    /// Description : 
    ///     UI图片带透视的阴影效果
    /// </summary>
    public class UIImageShadow : BaseMeshEffect
    {
        public bool fade = true;
        public Color color = Color.black;
        public Vector2 offset = Vector2.zero;
        public override void ModifyMesh(VertexHelper vh)
        {
            if(!enabled){
                return;
            }

            List<UIVertex> verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);

            int count = verts.Count;
            UIVertex vt;
            for (int i = 0; i < count; ++i)
            {
                vt = verts[i];
                verts.Add(vt);
                Vector3 v = vt.position;
                vt.color = color;
                if(i == 1 || i == 2 || i == 3){//1  2 3
                    v.x += offset.x * 100;
                    v.y += offset.y * 100;
                    if(fade){
                        vt.color.a *= 0;
                    }
                }
                vt.position = v;
                verts[i] = vt;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
        }
    }
}