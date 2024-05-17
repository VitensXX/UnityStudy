Shader "Joker/Grid Ray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask("Mask(R)",2d) = "white" {}
        _TextureDetails("details", vector) = (0,0,0,0)
        _ImageDetails("uvScale", vector) = (0,0,0,0)
        _RayOffset("rayOffset", vector) = (0,0,0,0)
        _GridSize("grid size", range(0,10)) = 0.5
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

            #define BalatroTime _Time.w * 10
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
            float _GridSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 GridRayEffect(fixed4 tex, half2 uv)
            {
                uv = (uv*(_ImageDetails.xy) - _TextureDetails.xy*_TextureDetails.zw)/_TextureDetails.zw;
                //rgb转换成hsl
                half4 hsl = HSL(0.5*tex + 0.5*half4(0,0,1,tex.a));
                //----镭射效果
                float t = _RayOffset.y*7.221 + BalatroTime;
                half2 floored_uv = (floor((uv*_TextureDetails.zw)))/_TextureDetails.zw;
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
                float res = (.5 + .5* cos( (_RayOffset.x) * 2.612 + ( field -0.5 ) *3.14));
                ///----------------
                half low = min(tex.r, min(tex.g, tex.b));
                half high = max(tex.r, max(tex.g, tex.b));
                half delta = 0.2+0.3*(high- low) + 0.1*high;

                ///----------网格图形
                half fac = 0.5*max(max(max(0., 7.*abs(cos(uv.x*_GridSize*20.))-6.),
                                    max(0., 7.*cos(uv.y*_GridSize*45. + uv.x*_GridSize*20.)-6.)), 
                                    max(0., 7.*cos(uv.y*_GridSize*45. - uv.x*_GridSize*20.)-6.));
                //-----颜色
                hsl.x = hsl.x + res * 2 + fac;
                hsl.y = hsl.y*1.3;	
                hsl.z = hsl.z*0.6+0.4;
                //最终颜色
                tex =(1-delta)*tex + delta*RGB(hsl);
                // if(tex.a<0.7) {tex.a /=3; }
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
                col = GridRayEffect(col, i.uv);

                fixed4 mask = tex2D(_Mask, i.uv);
                col = lerp(origin, col, mask.r);

                return col;
            }
            ENDCG
        }
    }
}
