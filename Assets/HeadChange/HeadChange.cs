using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeadChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Range(1, 5)]
    public int rotateCount = 3;//旋转圈数
    [Range(0, 2)]
    public float speed = 5;//速度

    public AnimationCurve timeCurve;

    const float SQUARE_ROOT_OF_2 = 1.4142f;
    const float SQUARE_CIRCLE_CLIP = SQUARE_ROOT_OF_2 / 2;
    const float CIRCLE_CLIP = 0.5f;
    const float OUTLINE = 0.02f;

    Transform _ts;
    Material _mat;
    float _tick = 1;
    float _progress;
    bool _stop = false;
    bool _squareToCircle = false;
    bool _circleToSquare = false;

    // Start is called before the first frame update
    void Start()
    {
        _ts = transform;
        _mat = GetComponent<RawImage>().material;
        _mat.SetFloat("_ClipRange", SQUARE_ROOT_OF_2 / 2);

        _tick = 1 / speed;
        _progress = 0;
        _stop = false;
        _squareToCircle = false;
        _circleToSquare = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop)
        {
            return;
        }

        _tick += Time.deltaTime;
        _progress = timeCurve.Evaluate(_tick * speed);

        //结束标志
        if (_progress >= 1)
        {
            _stop = true;
        }

        if (_squareToCircle)
        {
            //_mat.SetFloat("_ClipRange", Mathf.Lerp(SQUARE_CIRCLE_CLIP, CIRCLE_CLIP, _progress));
            //_mat.SetFloat("_Outline", Mathf.Lerp(0, OUTLINE, _progress));
            _mat.SetFloat("_Width", Mathf.Lerp(0, CIRCLE_CLIP, _progress));
            _ts.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -360 * rotateCount, _progress));
        }
        else if (_circleToSquare)
        {
            //_mat.SetFloat("_ClipRange", Mathf.Lerp(CIRCLE_CLIP, SQUARE_CIRCLE_CLIP, _progress));
            //_mat.SetFloat("_Outline", Mathf.Lerp(OUTLINE, 0, _progress));
            _mat.SetFloat("_Width", Mathf.Lerp(CIRCLE_CLIP, 0, _progress));
            _ts.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, 360 * rotateCount, _progress));
        }
    }

    void Change()
    {
        //重置结束标志
        _stop = false;

        //记录当前进度，以便反向旋转时从当前进度开始反向播放动画
        _progress = 1 - _progress;
        _tick = (1 - _tick * speed) / speed;

        if (!_squareToCircle)
        {
            _squareToCircle = true;
            _circleToSquare = false;
        }
        else
        {
            _circleToSquare = true;
            _squareToCircle = false;
        }
    }

    private void OnDestroy()
    {
        //材质球属性还原
        _mat.SetFloat("_ClipRange", 1);
        _mat.SetFloat("_Outline", 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Change();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Change();
    }
}
