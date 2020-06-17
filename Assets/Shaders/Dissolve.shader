Shader "P03/V2/Dissolve"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "white" {}
		_EdgeColor("Color", color) = (1,1,1,1)
		_Threshold("Threshold", Range(0, 1)) = 1
		_EdgeLength("Edge Length", Range(0, 0.05)) = 0.01
		[Space(10)]
		[Toggle(DIRECTION)] _Use_Direction("受方向影响?", Float) = 0
		[Toggle(DIRECTION_X)] _X("水平方向?", Float) = 0
		_MaxPos("MaxPos", Range(0, 100)) = 40
		_DistanceEffect("Distance Effect", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Cull off
        Pass
        {
			blend srcAlpha oneMinussrcalpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma	 multi_compile __ DIRECTION
			#pragma multi_compile __ DIRECTION_X

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            struct v2f
            {
				float4 pos : POSITION;
                float2 uv : TEXCOORD0;
				float2 uvNoiseTex : TEXCOORD1;
				float4 vertex : TEXCOORD2;
            };

            sampler2D _MainTex;
			half4 _MainTex_ST;

			sampler2D _NoiseTex;
			half4 _NoiseTex_ST;

			half _Threshold;
			half _EdgeLength;
			fixed4 _EdgeColor;
			half _DistanceEffect;
			float _MaxPos;
            v2f vert (appdata v)
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uvNoiseTex = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				o.vertex = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 c = tex2D(_MainTex, i.uv);
				
				//half dist = 1 - i.uv.y;
				//half dist = 1 - (i.vertex.z / 2 + 0.5);
#if DIRECTION
#if DIRECTION_X
				half dist = i.vertex.z / _MaxPos + 0.5 ;
#else
				half dist = i.vertex.x / _MaxPos + 0.5;
#endif
#else
				half dist = 1 - i.uv.y;
#endif

				//clip(dist - 0.5);
				//clip(tex2D(_NoiseTex, i.uvNoiseTex).g - _Threshold);
				//fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).g - _Threshold;
				//c.a *= 1- step(cutout, 0);

				//fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r * (1 - _DistanceEffect) + dist * _DistanceEffect;
				fixed cutout = lerp(tex2D(_NoiseTex, i.uvNoiseTex).r, dist ,_DistanceEffect);
				//fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r;
				c.a *= 1 - step(cutout - _Threshold, 0);
				c.rgb += step(cutout - _Threshold, _EdgeLength) * _EdgeColor.rgb * step(0.01, _Threshold);

				return c;
            }
            ENDCG
        }
    }
}
