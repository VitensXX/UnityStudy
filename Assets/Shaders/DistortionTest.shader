Shader "Unlit/DistortionTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _SpeedX ("Speed X", float) = 0
        _SpeedY ("Speed Y", float) = 0
        _Strength ("Strength", float) = 1
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
            #pragma	multi_compile __ TimeFomd
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            float _SpeedX;
            float _SpeedY;
            half _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _NoiseTex);
                //通过取模的方式 防止uv值变的很大很大
                #if TimeFomd
                    float timeAdd = fmod(_Time.x, 1);
                    float2 timer = float2(timeAdd, timeAdd);
                //不处理(因为纹理是Repeat,所以讲道理uv变得很大对纹理采样应该是没有影响的,至少编辑器里是这样)
                #else
                    float2 timer = float2(_Time.x, _Time.x);
                #endif
                //噪声图UV的流动
                o.uv.zw += timer * float2(_SpeedX, _SpeedY); 
                return o;
            }

            // 使用fixed精度计算扰动后的UV改变值
            inline fixed2 SamplerFromNoise_Fixed(half2 uv)
			{
				fixed4 noiseColor = tex2D(_NoiseTex, uv);
				noiseColor = (noiseColor * 2 - 1) * 0.005;
				return noiseColor.xy;
			}

            // 使用half精度计算扰动后的UV改变值
            inline half2 SamplerFromNoise_Half(half2 uv)
			{
				half4 noiseColor = tex2D(_NoiseTex, uv);
				noiseColor = (noiseColor * 2 - 1) * 0.005;
				return noiseColor.xy;
			}

            // 使用float精度计算扰动后的UV改变值
            inline float2 SamplerFromNoise_Float(half2 uv)
			{
				float4 noiseColor = tex2D(_NoiseTex, uv);
				noiseColor = (noiseColor * 2 - 1) * 0.005;
				return noiseColor.xy;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                //左边部分用float精度
                if(i.uv.x < 0.5 * _NoiseTex_ST.x){
                    float2 noiseOffset = SamplerFromNoise_Float(i.uv.zw) * _Strength;
                    fixed4 finalCol = tex2D(_MainTex, i.uv.xy + noiseOffset);
                    return finalCol;
                }
                //右下角用fixed
                else if(i.uv.y < 0.5 * _NoiseTex_ST.y){
                    fixed2 noiseOffset = SamplerFromNoise_Fixed(i.uv.zw) * _Strength;
                    fixed4 finalCol = tex2D(_MainTex, i.uv.xy + noiseOffset);
                    return finalCol;
                }
                //右上角用half
                else{
                    half2 noiseOffset = SamplerFromNoise_Half(i.uv.zw) * _Strength;
                    fixed4 finalCol = tex2D(_MainTex, i.uv.xy + noiseOffset);
                    return finalCol;
                }

            }
            ENDCG
        }
    }
}
