using UnityEngine;
using System.Collections;


//支持中文.
/*----------------------------------------------------------------
// Copyright (C) 公司名称 成都微美互动科技有限公司
// 版权所有。  
//
// 文件名：PeriodicFunction.cs
// 文件功能描述：周期函数
 
// 创建标识：Created by Vitens On 2019/08/27 09:51:55

// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/


public class PeriodicFunction
{
    const float DEFAULT_WAIT_VALUE = 888;

    public enum Type
    {
        Sawtooth,
        SawtoothPlus,
        Sine,
        SinePlus,
        Linear01,
        Linear10,
    }

    public static float Linear01(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        if (time < 0)
            return 0;

        time = Saturate(time, frequency + waitTime);

        if (time <= frequency)
        {
            return time % 1;
        }
        else
        {
            //使用传入的固定值
            if (waitTimeValue != DEFAULT_WAIT_VALUE)
                return waitTimeValue;

            return 0;
        }
    }

    public static float Linear10(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        if (time < 0)
            return 0;

        time = Saturate(time, frequency + waitTime);

        if (time <= frequency)
        {
            return 1 - time % 1;
        }
        else
        {
            //使用传入的固定值
            if (waitTimeValue != DEFAULT_WAIT_VALUE)
                return waitTimeValue;

            return 0;
        }
    }


    //线性锯齿波
    public static float Sawtooth(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        if(time < 0)
            return 0;

        time = Saturate(time, frequency + waitTime);

        atten = CalcAtten(time, atten);

        if (time <= frequency)
        {
            //一个锯齿周期分为四段计算
            float tempTime = time % 4;

            float v;
            if (tempTime > 0 && tempTime <= 1)
                v = tempTime / atten;
            else if (tempTime > 1 && tempTime <= 3)
                v = 2 - tempTime;
            else
                v = tempTime - 4;

            return v / atten;
        }
        else
        {
            //使用传入的固定值
            if (waitTimeValue != DEFAULT_WAIT_VALUE)
                return waitTimeValue;

            //根据频率算出衔接值
            return GetWaitValueByFrequency(frequency);
        }
    }

    //无负数的线性锯齿波
    public static float SawtoothWithoutNegative(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        return Mathf.Abs(Sawtooth(time, waitTime, frequency, waitTimeValue, atten));
    }

    //三角周期波, 1/4PI为一个频率.
    public static float Sine(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        if (time < 0)
        {
            return 0;
        }

        time = Saturate(time, frequency * (Mathf.PI / 2) + waitTime);

        if (time <= frequency * (Mathf.PI / 2))
        {
            return Mathf.Sin(time) / CalcAtten(time, atten);
        }
        else
        {
            //使用传入的固定值
            if (waitTimeValue != DEFAULT_WAIT_VALUE)
            {
                return waitTimeValue;
            }

            //根据频率算出衔接值
            return GetWaitValueByFrequency(frequency);
        }
    }

    //无负数的正弦波
    public static float SineWithoutNegative(float time, float waitTime, int frequency, float waitTimeValue = DEFAULT_WAIT_VALUE, float atten = 0)
    {
        return Mathf.Abs(Sine(time, waitTime, frequency, waitTimeValue, atten));
    }

    static float Saturate(float time, float T)
    {
        return time % T;
    }

    static float CalcAtten(float time, float atten)
    {
        return  1 + ((int)time / 2) * atten;
    }

    static int GetWaitValueByFrequency(int frequency)
    {
        switch (frequency % 4)
        {
            case 0:
            case 2:
                return 0;
            case 1:
                return 1;
            case 3:
                return -1;
            default:
                return 0;
        }
    }
}
