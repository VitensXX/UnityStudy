using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Transition : MonoBehaviour
{
    public Camera cam;
    public Image target;
    public bool UnscaledTime;
    public bool play;

    const float MaxFactor = 1.42f;

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
        //Canvas 为 worldPosition 模式
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, target.transform.position);

        _mat = GetComponent<RawImage>().material;
        _mat.SetFloat("_CenterX", screenPos.x / Screen.width);
        _mat.SetFloat("_CenterY", screenPos.y / Screen.height);
        Debug.LogError(screenPos);
        _mat.SetFloat("_ScreenWidth", Screen.width);
        _mat.SetFloat("_ScreenHeight", Screen.height);
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
}
