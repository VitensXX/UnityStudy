#ifndef JOKER_INCLUDE  
#define JOKER_INCLUDE

#define BalatroTime _Time.w * 10

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _Mask;
float4 _Mask_ST;
float _Saturation;
float _MaskVal;

half hue(half s, half t, half h)
{
    half hs = fmod(h, 1.)*6;
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

#define MASK(col, uv)\
    fixed4 mask = tex2D(_Mask, uv);\
    col *= mask.r;


fixed4 RainbowEffect(fixed4 tex, half2 uv, float4 uvST, float factor, float speed)
{
    uv = uv*uvST.zw - uvST.xy;

    //提取亮度范围信息，用做后面的高光Mask
    half low = min(tex.r, min(tex.g, tex.b));
    half high = max(tex.r, max(tex.g, tex.b));
    half delta = high - low;
    half saturation_fac = 1 - max(0, 0.05*(1.1-delta));

    //转化为hsl
    half4 hsl = HSL(half4(tex.r*saturation_fac, tex.g*saturation_fac, tex.b, tex.a));
    float t = factor + BalatroTime * speed;

    half2 floored_uv = uv;
    half2 uv_scaled_centered = (floored_uv - 0.5) * 50.;

    half2 field_part1 = uv_scaled_centered + 50.*half2(sin(-t / 143.6340), cos(-t / 99.4324));
    half2 field_part2 = uv_scaled_centered + 50.*half2(cos( t / 53.1532),  cos(t / 61.4532));
    half2 field_part3 = uv_scaled_centered + 50.*half2(sin(-t / 87.53218), sin(-t / 49.0000));
    float field = (1+ (
        cos(length(field_part1) / 19.483) + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) +
        cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) ))/2;
    float res = (0.5 + 0.5* cos(factor * 2.612 + ( field + -0.5 ) *3.14));
    hsl.x += res;
    // hsl.y = min(0.6,hsl.y+0.5);
    hsl.y *= _Saturation;
    tex.rgb = RGB(hsl).rgb;
    
    return saturate(tex);
}

half4 SimpleRayEffect(fixed4 tex, half2 uv,
    half2 imageDetails ,half4 textureDetails,half offsetSpeed,half4 rayColor)
    {
        half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
        //half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.zw)/textureDetails.zw;
        uv = uv*uvST.zw - uvST.xy;
    
        half low = min(tex.r, min(tex.g, tex.b));
        half high = max(tex.r, max(tex.g, tex.b));
        half delta = high-low -0.1;
        //////------纹理生成,调调参数很不一样
        half fac = 0.8 + 0.9*sin(11*uv.x+4.32*uv.y + offsetSpeed*12*BalatroTime+ cos(offsetSpeed*5.3*BalatroTime + uv.y*4.2 - uv.x*4));
        half fac2 = 0.5 + 0.5*sin(8.*uv.x+2.32*uv.y + offsetSpeed*5*BalatroTime - cos(offsetSpeed*2.3*BalatroTime + uv.x*8.2));
        half fac3 = 0.5 + 0.5*sin(10.*uv.x+5.32*uv.y + offsetSpeed*6.111*BalatroTime + sin(offsetSpeed*5.3*BalatroTime + uv.y*3.2));
        half fac4 = 0.5 + 0.5*sin(3.*uv.x+2.32*uv.y + offsetSpeed*8.111*BalatroTime + sin(offsetSpeed*1.3*BalatroTime + uv.y*11.2));
        half fac5 = sin(0.9*16.*uv.x+5.32*uv.y + offsetSpeed*12*BalatroTime + cos(offsetSpeed*5.3*BalatroTime + uv.y*4.2 - uv.x*4));
        half maxfac = 0.7*max(max(fac, max(fac2, max(fac3,0.0))) + (fac+fac2+fac3*fac4), 0);
        ///////颜色校正
        // tex.rgb = tex.rgb*0.5 + half3(0.4, 0.4, 0.8);
        tex.rgb = tex.rgb*0.5 +rayColor;
    
        ///////调整分布
        tex.r = tex.r-delta + delta*maxfac*(0.7 + fac5*0.27) - 0.1;
        tex.g = tex.g-delta + delta*maxfac*(0.7 ) - 0.1;
        tex.b = tex.b-delta + delta*maxfac*(0.7- fac5*0.27) - 0.1;
        tex.a = tex.a*rayColor.a*(0.5*max(min(1., max(0,0.3*max(low*0.2, delta)+ min(max(maxfac*0.1,0.), 0.4)) ), 0) + 0.15*maxfac*(0.1+delta));
    
        return saturate(tex);
    }

//圆角部分
float _R;
float _Aspect;
float _Fade;

fixed CalcAlpha(half2 uv){
    //计算出阈值 uv的xy在 +-threshold范围内为显示，剩下四个角需要根据圆来判断
    half threshold = 0.5h - _R;
    //四个角的范围内
    half l = length(abs(uv) - half2(threshold, threshold));
    half stepL = step(_R, l);
    
    //除了角的其他部分
    half stepX = step(threshold, abs(uv.x));
    half stepY = step(threshold, abs(uv.y));

    //三个step只要有一个为0，则alpha就为1，所以需要裁减的部分三个step需要都为1
    return 1 - stepX * stepY * stepL * abs(((l - _R) / _Fade));
}

#define ROUND_CORNER(col) \
    half2 uv = IN.texcoord.xy - half2(0.5h, 0.5h);\
    half aspectOfR = _R / _Aspect;\
    if(uv.x > (0.5 - aspectOfR) ){\
        uv.x = lerp(0.5 - _R, 0.5, (uv.x - (0.5 - aspectOfR))/aspectOfR);\
        color.a = CalcAlpha(uv);\
    }\
    else if(uv.x < (-0.5 + aspectOfR)){\
        uv.x = lerp(-0.5 + _R, -0.5, abs((uv.x - (-0.5 + aspectOfR))/aspectOfR));\
        color.a = CalcAlpha(uv);\
    }\
    else{\
        color.a = 1;\
    }\

#endif