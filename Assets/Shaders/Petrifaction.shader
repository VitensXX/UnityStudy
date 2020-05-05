//Created by Vitens

//石化效果
Shader "Vitens/Petrifaction" 
{
	Properties{
		 _MainTex("Main Tex", 2D) = "white" {}
		 _StatueTex("Statue Tex", 2D) = "white" {}
		 _StatueDegree("石化程度", range(0, 1)) = 0.5
		 _Brightness("亮度", range(0.5, 3)) = 1
		 _Tint("Tint", color) = (1,1,1,1)
	}

	SubShader
	{

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			blend srcAlpha oneMinussrcalpha

			Cull off
			ZWrite on
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _StatueTex;
			float4 _StatueTex_ST;

			fixed4 _Tint;
			float _StatueDegree;
			float _Brightness;

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			v2f vert(a2v v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.texcoord, _StatueTex);

				return o;
			}

			float4 frag(v2f i) : SV_Target {

				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 colStatue = tex2D(_StatueTex, i.uv2);
				float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
				col.rgb = lerp(col.rgb, float3(grey, grey, grey), _StatueDegree);
				col.rgb *= lerp(fixed3(1,1,1), colStatue.rgb, _StatueDegree) * lerp(1,_Brightness, _StatueDegree);

				return col * _Tint;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
