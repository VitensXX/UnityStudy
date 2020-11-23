
Shader "Vitens/Transition Dir"
{
	Properties
	{
		_Color("Color", color) = (0,0,0,1)
		_Factor("Factor", range(-1,1.42)) = 0
		_Feather("Feather", range(0,0.3)) = 0
		_Angle("Angle", range(0, 360)) = 0
		//_MainTex("Texture", 2D) = "white" {}
		//_Factor("Factor", range(0.0001,0.2)) = 0.05
		//_R("radius", range(0, 0.2)) = 0.1
	}
	SubShader
	{
		Tags {
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass
		{
			blend SrcAlpha OneMinusSrcAlpha
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

			half _Factor;
			fixed4 _Color;
			half _Feather;
			half _Angle;

			//旋转UV
			inline fixed2 CalculateRotateUV(half2 uv, half radian)
			{
				fixed2 pivot = fixed2(0.5, 0.5);
				fixed cs = cos(radian);
				fixed sn = sin(radian);
				return mul(float2x2(cs, -sn, sn, cs), uv - pivot) + pivot;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = CalculateRotateUV(v.uv, radians(_Angle));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float a = 0;

				if (i.uv.x < _Factor) {
					a = 1;
				}
				else if (i.uv.x < _Factor + _Feather) {
					a = lerp(0, 1, (_Factor + _Feather - i.uv.x) / _Feather);
				}
				else {
					a = 0;
				}
				
				_Color.a *= a;
				return _Color;
			}

			ENDCG
		}
	}
}
