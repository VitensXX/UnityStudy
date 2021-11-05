using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiCircleMask : MonoBehaviour
{
    public Vector2 center1;
    public Vector2 center2;
    public Vector2 center3;
    Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<RawImage>().material;
        // _mat.SetFloatArray("_CenterArr", )
    }

    // Update is called once per frame
    void Update()
    {
        List<float> centerArr = new List<float>();
        centerArr.Add(center1.x);
        centerArr.Add(center1.y);
        centerArr.Add(center2.x);
        centerArr.Add(center2.y);
        centerArr.Add(center3.x);
        centerArr.Add(center3.y);
        _mat.SetFloatArray("_CenterArr", centerArr);
    }
}
