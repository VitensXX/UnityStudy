using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Image image;
    Material mat;
    float topV;
    float bottomV;
    RawImage r;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        Sprite sp = image.sprite;
        Vector2[] uv = sp.uv;
        float topV = uv[0].y;
        float bottomV = uv[3].y;
        Material mat = image.material;
        mat.SetFloat("_V_Max", topV);
        mat.SetFloat("_V_Min", bottomV);
    }

    // Update is called once per frame
    void Update()
    {
        // image
    }
}
