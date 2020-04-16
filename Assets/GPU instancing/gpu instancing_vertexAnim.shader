Shader "Vitens/Gpu Instancing_vertexAnim"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint("tint", Color) = (1,1,1,1)
        _AnimMap ("AnimMap", 2D) ="white" {}
        _NormalMap ("NormalMap", 2D) ="white" {}
        _Diffuse ("Diffuse", Color) = (1, 1, 1, 1)
		_Specular ("Specular", Color) = (1, 1, 1, 1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
        _AnimLen("Anim Length", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing //Use this to instruct Unity to generate instancing variants. It is not necessary for surface Shaders.

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID // necessary only if you want to access instanced properties in fragment Shader.
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss;

            sampler2D _AnimMap;
			float4 _AnimMap_TexelSize;//x == 1/width
            sampler2D _NormalMap;
			float4 _NormalMap_TexelSize;
            float _AnimLen;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Tint)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v, uint vid : SV_VertexID)
            {
                float f = _Time.y / _AnimLen;

				fmod(f, 1.0);

				float animMap_x = (vid+0.5) * _AnimMap_TexelSize.x;
				float animMap_y = f;

				float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

				float4 normal = tex2Dlod(_NormalMap, float4(animMap_x, animMap_y, 0, 0));
                v.normal = normal;

                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o); // necessary only if you want to access instanced properties in the fragment Shader

                o.vertex = UnityObjectToClipPos(pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.worldPos = mul(unity_ObjectToWorld, pos).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // necessary only if any instanced properties are going to be accessed in the fragment Shader.

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLightDir));
				fixed3 reflectDir = normalize(reflect(-worldLightDir, worldNormal));
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(reflectDir, viewDir)), _Gloss);

                fixed4 col = tex2D(_MainTex, i.uv) * fixed4(ambient + diffuse + specular, 1.0) * UNITY_ACCESS_INSTANCED_PROP(Props, _Tint);

                if(col.a < 0.6)
                    discard;

                return col;
            }
            ENDCG
        }
    }

    //FallBack "Specular"
}
