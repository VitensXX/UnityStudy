using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2021/11/17 21:06:21
/// 
/// Description : 
///     自动旋转展示模型用
/// </summary>
public class AutoRotate : MonoBehaviour
{
    public float speed = 30;

    Transform _ts;
    void Start()
    {
        _ts = transform;
    }

    void Update()
    {
        Vector3 curRotate = _ts.localEulerAngles;
        curRotate.y -= speed * Time.deltaTime;
        _ts.localEulerAngles = curRotate;
    }
}
