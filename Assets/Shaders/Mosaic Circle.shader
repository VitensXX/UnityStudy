
//马赛克 圆形
Shader "Vitens/Mosaic Circle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Factor("Factor", range(0.0001,0.2)) = 0.05
		_R("radius", range(0, 0.2)) = 0.1
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
			cull off
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed _Factor;
			fixed _R;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			//可错开的圆形
            fixed4 frag (v2f i) : SV_Target
            {
				int t = (i.uv.x) / _Factor;
				float2 center = round((i.uv + _Factor / 2) / _Factor) * _Factor - _Factor / 2;
				if (t % 2 == 0) 
				{
					if (i.uv.y > center.y) {
						center.y += _Factor / 2;
					}
					else {
						center.y -= _Factor / 2;
					}
				}
				
				if (distance(center, i.uv) < _R)
				{
					i.uv = center;
				}


				return tex2D(_MainTex, i.uv);
            }

			//圆形
			/*fixed4 frag(v2f i) : SV_Target
			{
				float2 center = round((i.uv + _Factor / 2) / _Factor) * _Factor - _Factor / 2;
				if (distance(center, i.uv) < _R)
				{
					i.uv = center;
				}

				return tex2D(_MainTex, i.uv);
			}*/
            ENDCG
        }
    }
}
