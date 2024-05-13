Shader "Joker/Joker 2"
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

            #define BalatroTime _Time.w * 2
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

            fixed4 RainbowEffect(fixed4 tex, half2 uv){
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
                col = RainbowEffect(col, i.uv);

                fixed4 mask = tex2D(_Mask, i.uv);
                col = lerp(origin, col, mask.r);

                return col;
            }
            ENDCG
        }
    }
}
