using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2022/6/25 15:31:01
/// 
/// Description : 
///     在UI上播放网格的顶点动画（主要为了解决在UI上播放骨骼动画）
/// </summary>
public class VertexAnimOnUI : MeshImage
{
    public Texture animTex;
    public Shader shader;
    public bool flip;
    public float scale = 1;
    public Vector2 posOffset;

    Material _mat;
    int _animMapId;
    int _OffsetAndScaleId;
    int _flipId;
    RectTransform _parent;

    protected override void OnEnable() {
        base.OnEnable();
        if(_mat == null){
            _mat = new Material(shader);
            _animMapId = Shader.PropertyToID("_AnimMap");
            _OffsetAndScaleId = Shader.PropertyToID("_OffsetAndScale");
            _flipId = Shader.PropertyToID("_Flip");
            this.material = _mat;
        }

        _mat.SetTexture(_animMapId, animTex);
        _parent = transform.parent as RectTransform;
    }
   
    void Update()
    {
        if(_mat != null){
            //overlay的方式 用下面这种
            // _mat.SetVector(_OffsetAndScaleId, new Vector4(worldPos.x - Screen.width / 2, worldPos.y  - Screen.height / 2, 0, scale));
            Vector2 pos = _parent.anchoredPosition;
            _mat.SetVector(_OffsetAndScaleId, new Vector4(pos.x, pos.y, 0, scale));
            _mat.SetFloat(_flipId, flip ? -1 : 1);
        }
    }

}
