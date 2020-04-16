
Shader "Battle/Scene"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Tint("Tint", color) = (1,1,1,1)
		_Diffuse("Diffuse", color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "BattleType" = "Scene" "LightMode" = "ForwardBase"}

		Pass
		{
			blend srcAlpha oneMinussrcalpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_fwdbase

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Diffuse;
			fixed4 _Tint;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//漫反射
				float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lambert = max(0, dot(worldNormal, worldLightDir));
				o.color = fixed4(lambert * _Diffuse.rgb * _LightColor0.rgb + fixed3(1,1,1), 1);
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Tint * i.color;

				return col;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
