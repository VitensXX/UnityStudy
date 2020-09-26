Shader "Custom/Light"
{
	Properties{
		[HDR]_TintColor("Color", Color) = (1,1,1,1)
		_FadeRange("range", range(-1,0)) = -0.5
		_FadeFactor("factor", range(0,10)) = 1
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
			Cull Off
			ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal:NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
                float modelPosZ : TEXCOORD2;
            };

			float4 _TintColor;
			float _FadeRange;
			float _FadeFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.modelPosZ = v.vertex.z;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _TintColor;

                //边缘虚化
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);
				col.a *= abs(dot(viewDir, worldNormal));
                
				//底部虚化(这里的坐标相关得参考具体的模型坐标来给一个合适的值)
				if (i.modelPosZ < _FadeRange) {
					col.a = lerp(col.a, 0, saturate(abs(i.modelPosZ - _FadeRange) * _FadeFactor));
				}

                return col;
            }
            ENDCG
        }
    }
}
