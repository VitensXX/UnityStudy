Shader "P03/V2/siwa"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white"{}
		_RimColorRight("rim right color", color) = (1,1,1,1)
		_RimColorLeft("rim left color", color) = (1,1,1,1)
		// _RimPower("rom power", range(1 , 64)) = 8
		// _Threshold("Threshold", Range(0, 1)) = 1
		// _SpecularStrength("_SpecularStrength", range(0,1)) = 0.2
		// _SpecularRamp("_SpecularRamp", range(0,1)) = 0.2

        _SWColor("SW color", color) = (0,0,0,1)
        _ColorRange("color range", range(-1,1)) = 0
        _OriginLerp("originLerp", range(0,1)) = 0
        _OutlienRangeRight("range right", range(0, 1)) = 0.5 
        _OutlienRangeLeft("range left", range(0, 1)) = 0.5 
        _Range("Range", range(-1,5)) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			Pass
			{
                blend srcalpha oneminusSrcalpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 worldNormal : TEXCOORD1;
					float3 worldPos : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _MaskTex;
				fixed4 _RimColorRight, _RimColorLeft;
				float _RimPower;
				float _Threshold;
				float _SpecularStrength;
				float _SpecularRamp;
                fixed4 _SWColor;
                float _Range,_ColorRange;
                float _OutlienRangeLeft,_OutlienRangeRight;
                float _OriginLerp;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);

					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
					fixed3 worldHalfDir = normalize(worldLightDir + worldViewDir);


					//half powValue = pow(1.0 - abs(dot(worldViewDir, worldNormal)), _RimPower);
					/*half powValue = abs(dot(worldViewDir, worldNormal));
					if (powValue < _Threshold) {
						col = _RimColor;
					}
					else {

					}*/
					//col.rgb += _RimColor.rgb * powValue;

                    

                    



					half NDotL = dot(worldViewDir, worldLightDir);
					// half v = smoothstep(0.5, 0.7, NDotL);
					// v = lerp(0.9, 1, v);
					// col *= v;


					//二值化高光
					// half NdotH = saturate(dot(worldNormal, worldHalfDir));
					// v = pow(NdotH, _RimPower); // 缩小夹角系数的值，由于NdotH在0-1，所以pow后会变得更小，_Smoothness参考值为8-64
					// v = step(v, _SpecularRamp); // 小于_SpecularRamp的值将为0，反之为1
					// v = v * _SpecularStrength; // 定义高光强度，参考值为0.2
					// col += col * v; // 在原有颜色的基础上叠加

                    // // if(NdotH > _Range){
                    //     // col.rgb = lerp(col.rgb, _SWColor.rgb, 1 - NdotH);
                    //     col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                    //     col.rgb = lerp(col.rgb, _SWColor.rgb, 1 - NdotH - _ColorRange);
                    // // }


                    half NDotV = dot(worldViewDir, worldNormal);
                    half NdotH = saturate(dot(worldNormal, worldHalfDir));


                    // if(NDotV < _OutlienRangeRight && worldNormal.x > 0){
                    //     // return fixed4(1,0,0,1);
                        
                    //     // worldNormal.x = 0;
                    //     // half NdotH = saturate(dot(worldNormal, worldHalfDir));
                    //     // v = pow(NdotH, _RimPower); // 缩小夹角系数的值，由于NdotH在0-1，所以pow后会变得更小，_Smoothness参考值为8-64
                    //     // v = step(v, _SpecularRamp); // 小于_SpecularRamp的值将为0，反之为1
                    //     // v = v * _SpecularStrength; // 定义高光强度，参考值为0.2
                    //     // col += col * v; // 在原有颜色的基础上叠加

                    //     // // if(NdotH > _Range){
                    //     //     // col.rgb = lerp(col.rgb, _SWColor.rgb, 1 - NdotH);
                    //     //     col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                    //     //     col.rgb = lerp(col.rgb, _SWColor.rgb, NdotH - _ColorRange);
                    //     // }
                    //     // col.rgb += _RimColorRight.xyz;

                    // }
                
                    // else if(NDotV < _OutlienRangeLeft && worldNormal.x < 0){
                    //     // return fixed4(0,0,1,1);

                    //     col.rgb += _RimColorLeft.xyz;
                    // }
                    // else{


                    fixed4 maskColor = tex2D(_MaskTex, i.uv);
                    if(maskColor.r > 0.1){

                        col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                        float lerpVal = 1 - NdotH - _ColorRange;
                        col.rgb = lerp(col.rgb, _SWColor.rgb, saturate(lerpVal) );

                        if(NDotV < _OutlienRangeRight && worldNormal.x > 0){

                            // col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                            // float lerpVal = 1 - NdotH - _ColorRange;
                            // col.rgb = lerp(col.rgb, _SWColor.rgb, lerpVal );

                            col.rgb += _RimColorRight.rgb;
                        }
                        else if(NDotV < _OutlienRangeLeft && worldNormal.x < 0){
                            // col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                            // float lerpVal = 1 - NdotH - _ColorRange;
                            // col.rgb = lerp(col.rgb, _SWColor.rgb, lerpVal );

                            col.rgb += _RimColorLeft.rgb;  
                        }
                        // else{
                        //     col.rgb = lerp(col.rgb, _SWColor.rgb, _OriginLerp);
                        //     float lerpVal = 1 - NdotH - _ColorRange;
                        //     col.rgb = lerp(col.rgb, _SWColor.rgb, lerpVal );
                        // }
                    }

                    // }

                    // }

					return col;
				}
				ENDCG
			}
		}
}
