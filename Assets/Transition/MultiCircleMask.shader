//黑色圆形转场效果
Shader "Custom/UI/Multi Circle Mask"
{
    Properties
    {
        [HideInInspector]_MainTex("Texture", 2D) = "white" {}
        _Factor("Factor", range(-1,1.42)) = 0
        _Feather("Feather", range(0,0.3)) = 0
        _CenterX("CenterX", float) = 0.5
        _CenterY("CenterY", float) = 0.5
    }
    SubShader
    {
        Tags {
            "IgnoreProjector" = "True"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

    Pass
    {
        blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            float3 screenPos : TEXCOORD0;
            fixed4 color : COLOR;
        };

        half _Factor;
        //fixed4 _Color;
        half _Feather;
        half _Angle;
        fixed _CenterX;
        fixed _CenterY;
        float _Scale;
        // float _ScreenWidth , _ScreenHeight;
        float _CenterArr[6];

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            //o.worldPos = mul(unity_ObjectToWorld, v.vertex);
            o.screenPos = ComputeScreenPos(o.vertex);
            o.color = v.color;
            return o;
        }

        float CalcA(float2 UVscreenPos, float2 screenSize, float x, float y){
            // float _ScreenWidth = 2436;
            // float  _ScreenHeight = 1125; 
            // float2 screenSize = float2(_ScreenWidth, _ScreenHeight);

            // float2 screenSize = float2(_ScreenWidth, _ScreenHeight);
            float d = distance(UVscreenPos.xy * screenSize, fixed2(x, y) * screenSize);
            float factor = _Factor;
            float feather = _Feather;
            factor *= screenSize.x;
            feather *= screenSize.x;
            float a = 0;
            if (d < _Factor) {
                a = 0;
            }
            else if (d < factor + feather) {
                a = lerp(1, 0, (factor + feather - d) / feather);
            }
            else {
                a = 1;
            }

            return saturate(a);
        }

        fixed4 frag(v2f i) : SV_Target
        {
            //----test
            float _ScreenWidth = 2436;
            float  _ScreenHeight = 1125; 
            //----test 
            float a = 0;
            float2 screenSize = float2(_ScreenWidth, _ScreenHeight);
            // float d = distance(i.screenPos.xy * screenSize, fixed2(_CenterX, _CenterY) * screenSize);
            // _Factor *= _ScreenWidth;
            // _Feather *= _ScreenWidth;

            // if (d < _Factor) {
            //     a = 0;
            // }
            // else if (d < _Factor + _Feather) {
            //     a = lerp(1, 0, (_Factor + _Feather - d) / _Feather);
            // }
            // else {
            //     a = 1;
            // }
            for(int k = 0; k < 3; k++){
                i.color.a *= CalcA(i.screenPos, screenSize, _CenterArr[k*2], _CenterArr[k*2+1]);
            }
            // i.color.a *= CalcA(i.screenPos, screenSize, _CenterX, _CenterY);
            // i.color.a *= CalcA(i.screenPos, screenSize, 0, 0);
            // float r = i.color.r;
            // // float r1 = CalcA(i.screenPos, screenSize, 0, 0);
            // float r2 = CalcA(i.screenPos, screenSize, _CenterX, _CenterY);
            // r *=  r2;
            // return fixed4(r,0,0,1);
            // // i.color.a *= a;
            return i.color;
        }

        ENDCG
        }
    }
}