using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2020/12/14 22:07:49
/// 
/// Description : 
///     
/// </summary>
public class ComputeShaderExample : MonoBehaviour
{
    public ComputeShader cs;

    void Start()
    {
        ////一个元素的长度（bytes)
        //int stride = sizeof(int);
        //Debug.LogError(stride);
        ////创建一个buffer，大小为16个元素
        //ComputeBuffer outputBuffer = new ComputeBuffer(16 * stride, stride);
        ////找到kernel
        //int kernel = cs.FindKernel("CSMain");
        //cs.SetBuffer(kernel, "outDatas", outputBuffer);
        //cs.Dispatch(kernel, 2, 2, 1);

        //float[] datas = new float[16];
        //print(outputBuffer.stride);
        //outputBuffer.GetData(datas);

        //print(datas.Length);
        //for (int i = 0; i < datas.Length; i++)
        //{
        //    print(datas[i]);
        //}
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Calc();
        }
    }

    void Calc()
    {
        //一个元素的长度（bytes)
        int stride = sizeof(int);
        //Debug.LogError(stride);
        //创建一个buffer，大小为16个元素
        ComputeBuffer outputBuffer = new ComputeBuffer(64 * stride, stride);
        //找到kernel
        int kernel = cs.FindKernel("CSMain");
        cs.SetBuffer(kernel, "outputDatas", outputBuffer);
        cs.Dispatch(kernel, 2, 2, 1);

        float[] datas = new float[64];
        //print(outputBuffer.stride);
        outputBuffer.GetData(datas);
        outputBuffer.Release();
        //print(datas.Length);
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < datas.Length; i++)
        {
            s.Append(datas[i]);
            s.Append(",");
        }
        print(s.ToString());
    }
}
