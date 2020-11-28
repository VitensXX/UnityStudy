
Shader "Vitens/Transition Cir"
{
	Properties
	{
		_Color("Color", color) = (0,0,0,1)
		_Factor("Factor", range(-1,1.42)) = 0
		_Feather("Feather", range(0,0.3)) = 0
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 screenPos : TEXCOORD0;
			};

			half _Factor;
			fixed4 _Color;
			half _Feather;
			half _Angle;
			float4 _CenterAndScreenSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float a = 0;
				float d = distance(i.screenPos.xy * _CenterAndScreenSize.zw, _CenterAndScreenSize.xy * _CenterAndScreenSize.zw);
				_Factor *= _CenterAndScreenSize.z;
				_Feather *= _CenterAndScreenSize.z;
				if (d < _Factor) {
					a = 0;
				}
				else if (d < _Factor + _Feather) {
					a = lerp(1, 0, (_Factor + _Feather - d) / _Feather);
				}
				else {
					a = 1;
				}
				
				_Color.a *= a;
				return _Color;
			}

			ENDCG
		}
	}
}
