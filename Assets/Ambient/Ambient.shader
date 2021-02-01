Shader "Unlit/Ambient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed3 CalcAmbientColor(fixed3 color, fixed3 worldNormal, fixed3 albedo, fixed3 lightDir){
                fixed diff = max(0, dot(worldNormal, lightDir));
                return diff * color * albedo.rgb;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

                // sample the texture
                fixed4 albedo = tex2D(_MainTex, i.uv);
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
                return fixed4(ambient,1);
                //#if _AMBIENT_SKYBOX_COLOR
                    // ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
                //#else
                    // glstate_lightmodel_ambient unity_AmbientEquator UNITY_LIGHTMODEL_AMBIENT
                   fixed3 skyAmbient = CalcAmbientColor(unity_AmbientSky.xyz, worldNormal, albedo, fixed3(0,1,0));
                   fixed3 equatorAmbient = CalcAmbientColor(unity_AmbientEquator.xyz, worldNormal, albedo, viewDir);
                   fixed3 groundAmbient = CalcAmbientColor(unity_AmbientGround.xyz, worldNormal, albedo, fixed3(0,-1,0));
                   ambient =  skyAmbient + equatorAmbient + groundAmbient;
                //#endif
                return fixed4(ambient,1);
            }
            ENDCG
        }
    }
}
