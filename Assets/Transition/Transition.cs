﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Transition : MonoBehaviour
{
    //public Camera cam;
    public Graphic target;
    public bool UnscaledTime;

    public bool play;
    public bool hide;

    const float MaxFactor = 1.42f;
    const float MinFactor = -.58f;

    Material _mat;
    float _tick;
    bool _playing = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Play()
    {
        play = true;
        _tick = 0;
        _playing = true;
        Vector2 screenPos = GetUIScreenPosition(target);
        _mat = GetComponent<RawImage>().material;
        _mat.SetVector("_CenterAndScreenSize", new Vector4(screenPos.x, screenPos.y, Screen.width, Screen.height));
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            Play();
            play = false;
        }

        if (_playing)
        {
            _tick += UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_tick > MaxFactor)
            {
                _tick = MaxFactor;
                _playing = false;
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
