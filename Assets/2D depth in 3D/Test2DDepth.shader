Shader "Unlit/Test2DDepth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _angle("摆放倾斜角度", float) = 50
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _angle;

            //旋转到垂直于场景的位置
            inline half4 TranslateToVertical(appdata v)
            {
                fixed radian = _angle / 180 * 3.14159;
                fixed cosTheta = cos(radian);
                fixed sinTheta = sin(radian);
                half z = v.vertex.z * cosTheta - v.vertex.y * sinTheta;
                half y = v.vertex.z * sinTheta + v.vertex.y * cosTheta;
                v.vertex = half4(v.vertex.x, y, z, v.vertex.w);
                return UnityObjectToClipPos(v.vertex);
            }

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //在MVP变换之后再进行旋转操作,并修改顶点的Z值(深度)
                //弧度
                fixed radian = _angle / 180 * 3.14159;
                fixed cosTheta = cos(radian);
                fixed sinTheta = sin(radian);

                //旋转中心点(测试用的四边形, 正常的spine做的模型脚下旋转的点就是(0,0), 可以省去下面这一步已经旋转完成后的 +center操作)
                half2 center = half2(0, -0.5);
                v.vertex.zy -= center;

                half z = v.vertex.z * cosTheta - v.vertex.y * sinTheta;
                half y = v.vertex.z * sinTheta + v.vertex.y * cosTheta;
                v.vertex = half4(v.vertex.x, y, z, v.vertex.w);

                v.vertex.zy += center;

                float4 verticalClipPos = UnityObjectToClipPos(v.vertex);
                o.vertex.z = verticalClipPos.z / verticalClipPos.w * o.vertex.w ;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
