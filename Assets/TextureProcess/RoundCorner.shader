
//圆角处理
Shader "Vitens/RoundCorner"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _R("圆角大小", range(0, 0.25)) = 0.1
        _Aspect("图片长宽比", float) = 1 
        _Fade("Fade", range(0.0001, 0.01)) = 0.001
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            blend SrcAlpha OneMinusSrcAlpha
            ZWrite off
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
            float2 _MainTex_TexelSize;
            fixed _R;
            float _Aspect;
            float _Fade;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed CalcAlpha(half2 uv){
                //计算出阈值 uv的xy在 +-threshold范围内为显示，剩下四个角需要根据圆来判断
                half threshold = 0.5h - _R;
                //四个角的范围内
                half l = length(abs(uv) - half2(threshold, threshold));
                half stepL = step(_R, l);
                
                //除了角的其他部分
                half stepX = step(threshold, abs(uv.x));
                half stepY = step(threshold, abs(uv.y));

                //三个step只要有一个为0，则alpha就为1，所以需要裁减的部分三个step需要都为1
                return 1 - stepX * stepY * stepL * abs(((l - _R) / _Fade));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                half2 uv = i.uv.xy - half2(0.5h, 0.5h);//将UV中心移动到图片中心

                half aspectOfR = _R / _Aspect;
                //圆角部分的UV进行映射
                if(uv.x > (0.5 - aspectOfR) ){
                    uv.x = lerp(0.5 - _R, 0.5, (uv.x - (0.5 - aspectOfR))/aspectOfR);
                    col.a = CalcAlpha(uv);
                }
                else if(uv.x < (-0.5 + aspectOfR)){
                    uv.x = lerp(-0.5 + _R, -0.5, abs((uv.x - (-0.5 + aspectOfR))/aspectOfR));
                    col.a = CalcAlpha(uv);
                }
                else{
                    col.a = 1;
                }

                return col;
            }
            ENDCG
        }
    }
}

