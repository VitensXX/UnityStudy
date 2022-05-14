using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitens.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic))]
    //被SoftMask管理的UI元素,代码动态添加不需要手动挂载
    public class SoftMaskable : MonoBehaviour
    {
        Material _mat;
        // Start is called before the first frame update
        void OnEnable()
        {
            Graphic graphic = GetComponent<Graphic>();
            Debug.LogError(graphic.material.name);
            // _mat = graphic.material;
            _mat = new Material(Shader.Find("UI/UI-SoftMask"));
            _mat.hideFlags = HideFlags.HideAndDontSave;
            graphic.material = _mat;
            // _mat.hideFlags = HideFlags.NotEditable;
        }

        public void SetSoftParam(Vector4 rect, Vector4 param){
            _mat.SetVector("_SoftRect", rect);
            _mat.SetVector("_MaskParam", param);
        }

        
    }

}
