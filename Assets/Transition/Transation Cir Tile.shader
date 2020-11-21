
Shader "Vitens/Transation Cir Tile"
{
	Properties
	{
		_Color("Color", color) = (0,0,0,1)
		_Factor("Factor", range(-1,1.42)) = 0
		_CenterX("CenterX", float) = 0.5
		_CenterY("CenterY", float) = 0.5
		//Max
		_Scale("Max Screen Size", float) = 1000
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			half _Factor;
			fixed4 _Color;
			fixed _CenterX;
			fixed _CenterY;
			float _Scale;
			float _TileSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				_Factor *= _Scale;

				float a = 0;
				//float d = distance(i.worldPos, fixed2(_CenterX, _CenterY));


				float2 center = round((i.worldPos.xy) / _TileSize) * _TileSize;
				float tileAlpha = 0;
				float size = lerp(0, _TileSize / 1.8, distance(i.worldPos.xy, fixed2(_CenterX, _CenterY))/ _Factor);
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

