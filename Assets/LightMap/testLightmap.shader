Shader "Battle/NormalGrid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Diffuse ("Diffuse", Color) = (1, 1, 1, 1)
		_Specular ("Specular", Color) = (1, 1, 1, 1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
        _Metallic("Metallic",Range(0,1)) = 0
    }
    SubShader
    {

        Pass
        {
            Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
			

            struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float2 uv2 : TEXCOORD3;//光照贴图UV
                UNITY_FOG_COORDS(4)
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss;
            float _Metallic;

            v2f vert (a2v v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv2.xy * unity_LightmapST.xy + unity_LightmapST.zw;//光照贴图的UV
                o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				
				fixed3 diffuse = col.rgb * _LightColor0.rgb * _Diffuse.rgb * max(0, dot(worldNormal, worldLightDir)) * (1-_Metallic);
				
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss);
				// return col;


				// col.rgb = fixed3(ambient * (diffuse + specular));
				// col.rgb = fixed3(diffuse + specular);
				// col.rgb = col.rgb *ambient;
                // col.rgb *= ambient + diffuse + specular;
                col.rgb = ambient;
                // col.rgb += DecodeLightmapRGBM(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2),unity_Lightmap_HDR);//光照贴图的颜色值解码
                col.rgb += DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));//光照贴图的颜色值解码

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }

        // Pass{    
        //     Tags { "RenderType"="Opaque"  "LightMode" = "ForwardAdd"}
        //     Blend One One
        //     CGPROGRAM  

        //     fixed4 _Color;

        //     #define POINT
        
        //     #include "Autolight.cginc" 
        //     #include "UnityPBSLighting.cginc"
        //     #pragma vertex vert      
        //     #pragma fragment frag  
        //     struct a2v{  
        //         float4 vertex : POSITION;  
        //         float3 normal : NORMAL;  
        //     };  
  
        //     struct v2f {  
        //         float4 pos : SV_POSITION;  
        //         float3 worldNormal : TEXCOORD0;  
        //         float3 worldPos : TEXCOORD1;  
        //     };  
  
        //     v2f vert(a2v v) {  
        //         v2f o;  
        //         o.pos = UnityObjectToClipPos (v.vertex);  
        //         o.worldNormal = UnityObjectToWorldNormal(v.normal);  
        //         o.worldPos =  mul(unity_ObjectToWorld,v.vertex).xyz;  
        //         return o;  
  
        //     }  
  
        //     fixed4 frag(v2f i) : SV_Target{  
        //         UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
        //         return _Color*attenuation;  
        //     }  

        //     ENDCG   
        // }  
    }
    FallBack "Diffuse"
}
