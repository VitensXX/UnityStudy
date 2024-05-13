Shader "Joker/inverse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // _Factor("Factor", range(0,1)) = 1
        _Mask("Mask(R)",2d) = "white" {}
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
            sampler2D _Mask;
            float4 _Mask_ST;
            // float _Factor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 origin = col;
                col.rgb = 1-col.rgb;

                fixed4 mask = tex2D(_Mask, i.uv);
                col = lerp(origin, col, mask.r);

                return col;
            }
            ENDCG
        }
    }
}
