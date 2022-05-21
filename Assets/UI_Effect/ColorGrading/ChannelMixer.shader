Shader "Unlit/ChannelMixer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Red("red", vector) = (1,0,0,0)
        _Green("green", vector) = (0,1,0,0)
        _Blue("blue", vector) = (0,0,1,0)
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Red,_Green,_Blue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 colCopy = col;
                col.r = saturate(dot(colCopy.rgb, _Red.xyz));
                col.g = saturate(dot(colCopy.rgb, _Green.xyz));
                col.b = saturate(dot(colCopy.rgb, _Blue.xyz));

                return col;
            }
            ENDCG
        }
    }
}
