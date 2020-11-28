using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Transition : MonoBehaviour
{
    public Graphic target;
    public bool UnscaledTime;
    public float duration = 1;
    public float feather = 0.02f;

    public bool playIn;
    public bool playOut;

    const float MaxFactor = 1;// 1.42f;
    const float MinFactor = 0;//-.58f;

    Material _mat;
    float _tick;
    bool _playing = false;
    bool _in = false;
    float _speed = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    //播放出场效果 视野从聚焦出变小
    public void PlayOut()
    {
        Prepare();
        _in = false;
        _tick = MaxFactor;
    }

    //播放入场效果 视野从聚焦处变大
    public void PlayIn()
    {
        Prepare();
        _in = true;
        _tick = MinFactor - feather;
    }

    void Prepare()
    {
        if (!_mat)
        {
            _mat = GetComponent<RawImage>().material;
        }

        _speed = duration == 0 ? 1 : 1 / duration;
       
        _playing = true;
        Vector2 screenPos = GetUIScreenPosition(target);
        _mat.SetVector("_CenterAndScreenSize", new Vector4(screenPos.x, screenPos.y, Screen.width, Screen.height));
        _mat.SetFloat("_Feather", feather);
    }

    // Update is called once per frame
    void Update()
    {
        if (playIn)
        {
            PlayIn();
            playIn = false;
        }

        if (playOut)
        {
            PlayOut();
            playOut = false;
        }

        if (_playing)
        {
            if (_in)
            {
                _tick += (UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * _speed;
                if (_tick > MaxFactor)
                {
                    _tick = MaxFactor;
                    _playing = false;
                }
            }
            else
            {
                _tick -= (UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * _speed;
                if (_tick < MinFactor - feather)
                {
                    _tick = MinFactor - feather;
                    _playing = false;
                }
            }

            _mat.SetFloat("_Factor", _tick);
        }
    }

    //获取UI的屏幕坐标【0,1】
    Vector2 GetUIScreenPosition(Graphic ui)
    {
        //获取到UI所处的canvas
        Canvas canvas = ui.GetComponentInParent<Canvas>();

        //Overlay模式 或者 ScreenSpaceCamera模式没有关联UI相机的情况
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
            canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            float x = ui.transform.position.x / Screen.width;
            float y = ui.transform.position.y / Screen.height;
            return new Vector2(x, y);
        }
        //ScreenSpaceCamera 和 WorldSpace模式  注意WorldSpace没有关联UI相机获取到的就会有问题
        else
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, ui.transform.position);
            float x = screenPos.x / Screen.width;
            float y = screenPos.y / Screen.height;
            return new Vector2(x, y);
        }
    }
}
