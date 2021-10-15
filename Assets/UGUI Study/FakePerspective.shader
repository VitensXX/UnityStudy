// 假的透视效果
Shader "Vitens/FakePerspective"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Theta ("Theta", range(0,90)) = 0 
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
                if(i.uv.x < 0.5){
                    
                    float tanTheta = tan(_Theta / 180 * 3.14);
                    float a = i.uv.y * tanTheta;
                    float b = 0.5 - a;
                    if(i.uv.x < a){
                        col.a = 0;
                    }
                    else{
                        float x = (2 * i.uv.x  - 2 * a) / b;
                        col = tex2D(_MainTex, float2(x, i.uv.y));
                    }
                }
                else{
                    col = tex2D(_MainTex, i.uv);
                }
                return col;
            }
            ENDCG
        }
    }
}
