using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitens.UI
{
    [ExecuteInEditMode]
    //软裁剪父控件
    public class SoftMask : MonoBehaviour, IMeshModifier
    {
        [Range(0, 100)]
        public float softRange = 10;

        //两个anchor确定裁剪窗口的位置和大小
        Transform _leftBottomAnchor;
        Transform _rightTopAnchor;
        List<SoftMaskable> _softMaskables;


        void Start()
        {
            CreatRectAnchor();
            Graphic[] graphics = GetComponentsInChildren<Graphic>();
            _softMaskables = new List<SoftMaskable>();
            for (int i = 0; i < graphics.Length; i++)
            {
                SoftMaskable softMaskable = graphics[i].gameObject.AddComponent<SoftMaskable>();
                softMaskable.hideFlags = HideFlags.DontSave;
                _softMaskables.Add(softMaskable);
            }
        }

        void Update()
        {
            Vector3 lbWorldPos = _leftBottomAnchor.position;
            Vector3 rtWorldPos = _rightTopAnchor.position;
            Vector4 maskRect = new Vector4(
                MappingPos(lbWorldPos.x, Screen.width), 
                MappingPos(lbWorldPos.y, Screen.height), 
                MappingPos(rtWorldPos.x, Screen.width), 
                MappingPos(rtWorldPos.y, Screen.height));
            Vector4 maskParam = new Vector4(Screen.width, Screen.height, softRange, softRange);
            for (int i = 0; i < _softMaskables.Count; i++)
            {
                _softMaskables[i].SetSoftParam(maskRect, maskParam);
            }
        }

        // 映射世界坐标
        float MappingPos(float x, float len){
            return x - len / 2;
        }

        //创建左下和右上锚点
        void CreatRectAnchor(){
            if(!_leftBottomAnchor){
                _leftBottomAnchor = new GameObject("LeftBottomAnchor", typeof(RectTransform)).transform;
                _leftBottomAnchor.hideFlags = HideFlags.DontSave;
                _leftBottomAnchor.SetParent(transform);
                RectTransform rectLB = _leftBottomAnchor as RectTransform;
                rectLB.anchorMin = Vector2.zero;
                rectLB.anchorMax = Vector2.zero;
                rectLB.anchoredPosition3D = Vector3.zero;
            }

            if(!_rightTopAnchor){
                _rightTopAnchor = new GameObject("RightTopAnchor", typeof(RectTransform)).transform as RectTransform;
                _rightTopAnchor.hideFlags = HideFlags.DontSave;
                _rightTopAnchor.SetParent(transform);
                RectTransform rectRT = _rightTopAnchor as RectTransform;
                rectRT.anchorMin = Vector2.one;
                rectRT.anchorMax = Vector2.one;
                rectRT.anchoredPosition3D = Vector3.zero;
            }
        }

        /// <summary>
		/// Call used to modify mesh.
		/// </summary>
		void IMeshModifier.ModifyMesh(Mesh mesh)
		{
            Debug.LogError("!!!!!!!!!!!!ModifyMesh");
		}

		/// <summary>
		/// Call used to modify mesh.
		/// </summary>
		void IMeshModifier.ModifyMesh(VertexHelper verts)
		{
            Debug.LogError("!!!!!!!!!!!!ModifyMesh2");

		}
    }
}


