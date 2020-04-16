
//马赛克
Shader "Vitens/Mosaic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Degree("Degree", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed _Degree;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
				i.uv.x = round(i.uv.x / _Degree) * _Degree;
				i.uv.y = round(i.uv.y / _Degree) * _Degree;
				fixed4 col = tex2D(_MainTex, i.uv);

				if (facing > 0)
					col *= fixed4(1, 0, 0, 1);

				return col;
            }
            ENDCG
        }
    }
}
