
//马赛克
Shader "Vitens/Mosaic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Factor("Factor", range(0.0001,0.2)) = 0.05
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//round(x) 四舍五入, 为了让周围的像素取中心点的颜色值. _Factor范围大小的参数,表现上就是马赛克色块的大小
				//i.uv.x = round(i.uv.x / _Factor) * _Factor;
				//i.uv.y = round(i.uv.y / _Factor) * _Factor;
                //_Factor / 2 的偏移操作是为了保证从左下角开始的小方块都是平均大小
				i.uv.x = round((i.uv.x + _Factor / 2) / _Factor) * _Factor  - _Factor / 2;
				i.uv.y = round((i.uv.y  + _Factor / 2)/ _Factor) * _Factor - _Factor / 2;
				return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
