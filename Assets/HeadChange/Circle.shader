Shader "vitens/circle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ClipRange("Clip Range", Range(0, 0.7071)) = 0.5
        _Outline("Outline", Range(0, 0.2)) = 0
        _OutlineColor("Outline color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ClipRange;
            float _Outline;
            fixed4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                //裁剪 step(a,x)  x >= a  return 1,  x < a return 0
                col.a = step(distance(i.uv, float2(0.5, 0.5)), _ClipRange);

                //if(distance(i.uv, float2(0.5, 0.5)) > _ClipRange)
                //{
                //    discard;
                //}

                //添加Outline
                if(distance(i.uv, float2(0.5, 0.5)) > _ClipRange - _Outline)
                {
                    col.rgb = _OutlineColor.rgb;
                }

                return col;
            }
            ENDCG
        }
    }
}
