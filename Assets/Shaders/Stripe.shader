Shader "Vitens/Stripe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width("Width", range(0, 2)) = 0.1
        _Alpha1("Alpha1", range(0,1)) = 1
        _Alpha2("Alpha2", range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            blend SrcAlpha OneMinusSrcAlpha
            cull off
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
                float3 modelPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Width;
            fixed _Alpha1;
            fixed _Alpha2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.modelPos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                texColor.a *= _Alpha1;
				half inputX = i.modelPos.x;
				inputX -= step(inputX,0) * _Width / 2;	//负的部分向左偏移 _Width/2,避免x为0的地方竖条宽是2倍
				inputX = abs(inputX);

				texColor.a *= saturate(1 - step(fmod(inputX, _Width), _Width / 2) + _Alpha2);


                return texColor;
            }
            ENDCG
        }
    }
}
