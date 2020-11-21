
Shader "Vitens/Transation Cir"
{
	Properties
	{
		_Color("Color", color) = (0,0,0,1)
		_Factor("Factor", range(-1,1.42)) = 0
		_Feather("Feather", range(0,0.3)) = 0
		_CenterX("CenterX", float) = 0.5
		_CenterY("CenterY", float) = 0.5
		//Max
		//_Scale("Max Screen Size", float) = 1000
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
			fixed _CenterX;
			fixed _CenterY;
			float _Scale;
			float _ScreenWidth, _ScreenHeight;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
			//	_ScreenHeight = 1125;
			//_ScreenWidth = 2436;

				float a = 0;
				float2 screenSize = float2(_ScreenWidth, _ScreenHeight);
				float d = distance(i.screenPos.xy * screenSize, fixed2(_CenterX, _CenterY) * screenSize);
				_Factor *= _ScreenWidth;
				_Feather *= _ScreenWidth;
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
