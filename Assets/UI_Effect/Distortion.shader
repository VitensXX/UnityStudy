
//扰动
Shader "UI/Distortion"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData]_Color ("Tint", Color) = (1,1,1,1)

        [PerRendererData]_StencilComp ("Stencil Comparison", Float) = 8
        [PerRendererData]_Stencil ("Stencil ID", Float) = 0
        [PerRendererData]_StencilOp ("Stencil Operation", Float) = 0
        [PerRendererData]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [PerRendererData]_StencilReadMask ("Stencil Read Mask", Float) = 255

        [PerRendererData]_ColorMask ("Color Mask", Float) = 15

		_DistortionTex("噪声图",2D) = "white"{}
        _MaskTex("Mask (R)", 2D) = "white"{} 
		_SpeedX("SpeedX",range(-10,10)) = 2
		_SpeedY("SpeedY",range(-10,10)) = 2
		_DistortionStrength("扭曲强度",range(0,100)) = 2

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
                half2 distortionUV : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            sampler2D _DistortionTex;
            half4 _DistortionTex_ST;
            half _SpeedX;
            half _SpeedY;
            half _DistortionStrength;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.distortionUV = TRANSFORM_TEX(v.texcoord, _DistortionTex);
                float timeAdd = fmod(_Time.x, 1);
                float2 timer = float2(timeAdd, timeAdd);
                OUT.distortionUV += timer * float2(_SpeedX,_SpeedY);

                OUT.color = v.color * _Color;
                return OUT;
            }

            inline float2 SamplerFromNoise(half2 uv, sampler2D noiseTex, half4 noiseTex_ST, half offsetScaleFactor)
            {
                float2 newUV = uv * noiseTex_ST.xy + noiseTex_ST.zw;
                float4 noiseColor = tex2D(noiseTex, newUV);
                //将结果只从 0~1 转到 -1~1,并缩小适配UV的偏移单位 offsetScaleFactor 0.005
                noiseColor = (noiseColor * 2 - 1) * offsetScaleFactor;
                return noiseColor.xy;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 noiseOffset = SamplerFromNoise(IN.distortionUV, _DistortionTex, _DistortionTex_ST, 0.005);
                
                fixed4 maskCol = tex2D(_MaskTex, IN.texcoord);
                half4 color = (tex2D(_MainTex, IN.texcoord + noiseOffset * _DistortionStrength * maskCol.a * maskCol.r) + _TextureSampleAdd) * IN.color;

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
