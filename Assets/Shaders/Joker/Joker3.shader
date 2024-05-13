Shader "Joker/Joker 3"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask("Mask(R)",2d) = "white" {}
        _TextureDetails("details", vector) = (0,0,0,0)
        _ImageDetails("uvScale", vector) = (0,0,0,0)
        _RayOffset("rayOffset", vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "JokerInclude.cginc"
            #define BalatroTime _Time.w*10
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            // sampler2D _Mask;
            // float4 _Mask_ST;

            float4 _TextureDetails;
            float2 _RayOffset;
            float2 _ImageDetails;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 RayEffect(fixed4 tex, half2 uv){
                uv = (uv*(_ImageDetails.xy) - _TextureDetails.xy*_TextureDetails.zw)/_TextureDetails.zw;

                //提取亮度范围信息，用做后面的高光Mask
                half low = min(tex.r, min(tex.g, tex.b));
                half high = max(tex.r, max(tex.g, tex.b));
                half delta = high - low;
                half saturation_fac = 1 - max(0, 0.05*(1.1-delta));

                //转化为hsl
                half4 hsl = HSL(half4(tex.r*saturation_fac, tex.g*saturation_fac, tex.b, tex.a));
                float t = _RayOffset.y*2.221 + BalatroTime;

                half2 floored_uv = uv;
                half2 uv_scaled_centered = (floored_uv - 0.5) * 50.;

                half2 field_part1 = uv_scaled_centered + 50.*half2(sin(-t / 143.6340), cos(-t / 99.4324));
                half2 field_part2 = uv_scaled_centered + 50.*half2(cos( t / 53.1532),  cos(t / 61.4532));
                half2 field_part3 = uv_scaled_centered + 50.*half2(sin(-t / 87.53218), sin(-t / 49.0000));
                float field = (1+ (
                    cos(length(field_part1) / 19.483) + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) +
                    cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) ))/2;
                    float res = (0.5 + 0.5* cos( (_RayOffset.x) * 2.612 + ( field + -0.5 ) *3.14));
                    hsl.x = hsl.x+ res + _RayOffset.y*0.04;
                    hsl.y = min(0.6,hsl.y+0.5);
                tex.rgb = RGB(hsl).rgb;

                // if(tex.a<0.7) tex.a = tex.a/3;


                return saturate(tex);
            }

            // half4 SimpleRayEffect(half2 texture_coords,
            //     half2 imageDetails ,half4 textureDetails,half offsetSpeed,half4 rayColor)
            //     {
            //         half4 tex = SAMPLE_TEXTURE2D(colTexture,SSS,texture_coords);
            //         half2 uv = (((texture_coords)*(imageDetails.xy)) - textureDetails.xy*textureDetails.zw)/textureDetails.zw;
                
            //         half low = min(tex.r, min(tex.g, tex.b));
            //         half high = max(tex.r, max(tex.g, tex.b));
            //         half delta = high-low -0.1;
            //         //////------纹理生成,调调参数很不一样
            //         half fac = 0.8 + 0.9*sin(11*uv.x+4.32*uv.y + offsetSpeed*12*BalatroTime+ cos(offsetSpeed*5.3*BalatroTime + uv.y*4.2 - uv.x*4));
            //         half fac2 = 0.5 + 0.5*sin(8.*uv.x+2.32*uv.y + offsetSpeed*5*BalatroTime - cos(offsetSpeed*2.3*BalatroTime + uv.x*8.2));
            //         half fac3 = 0.5 + 0.5*sin(10.*uv.x+5.32*uv.y + offsetSpeed*6.111*BalatroTime + sin(offsetSpeed*5.3*BalatroTime + uv.y*3.2));
            //         half fac4 = 0.5 + 0.5*sin(3.*uv.x+2.32*uv.y + offsetSpeed*8.111*BalatroTime + sin(offsetSpeed*1.3*BalatroTime + uv.y*11.2));
            //         half fac5 = sin(0.9*16.*uv.x+5.32*uv.y + offsetSpeed*12*BalatroTime + cos(offsetSpeed*5.3*BalatroTime + uv.y*4.2 - uv.x*4));
            //         half maxfac = 0.7*max(max(fac, max(fac2, max(fac3,0.0))) + (fac+fac2+fac3*fac4), 0);
            //         ///////颜色校正
            //         // tex.rgb = tex.rgb*0.5 + half3(0.4, 0.4, 0.8);
            //         tex.rgb = tex.rgb*0.5 +rayColor;
                
            //         ///////调整分布
            //         tex.r = tex.r-delta + delta*maxfac*(0.7 + fac5*0.27) - 0.1;
            //         tex.g = tex.g-delta + delta*maxfac*(0.7 ) - 0.1;
            //         tex.b = tex.b-delta + delta*maxfac*(0.7- fac5*0.27) - 0.1;
            //         tex.a = tex.a*rayColor.a*(0.5*max(min(1., max(0,0.3*max(low*0.2, delta)+ min(max(maxfac*0.1,0.), 0.4)) ), 0) + 0.15*maxfac*(0.1+delta));
                
            //         return saturate(tex);
            //     }

            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // fixed4 test = fixed4( _LightCol.rgb * _Light ,1);
                // test = RainbowEffect(test, i.uv);
                
                // fixed4 mask = tex2D(_Mask, i.uv);
                // test *= mask.r;

                // col.rgb += test.rgb;
                // return col;


                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 origin = col;
                col.rgb = 1- col.rgb;
                col = RayEffect(col, i.uv);

                fixed4 mask = tex2D(_Mask, i.uv);
                col = lerp(origin, col, mask.r);

                return col;
            }
            ENDCG
        }
    }
}
