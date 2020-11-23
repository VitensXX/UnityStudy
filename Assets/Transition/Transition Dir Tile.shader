
Shader "Vitens/Transition Dir Tile"
{
	Properties
	{
		_Color("Color", color) = (0,0,0,1)
		_Factor("Factor", range(-1,1.42)) = 0
		_Feather("Feather", range(0,0.3)) = 0
		_Angle("Angle", range(0, 360)) = 0
		_TileSize("Tile Size", range(10,1000)) = 20
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
				float3 worldPos : TEXCOORD1;
			};

			half _Factor;
			fixed4 _Color;
			half _Feather;
			half _Angle;
			float _TileSize;
			float _Width;
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
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = CalculateRotateUV(v.uv, radians(_Angle));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				_Factor *= 3;
				float2 center = round((i.worldPos.xy - _TileSize/2) / _TileSize) * _TileSize + _TileSize/2;

				//错开效果
				int t = i.worldPos.x / _TileSize;
				if (t % 2 == 0)
				{
					if (i.worldPos.y > center.y) {
						center.y += _TileSize / 2;
					}
					else {
						center.y -= _TileSize / 2;
					}
				}

				float tileAlpha = 0;
				float size = lerp(0, _TileSize / 1.8, (_Factor - i.uv.x));
				if (distance(center, i.worldPos.xy) < size)
				{
					tileAlpha = 1;
				}
				else {
					tileAlpha = 0;
				}


				_Color.a *= tileAlpha;
				return _Color;
			}

			ENDCG
		}
	}
}
