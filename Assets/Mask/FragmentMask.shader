Shader "Unlit/FragmentMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskColor("Mask Color", color) = (0,0,0,0.5)
        _Level("Level", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
            fixed4 _MaskColor;
            int _Level;

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
                if(col.r < _Level * 0.1){
                    col.a = 0;
                }
                else{
                    col = _MaskColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
