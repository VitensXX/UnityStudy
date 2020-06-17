Shader "P03/V2/Distortion"
{
    Properties
    {
		[hdr]_Tint("Tint", COLOR) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_DistortionStrength("Distortion",range(0,30)) = 2
		_MoveSpeedX("MoveSpeedX",range(-20,20)) = 2
		_MoveSpeedY("MoveSpeedY",range(-20,20)) = 2
		[Space(10)]
		[Toggle(SECOND)] _Use_SECOND("第二层?", Float) = 0
		_MainTex2("Texture", 2D) = "white" {}
		_DistortionStrength2("Distortion",range(0,30)) = 2
		_MoveSpeedX2("MoveSpeedX",range(-20,20)) = 2
		_MoveSpeedY2("MoveSpeedY",range(-20,20)) = 2

		[Space(20)]
		_DistortionTex("noise",2D) = "white"{}
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma	 multi_compile __ SECOND

            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD2;
				half2 distortionUV : TEXCOORD1;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _MainTex2;
			half4 _MainTex_ST;
			half4 _MainTex2_ST;
			sampler2D _DistortionTex;
			half4 _DistortionTex_ST;
			half _MoveSpeedX;
			half _MoveSpeedY;
			half _MoveSpeedX2;
			half _MoveSpeedY2;
			half _DistortionStrength;
			half _DistortionStrength2;
			fixed4 _Tint;

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _MainTex2);
				o.distortionUV = TRANSFORM_TEX(v.uv, _DistortionTex);
				float2 timer = float2(_Time.x, _Time.x);
				//o.distortionUV += timer * float2(_SpeedX, _SpeedY);
				o.uv += timer * float2(_MoveSpeedX, _MoveSpeedY);
				o.uv2 += timer * float2(_MoveSpeedX2, _MoveSpeedY2);
				o.color = v.color;
                return o;
            }

			inline fixed2 SamplerFromNoise(half2 uv, sampler2D noiseTex, half4 noiseTex_ST, half offsetScaleFactor)
			{
				half2 newUV = uv * noiseTex_ST.xy + noiseTex_ST.zw;
				fixed4 noiseColor = tex2D(noiseTex, newUV);
				//将结果只从 0~1 转到 -1~1,并缩小适配UV的偏移单位 offsetScaleFactor 0.005
				noiseColor = (noiseColor * 2 - 1) * offsetScaleFactor;
				return noiseColor.xy;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 noiseOffset = SamplerFromNoise(i.distortionUV, _DistortionTex, _DistortionTex_ST, 0.005);
				fixed4 finalCol = tex2D(_MainTex, i.uv + noiseOffset * _DistortionStrength)* i.color;

#if SECOND
				finalCol = lerp(finalCol, tex2D(_MainTex2, i.uv2 + noiseOffset * _DistortionStrength2)* i.color, 0.5);
#endif

				finalCol.rgb *= _Tint.rgb ;

                return finalCol;
            }
            ENDCG
        }
    }
}
