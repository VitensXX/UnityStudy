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
            UIVertex vt;
            float maxHeight = float.MinValue;
            //获得每个字的高度
            float[] heightForPerText = new float[vertexCount / countOfOneText];
            for (int i = 0; i < heightForPerText.Length; i++)
            {
                //1 是上面的点  2是下面的点
                vt = verts[countOfOneText * i + 1];
                float max = vt.position.y;
                vt = verts[countOfOneText * i + 2];
                float min = vt.position.y;
                float height = max - min;
                heightForPerText[i] = height;
                maxHeight = Mathf.Max(height, maxHeight);
            }

            for (int i = 0; i < vertexCount; ++i)
            {
                vt = verts[i];
                verts.Add(vt);
                Vector3 v = vt.position;
                vt.color = color;
                if(i % countOfOneText == 0 || i % countOfOneText == 1 || i % countOfOneText == 5){// 0 1 5
                    int textCount = i / countOfOneText;
                    float height = heightForPerText[textCount];
                    v.y += (offset.y * height);
                    v.x += offset.x * height;
                    if(fade){
                        vt.color = new Color(color.r, color.g, color.b, color.a * (1 - height / maxHeight));
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
