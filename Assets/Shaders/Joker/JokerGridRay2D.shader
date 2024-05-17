
Shader "Joker2D/Grid Ray"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        [HideInInspector][Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Mask("Mask(R)",2d) = "white" {}
        _Speed("Speed", range(0,10)) = 0
        _Factor("Factor", float) = 0
        _GridSize("grid size", range(0,10)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "JokerInclude.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float _Speed;
            float _GridSize;
            float _Factor;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            half4 GridRayEffect(fixed4 tex, half2 uv)
            {
                //rgb转换成hsl
                half4 hsl = HSL(0.5*tex + 0.5*half4(0,0,1,tex.a));
                //镭射效果
                float t = BalatroTime *_Speed;
                half2 floored_uv = floor(uv*1000)/1000;
                half2 uv_scaled_centered = (floored_uv - 0.5) * 250.;
                ///油性
                half2 field_part1 = uv_scaled_centered + 50.*half2(sin(-t / 143.6340), cos(-t / 99.4324));
                half2 field_part2 = uv_scaled_centered + 50.*half2(cos( t / 53.1532),  cos( t / 61.4532));
                half2 field_part3 = uv_scaled_centered + 50.*half2(sin(-t / 87.53218), sin(-t / 49.0000));

                float field = (1.+ (
                                    cos(length(field_part1) / 19.483) 
                                    + sin(length(field_part2) / 33.155) * cos(field_part2.y / 15.73) 
                                    +cos(length(field_part3) / 27.193) * sin(field_part3.x / 21.92) )
                                )/2.;
                float res = (.5 + .5* cos(_Factor * 2.612 + (field -0.5) *3.14));
                
                half low = min(tex.r, min(tex.g, tex.b));
                half high = max(tex.r, max(tex.g, tex.b));
                half delta = 0.2+0.3*(high- low) + 0.1*high;

                //网格图形
                half fac = 0.5*max(max(max(0., 7.*abs(cos(uv.x*_GridSize*20.))-6.),
                                    max(0., 7.*cos(uv.y*_GridSize*45. + uv.x*_GridSize*20.)-6.)), 
                                    max(0., 7.*cos(uv.y*_GridSize*45. - uv.x*_GridSize*20.)-6.));
                //颜色
                hsl.x = hsl.x + res * 2 + fac;
                hsl.y = hsl.y*1.3;	
                hsl.z = hsl.z*0.6+0.4;
                //最终颜色
                tex =(1-delta)*tex + delta*RGB(hsl);
                return saturate(tex);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                // fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 origin = color;
                color = GridRayEffect(color, IN.texcoord);

                fixed4 mask = tex2D(_Mask, IN.texcoord);
                color.rgb = lerp(origin.rgb, color.rgb, mask.r);

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
