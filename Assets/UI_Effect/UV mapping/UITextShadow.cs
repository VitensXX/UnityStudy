using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitens.UIExtension
{
    /// <summary>
    /// Created by Vitens on 2022/5/14 19:42:37
    /// 
    /// Description : 
    ///     UI界面Text组件带透视效果的阴影
    /// </summary>
    public class UITextShadow : BaseMeshEffect
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

            int countOfOneText = 6;
            int vertexCount = verts.Count;
            float underPosY = verts[2].position.y;
            UIVertex vt;

            for (int i = 0; i < vertexCount; ++i)
            {
                vt = verts[i];
                verts.Add(vt);
                Vector3 v = vt.position;
                vt.color = color;
                if(i % countOfOneText == 0 || i % countOfOneText == 1 || i % countOfOneText == 5){// 0 1 5
                    v.x += offset.x * (v.y - underPosY);
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
