#ifndef JOKER_INCLUDE  
#define JOKER_INCLUDE

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _Mask;
float4 _Mask_ST;

half hue(half s, half t, half h)
{
    half hs = fmod(h, 1.)*6.;
    if (hs < 1.) return (t-s) * hs + s;
    if (hs < 3.) return t;
    if (hs < 4.) return (t-s) * (4.-hs) + s;
    return s;
}

//HSL转RGB
half4 RGB(half4 c)
{
    if (c.y < 0.0001)
    return half4(c.zzz,c.a);

    half t = (c.z < .5) ? c.y*c.z + c.z : -c.y*c.z + (c.y+c.z);
    half s = 2.0 * c.z - t;
    return half4(hue(s,t,c.x + 1./3.), hue(s,t,c.x), hue(s,t,c.x - 1./3.), c.w);
}

//RGB转HSL
half4 HSL(half4 c)
{
    half low = min(c.r, min(c.g, c.b));
    half high = max(c.r, max(c.g, c.b));
    half delta = high - low;
    half sum = high+low;
    half4 hsl = half4(0, 0, 0.5 * sum, c.a);
    //如果最大值和最小值相等，则色调为0，灰度
    if (delta == 0)
    {
        return hsl;
    }
    //饱和度计算
    hsl.y = (hsl.z < .5) ? delta / sum : delta / (2.0 - sum);
    //计算色相
    if (high == c.r)
    hsl.x = (c.g - c.b) / delta;//在黄色和洋红之间
    else if (high == c.g)
    hsl.x = (c.b - c.r) / delta + 2.0;//在青色和黄色之间
    else
    hsl.x = (c.r - c.g) / delta + 4.0;//在洋红和青色之间
    hsl.x = fmod(hsl.x / 6, 1);//限制范围在0-1
    return hsl;
}

#endif