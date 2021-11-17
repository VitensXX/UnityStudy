Shader "P03/OutlineFor3D"
{
    Properties
    {
		_Outline("轮廓范围", float) = 3
		_OutlineColor("轮廓颜色", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "LightMode" = "Always"}

		pass {
			NAME "OUTLINE"
			Cull Front
			ZWrite[_ZWrite]

			blend srcalpha oneminusSrcalpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Outline;
			fixed4 _OutlineColor;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};

			v2f vert(a2v v) {
				v2f o;

				/*float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				normal.z = -0.5;
				pos = pos + float4(normalize(normal), 0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);*/

				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				//o.pos = UnityObjectToClipPos(v.vertex + v.normal * _Outline);
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 clipNormal = mul((float3x3)UNITY_MATRIX_MVP, v.normal).xyz;
				float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _Outline * o.pos.w;
				o.pos.xy += offset;

				return o;
			}

			float4 frag(v2f i) : SV_Target {
				/*fixed4 color;
				color.rgb = i.pos.w;
				color.a = _OutlineColor.a;*/

				fixed4 color = _OutlineColor;

				//边缘虚化
				/*fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);
				color.a *= abs(dot(viewDir, worldNormal)*2);*/

				return color;
			}

			ENDCG
		}

        //Pass
        //{
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    // make fog work
        //    #pragma multi_compile_fog

        //    #include "UnityCG.cginc"

        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        UNITY_FOG_COORDS(1)
        //        float4 vertex : SV_POSITION;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;

        //    v2f vert (appdata v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);
        //        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //        UNITY_TRANSFER_FOG(o,o.vertex);
        //        return o;
        //    }

        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        // sample the texture
        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        // apply fog
        //        UNITY_APPLY_FOG(i.fogCoord, col);
        //        return col;
        //    }
        //    ENDCG
        //}
    }
}
