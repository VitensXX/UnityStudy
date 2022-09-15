Shader "UI/FlowMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _FlowMap("FlowMap", 2D) = "white" {}
        _FlowSpeed("intensity", float) = 0.1
        _TimeSpeed("speed", float)= 1
        [Toggle]_reverse_flow("reverse", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
                "IgnoreProjector" = "True"
                "RenderType" = "Opaque"
        }
        Cull Off
        Lighting Off
        ZWrite On
        LOD 100
        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _REVERSE_FLOW_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _FlowMap;
            float4 _MainTex_ST;
            float _FlowSpeed;
            float _TimeSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //0~1 *2-1 得到 -1~1 方向
                fixed4 flowDir = tex2D(_FlowMap, i.uv) * 2.0 - 1.0;
                //强度修正
                flowDir *= _FlowSpeed;
                //正负修正
                #ifdef _REVERSE_FLOW_ON
                    flowDir *= -1;
                #endif

                //两个0~1循环 计时
                float phase0 = frac(_Time * 0.1 * _TimeSpeed);
                float phase1 = frac(_Time * 0.1 * _TimeSpeed + 0.5);

                float2 tiling_uv = i.uv * _MainTex_ST.xy +   _MainTex_ST.zw;

                half4 tex0 = tex2D(_MainTex, saturate(tiling_uv + flowDir.xy * phase0));
                half4 tex1 = tex2D(_MainTex, saturate(tiling_uv + flowDir.xy * phase1));

                float flowLerp = abs((0.5 - phase0) / 0.5);
                half3 finalColor = lerp(tex0.rgb, tex1.rgb, flowLerp);
                return float4(finalColor, tex0.a) * _Color;
            }
            ENDCG
        }
    }
}
