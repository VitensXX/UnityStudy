#define BalatroTime _Time.y*0.01
//////效果1--------------镭射卡----------------
//////就叫他镭射卡吧imageDetails uv缩放比例 textureDetails 调整镭射圈圈的位置大小等 rayOffset材质交互的主要参数，动动就知道了
half4 RayEffect( Texture2D coLTexture,SamplerState SSS, half2 texture_coords,
half2 imageDetails,half4 textureDetails,half2 rayOffset )
{
    half4 tex = SAMPLE_TEXTURE2D(coLTexture,SSS,texture_coords);
    half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.zw)/textureDetails.zw;
    half2 adjusted_uv = uv - 0.5;
    adjusted_uv.x = adjusted_uv.x*textureDetails.z/textureDetails.w;
    //拉个curve
    half low = min(tex.r, min(tex.g, tex.b));
    half high = max(tex.r, max(tex.g, tex.b));
    half delta = min(high, max(0.5, 1. - low));

    half fac = max
    (min
    (
    2*sin((length(90*adjusted_uv) + rayOffset.x*2) + 3*(1.+0.8*cos(length(113.1121*adjusted_uv) - rayOffset.x*3.121))) 
    - 1 
    - max(5.-length(90*adjusted_uv), 0), 1
    )
    , 0);
    half2 rotater = half2(cos(rayOffset.x*0.1221), sin(rayOffset.x*0.3512));
    // half angle = dot(rotater, adjusted_uv)/(length(rotater)*length(adjusted_uv));
    half angle = dot(normalize(rotater),normalize(adjusted_uv));
    ///////////放射高光
    half fac2 = max(
    min(5*cos(rayOffset.y*0.3 + angle*3.14*(2.2+0.9*sin(rayOffset.x*1.65 + 0.2*rayOffset.y))) 
    - 4
    - max(2-length(20*adjusted_uv), 0), 1)
    , 0);
    ///////////横向高光
    half fac3 = 0.3*max(min(2.*sin(rayOffset.x*5. + uv.x*3. + 3.*(1.+0.5*cos(rayOffset.x*7.))) - 1, 1), -1);
    ///////////竖向高光
    half fac4 = 0.3*max(min(2.*sin(rayOffset.x*6.66 + uv.y*3.8 + 3.*(1.+0.5*cos(rayOffset.x*3.414))) - 1, 1), -1);
    half maxfac = max(max(fac, max(fac2, max(fac3, max(fac4, 0.0)))) + 2.2*(fac+fac2+fac3+fac4), 0);
    tex.r = tex.r-delta + delta*maxfac*0.5;
    tex.g = tex.g-delta + delta*maxfac*0.5;
    tex.b = tex.b + delta*maxfac*1.9;
    tex.a = min(tex.a, 0.3*tex.a + 0.9*min(1, maxfac*0.1));
    return saturate(tex);
}
//////效果2---------------彩虹卡---------------
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
half4 RainbowEffect( Texture2D colTexture,SamplerState SSS, half2 texture_coords,
half2 imageDetails,half4 textureDetails,half2 rayOffset)
{
    half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
    half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.ba)/textureDetails.ba;
    ///////提取亮度范围信息，用做后面的高光Mask
    half low = min(tex.r, min(tex.g, tex.b));
    half high = max(tex.r, max(tex.g, tex.b));
    half delta = high - low;
    half saturation_fac = 1 - max(0, 0.05*(1.1-delta));
    //////转化为hsl
    half4 hsl = HSL(half4(tex.r*saturation_fac, tex.g*saturation_fac, tex.b, tex.a));
    float t = rayOffset.y*2.221 + BalatroTime;
    ///////像素化
    //half2 floored_uv = (floor((uv*textureDetails.ba)))/textureDetails.ba;
    ///////
    half2 floored_uv = uv;
    half2 uv_scaled_centered = (floored_uv - 0.5) * 50.;
    ///--------------------------------------
    half2 field_part1 = uv_scaled_centered + 50.*half2(sin(-t / 143.6340), cos(-t / 99.4324));
    half2 field_part2 = uv_scaled_centered + 50.*half2(cos( t / 53.1532),  cos(t / 61.4532));
    half2 field_part3 = uv_scaled_centered + 50.*half2(sin(-t / 87.53218), sin(-t / 49.0000));
    float field = (1+ (
    cos(length(field_part1) / 19.483) + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) +
    cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) ))/2;
    float res = (0.5 + 0.5* cos( (rayOffset.x) * 2.612 + ( field + -0.5 ) *3.14));
    hsl.x = hsl.x+ res + rayOffset.y*0.04;
    hsl.y = min(0.6,hsl.y+0.5);
    tex.rgb = RGB(hsl).rgb;
    //类似增加对比度，暗（透明）的地方会更暗（透明）
    if(tex.a<0.7) tex.a = tex.a/3;
    ///--------------------------------------
    return saturate(tex);
}
//////效果3--------------反相黑暗卡--------------
half4 SimpleRayEffect(Texture2D colTexture,SamplerState SSS, half2 texture_coords,
half2 imageDetails ,half4 textureDetails,half offsetSpeed,half4 rayColor)
{
    half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
    half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.zw)/textureDetails.zw;

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
//效果3颜色翻转，额，这个结果偏向红色，negative看作开关，0和非0的差别,哈哈，我还是比较喜欢直接反相一下然后调效果，好像也不错，欢迎击剑
half4 NegativeColor(Texture2D colTexture,SamplerState SSS, half2 texture_coords ,half negative,half4 color)
{
    half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
    half4 SAT = HSL(tex);
    if (negative > 0.0 || negative < 0.0) {
        SAT.b = (1-SAT.b);
    }
    //结果偏红色
    SAT.r = 0.2-SAT.r;
    //特定颜色，可修改
    tex = RGB(SAT) + 0.8*half4(0.31,0.39,0.41,0.);
    // tex = RGB(SAT)+0.8*color;
    if(tex.a<0.7) tex.a= tex.a/3;
    return tex;
}
//////效果4-------------网格镭射卡-----------------
half4 RayEffect2(Texture2D colTexture,SamplerState SSS, half2 texture_coords ,
                    half2 imageDetails,half4 textureDetails,half2 rayOffset,half gridSize,half4 color)
{
    half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
    half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.zw)/textureDetails.zw;
    //rgb转换成hsl
    half4 hsl = HSL(0.5*tex + 0.5*half4(0,0,1,tex.a));
    //----镭射效果
    float t = rayOffset.y*7.221 + BalatroTime;
    half2 floored_uv = (floor((uv*textureDetails.zw)))/textureDetails.zw;
    half2 uv_scaled_centered = (floored_uv - 0.5) * 250.;
    ///--油性
    half2 field_part1 = uv_scaled_centered + 50.*half2(sin(-t / 143.6340), cos(-t / 99.4324));
    half2 field_part2 = uv_scaled_centered + 50.*half2(cos( t / 53.1532),  cos( t / 61.4532));
    half2 field_part3 = uv_scaled_centered + 50.*half2(sin(-t / 87.53218), sin(-t / 49.0000));

    float field = (1.+ (
                        cos(length(field_part1) / 19.483) 
                        + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) 
                        +cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) )
                    )/2.;
    float res = (.5 + .5* cos( (rayOffset.x) * 2.612 + ( field -0.5 ) *3.14));
    ///----------------
    half low = min(tex.r, min(tex.g, tex.b));
    half high = max(tex.r, max(tex.g, tex.b));
    half delta = 0.2+0.3*(high- low) + 0.1*high;

    ///----------网格图形 ,可以替换成自己想要的图形
    half fac = 0.5*max(max(max(0., 7.*abs(cos(uv.x*gridSize*20.))-6.),
                        max(0., 7.*cos(uv.y*gridSize*45. + uv.x*gridSize*20.)-6.)), 
                        max(0., 7.*cos(uv.y*gridSize*45. - uv.x*gridSize*20.)-6.));
    //-----颜色
    hsl.x = hsl.x + res + fac;
    hsl.y = hsl.y*1.3;	
    hsl.z = hsl.z*0.6+0.4;
    //最终颜色
    tex =(1-delta)*tex + delta*RGB(hsl)*color;
    if(tex.a<0.7) {tex.a /=3; }
    return saturate(tex);
}
