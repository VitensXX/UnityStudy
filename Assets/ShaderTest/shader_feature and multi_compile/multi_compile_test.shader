Shader "Test/multi_compile_test"
{
    Properties
	{
		[Toggle(_SHOW_RED_ON)] _DisplayColor ("显示红色?", float) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			//#pragma shader_feature _SHOW_RED_ON
			#pragma multi_compile __ _SHOW_RED_ON

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//原始显示为白色
				fixed4 color = fixed4(1,1,1,1);

				//勾选上显示红色
				#if _SHOW_RED_ON
					color = fixed4(1,0,0,1);
				#endif

				return color;
			}
			ENDCG
		}
	}

	FallBack off
}
