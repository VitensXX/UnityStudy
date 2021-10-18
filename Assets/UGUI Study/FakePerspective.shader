// 假的透视效果
Shader "Vitens/FakePerspective"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Theta ("Theta", range(-90,90)) = 0 
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        blend SrcAlpha oneMinusSrcAlpha
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
            float _Theta;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(1,1,1,1);
                float tanTheta = tan(_Theta / 180 * 3.14);
                float a = (1 - i.uv.y) * tanTheta;
                float b = 0.5 - a;
                if(i.uv.x < a || i.uv.x > 1 - a){
                    col.a = 0;
                }
                else{
                    float x = (i.uv.x - a) / (2 * b);
                    col = tex2D(_MainTex, float2(x, i.uv.y));
                }
                return col;
            }
            ENDCG
        }
    }
}
