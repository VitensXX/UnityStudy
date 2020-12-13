using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TestComputeShader : MonoBehaviour
{
    public ComputeShader cs;
    public ComputeShader cs2;
    public RawImage img;
    public Texture2D tex;
    public Vector2Int size = new Vector2Int(1024, 1024);
    Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        Stopwatch watch = Stopwatch.StartNew();
        watch.Restart();
        //Texture_GPU
        //Texture tex = new Texture();
        colors = tex.GetPixels();
        watch.Stop();
        print("GetPixels:" + watch.ElapsedMilliseconds);
        watch.Restart();

        ToCumputeShader();

        Texture2D tempTex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                //Color col = colors[y * size.x + x];
                //float gray = col.r * 0.299f + col.g * 0.587f + col.b * 0.114f;
                //tempTex.SetPixel(x, y, new Color(gray, gray, gray));

                tempTex.SetPixel(x, y, colors[y * size.x + x]);
            }
        }

        watch.Stop();
        print("SetPixels:" + watch.ElapsedMilliseconds);
        watch.Restart();
        tempTex.Apply();
        watch.Stop();
        print("Apply:" + watch.ElapsedMilliseconds);
        img.texture = tempTex;
    }

    //void Start()
    //{
    //    ToCS2();

    //    //img.texture = tex;
    //}

    // Update is called once per frame
    void Update()
    {

    }

    void ToCumputeShader()
    {
        ComputeBuffer inBuffer = new ComputeBuffer(size.x * size.y, 16);
        ComputeBuffer outBuffer = new ComputeBuffer(size.x * size.y, 16);
        int kernel = cs.FindKernel("CSMain");
        inBuffer.SetData(colors);

        cs.SetBuffer(kernel, "inDatas", inBuffer);
        cs.SetBuffer(kernel, "outDatas", outBuffer);
        cs.Dispatch(kernel, size.x/ 8, size.y / 8, 1);

        outBuffer.GetData(colors);
    }

    void ToCS2()
    {
        RenderTexture rt = new RenderTexture(256,256,24);
        rt.enableRandomWrite = true;
        rt.Create();

        //Texture2D tex = new Texture2D()
        int kernel = cs.FindKernel("CSMain");
        cs2.SetTexture(kernel, "Result", rt);
        //cs2.Dispatch(kernel, 256 / 8, 256/8, 1);
        cs2.Dispatch(kernel, 256 / 8, 256 / 8, 1);
        img.texture = rt;
    }
}
