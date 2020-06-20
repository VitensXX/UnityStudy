//Created by Vitens

//战场卡通轮廓描边,支持纯自发光效果(不受光照影响),也可与光照交互,漫反射,高光,内置的阴影投射
//添加对光照贴图的支持

Shader "P03/BattlefieldToon" {
   Properties {
		_ColorGlitter ("Color Glitter", Color) = (0, 0, 0, 0)
		_MainTex ("Main Tex", 2D) = "white" {}
		//_Ramp ("Ramp Texture", 2D) = "white" {}
		_ZWrite("轮廓类型(1效果更好, 如果闪烁则切换为0)", Float) = 0
		_Outline ("轮廓范围", Range(0, 1)) = 0.1
		_OutlineColor ("轮廓颜色", Color) = (0, 0, 0, 1)
		_Specular ("高光颜色", Color) = (1, 1, 1, 1)
		_SpecularScale ("高光范围", Range(0, 0.1)) = 0.01
		_LightIntensity("光照强度(0为自发光,不受光照影响)", range(-1,1)) = 0.5
		[PerRendererData]_Alpha("透明度", Range(0,1)) = 1
		[PerRendererData]_RimColor("边缘泛光颜色", COLOR) = (1,1,1,1)
		[PerRendererData]_RimPower("边缘泛光参数", range(0.1,5)) = 2
		[PerRendererData]_rimIntensity("边缘泛光强度", range(0.1, 3)) = 1
		[PerRendererData]_ZWrite2("透明状态下需要关闭的深度写入", float) = 1

		[Space(20)]
		_NoiseTex("Noise", 2D) = "white" {}
		//_Threshold("Threshold", Range(0.0, 1.0)) = 0
		//_EdgeLength("Edge Length", Range(0.0, 0.2)) = 0.1
		//_EdgeFirstColor("First Edge Color", Color) = (1,1,1,1)
		//_EdgeSecondColor("Second Edge Color", Color) = (1,1,1,1)
		//_DistanceEffect("distanceEffect", range (0,1)) = 0.5
		//_AshWidth("AshWidth",Range(0,0.5)) = 0
	}
    SubShader {
		
		Pass {
			Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode" = "Always"}
			NAME "OUTLINE"
			Cull Front
			ZWrite [_ZWrite]
			
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
			};
			
			v2f vert (a2v v) {
				v2f o;
			
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
				normal.z = -0.5;
				pos = pos + float4(normalize(normal), 0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target { 
				return float4(_OutlineColor.rgb, 1);               
			}
			
			ENDCG
		}
		
		Pass {
			Tags { "LightMode"="ForwardBase" }
			
			blend srcAlpha oneMinussrcalpha

			Cull off
			ZWrite [_ZWrite2]
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ RIMCOLOR_ON
			#pragma multi_compile __ ONLY_DISPLAY_RIMCOLOR BLACK_AND_RIMCOLOR

			#pragma multi_compile_fwdbase
		
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityShaderVariables.cginc"
			
			fixed4 _ColorGlitter;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Ramp;
			fixed4 _Specular;
			fixed _SpecularScale;
			fixed _LightIntensity;
			half _Alpha;
			half4 _RimColor;
			half _RimPower;

			sampler2D _NoiseTex;
			half4 _NoiseTex_ST;
			half _Threshold;
			half _EdgeLength;
			fixed4 _EdgeFirstColor;
			//fixed4 _EdgeSecondColor;
			half _DistanceEffect;
			half _AshWidth;
			half2 _BeginPos;
			half _rimIntensity;
		
			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 tangent : TANGENT;
			}; 
		
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float2 uv2 : TEXCOORD3;//光照贴图UV
				float2 uvNoiseTex : TEXCOORD4; 
				SHADOW_COORDS(5)
			};
			
			v2f vert (a2v v) {
				v2f o;
				
				o.pos = UnityObjectToClipPos( v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.uv2 = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;//光照贴图的UV
				o.worldNormal  = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uvNoiseTex = TRANSFORM_TEX(v.texcoord, _NoiseTex);
				
				TRANSFER_SHADOW(o);
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target { 
				
				fixed4 c = tex2D (_MainTex, i.uv);

				half dist = 1 - i.uv.x; 

				fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).g * (1 - _DistanceEffect) + dist * _DistanceEffect;
				c.a *= 1 - step(cutout - _Threshold + _AshWidth,0);

				half degree = saturate((cutout - _Threshold + _AshWidth) / _EdgeLength);
				//fixed4 edgeColor = lerp(_EdgeFirstColor, _EdgeSecondColor, degree);

				//edgeColor.rgb = pow(edgeColor.rgb,5);
				//_EdgeFirstColor.rgb = pow(_EdgeFirstColor.rgb,5);
				
				//c.rgb = lerp(_EdgeFirstColor.rgb, c.rgb, degree);
				c.rgb += _EdgeFirstColor.rgb * (1 - degree);
				
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				// 使用自发光颜色 不需要计算光照
				if(_LightIntensity <= 0.001f){
					c.rgb *= (1 - _LightIntensity);
					c.rgb += _ColorGlitter;

					//fixed4 outlineColor;
					#if RIMCOLOR_ON
						half powValue;
						//c.rgb = fixed3(1,1,1);
						//half rim = 1.0 - abs(dot(worldViewDir, worldNormal));
						powValue = pow(1.0 - abs(dot(worldViewDir, worldNormal)), _RimPower);
						//c.rgb += _RimColor.rgb * powValue;

						//outlineColor.rgb = _RimColor.rgb * pow(rim, _RimPower);
						//c.rgb *= _RimColor.rgb * pow(rim, _RimPower);

						#if ONLY_DISPLAY_RIMCOLOR
							return fixed4(_RimColor.rgb * powValue, saturate(powValue * _rimIntensity));
						#elif BLACK_AND_RIMCOLOR 
							c.rgb = fixed3(1,1,1);
							c.rgb *= _RimColor.rgb * powValue;
							return fixed4(c.rgb, 1);
						#else
							c.rgb += _RimColor.rgb * powValue;
							return fixed4(c.rgb, _Alpha);
						#endif

					#endif

					return c;
				}
				
				//fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				//fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 worldHalfDir = normalize(worldLightDir + worldViewDir);
				
				fixed3 albedo = c.rgb;
				
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
				
				fixed diff =  dot(worldNormal, worldLightDir);
				diff = (diff * 0.5 + 0.5) * atten;
				
				//fixed3 diffuse = _LightColor0.rgb * albedo * tex2D(_Ramp, float2(diff, diff)).rgb;
				fixed3 diffuse = _LightColor0.rgb * albedo;
				
				fixed spec = dot(worldNormal, worldHalfDir);
				fixed w = fwidth(spec) * 2.0;
				fixed3 specular = _Specular.rgb * lerp(0, 1, smoothstep(-w, w, spec + _SpecularScale - 1)) * step(0.0001, _SpecularScale);
				
				fixed3 col = _LightIntensity * (ambient + diffuse + specular);
				col.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));//光照贴图的颜色值解码
				col.rgb += _ColorGlitter;

				return fixed4(col, c.a);
			}
		
			ENDCG
		}
	}
	FallBack "Diffuse"
}
