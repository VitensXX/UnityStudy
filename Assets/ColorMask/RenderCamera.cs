using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCamera : MonoBehaviour
{
    public Camera renderCam;
    public RawImage rawImg;
    // Start is called before the first frame update
    void Start()
    {
        RenderTexture rt = RenderTexture.GetTemporary(400, 400);
        renderCam.targetTexture = rt;
        rawImg.texture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
