using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteUVGetter : MonoBehaviour
{
    void Start()
    {
        Image image = GetComponent<Image>();
        if(image){
            Vector4 uvRange = GetSpriteUVRange(image);
            Material mat = image.material;
            mat.SetVector("_UV_Range", uvRange);
        }
    }

    Vector4 GetSpriteUVRange(Image image){
        Sprite sp = image.sprite;
        if(sp == null){
            return Vector4.zero;
        }
        Vector2[] uv = sp.uv;
        //四个分量值的含义  x:uMin  y:uMax  z:vMin  w:vMax
        Vector4 uvRange = new Vector4(1,0,1,0);
        for (int i = 0; i < uv.Length; i++)
        {
            Vector2 tempUV = uv[i];
            uvRange.x = Mathf.Min(uvRange.x, tempUV.x);
            uvRange.y = Mathf.Max(uvRange.y, tempUV.x);
            uvRange.z = Mathf.Min(uvRange.z, tempUV.y);
            uvRange.w = Mathf.Max(uvRange.w, tempUV.y);
        }

        return uvRange;
    }
}
