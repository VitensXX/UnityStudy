
Shader "Joker2D/Circle Ray"
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
        _UV_ST("UV ST", vector) = (0,0,1,1.5)
        _LightCol("Ray Color", color) = (1,1,1,1)
        _Factor("Factor", float) = 0

        _R("圆角大小", range(0, 0.25)) = 0.1
        _Aspect("图片长宽比", float) = 1 
        _Fade("圆角羽化", range(0.0001, 0.01)) = 0.001
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

            float4 _UV_ST;
            fixed4 _LightCol;
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

            fixed4 RayEffect(fixed4 tex, half2 uv)
            {
                uv = uv*_UV_ST.zw - _UV_ST.xy;
                half2 adjusted_uv = uv - 0.5; 
                adjusted_uv.x = adjusted_uv.x*_UV_ST.z/_UV_ST.w;

                //圆
                half fac = max
                    (min(2*sin((length(90*adjusted_uv) + _Factor*2) + 3*(1+0.8*cos(length(113.1121*adjusted_uv) - _Factor*3.121))) 
                    - 1 - max(5.-length(90*adjusted_uv), 0), 1), 0);

                half2 rotater = half2(cos(_Factor*0.1221), sin(_Factor*0.3512));

                half angle = dot(normalize(rotater),normalize(adjusted_uv));

                //放射高光
                half fac2 = max(min(5*cos(angle*3.14*(2.2+0.9*sin(_Factor*1.65))) - 4 - max(2-length(20*adjusted_uv), 0), 1), 0);

                //横向高光
                half fac3 = 0.3*max(min(2.*sin(_Factor*5. + uv.x*3. + 3.*(1.+0.5*cos(_Factor*7.))) - 1, 1), -1);
                //竖向高光
                half fac4 = 0.3*max(min(2.*sin(_Factor*6.66 + uv.y*3.8 + 3.*(1.+0.5*cos(_Factor*3.414))) - 1, 1), -1);
                half maxfac = max(max(fac, max(fac2, max(fac3, max(fac4, 0.0)))) + 2.2*(fac+fac2+fac3+fac4), 0);

                fixed4 origin = tex;

                half low = min(tex.r, min(tex.g, tex.b));
                half high = max(tex.r, max(tex.g, tex.b));
                half delta = min(high, max(0.5, 1 - low));

                tex.r = tex.r-delta + delta*maxfac*0.5;
                tex.g = tex.g-delta + delta*maxfac*0.5;
                tex.b = tex.b + delta*maxfac*2;
                // tex.a = min(tex.a, 0.3*tex.a + 0.9*min(1, maxfac*0.1));

                return tex;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                fixed4 ray = fixed4(_LightCol.rgb * _LightCol.a / 5, 1);
                ray = RayEffect(ray, IN.texcoord);
                MASK(ray, IN.texcoord)
                color.rgb += ray.rgb;

                ROUND_CORNER(color)
                

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
