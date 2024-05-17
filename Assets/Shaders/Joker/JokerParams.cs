using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Joker
{
    /// <summary>
    /// 小丑牌卡面效果参数控制，配合美术K动画需求
    /// </summary>
    [ExecuteInEditMode]
    public class JokerParams : MonoBehaviour
    {
        public float factor;
        public float speed;
        public Texture mask;
        Texture _lastMask;

        Material _mat;
        // Start is called before the first frame update
        void Start()
        {
            Graphic graphic = gameObject.GetComponent<Graphic>();
            if (graphic)
            {
                Material mat = graphic.material;
                _mat = new Material(mat);
#if !UNITY_EDITOR
                Destroy(mat);
#endif
                SetMask(mask);
                graphic.material = _mat;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_mat == null)
            {
                return;
            }

            _mat.SetFloat(ShaderPropertyToID._Factor, factor);
            _mat.SetFloat(ShaderPropertyToID._Speed, speed);

            //遮罩图修改，自动设置
            if (_lastMask != mask)
            {
                SetMask(mask);
            }
        }

        /// <summary>
        /// 设置遮罩
        /// </summary>
        public void SetMask(Texture tex)
        {
            _lastMask = tex;
            mask = tex;
            _mat.SetTexture(ShaderPropertyToID._Mask, mask);
        }
    }
}
