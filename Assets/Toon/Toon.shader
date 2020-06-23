Shader "P03/V2/Toon"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RimColor("rim color", color) = (1,1,1,1)
		_RimPower("rom power", range(8 , 64)) = 8
		_Threshold("Threshold", Range(0, 1)) = 1
		_SpecularStrength("_SpecularStrength", range(0,1)) = 0.2
		_SpecularRamp("_SpecularRamp", range(0,1)) = 0.2
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

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
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 worldNormal : TEXCOORD1;
					float3 worldPos : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _RimColor;
				float _RimPower;
				float _Threshold;
				float _SpecularStrength;
				float _SpecularRamp;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);

					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
					fixed3 worldHalfDir = normalize(worldLightDir + worldViewDir);


					//half powValue = pow(1.0 - abs(dot(worldViewDir, worldNormal)), _RimPower);
					/*half powValue = abs(dot(worldViewDir, worldNormal));
					if (powValue < _Threshold) {
						col = _RimColor;
					}
					else {

					}*/
					//col.rgb += _RimColor.rgb * powValue;

					half NDotL = dot(worldViewDir, worldLightDir);
					half v = smoothstep(0.5, 0.7, NDotL);
					v = lerp(0.9, 1, v);
					col *= v;


					//二值化高光
					half NdotH = saturate(dot(worldNormal, worldHalfDir));
					v = pow(NdotH, _RimPower); // 缩小夹角系数的值，由于NdotH在0-1，所以pow后会变得更小，_Smoothness参考值为8-64
					v = step(_SpecularRamp, v); // 小于_SpecularRamp的值将为0，反之为1
					v = v * _SpecularStrength; // 定义高光强度，参考值为0.2
					col += col * v; // 在原有颜色的基础上叠加

					return col;
				}
				ENDCG
			}
		}
}
