Shader "Hidden/SceneChange/RotationSceneChange"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Factor("Factor", range(0,1)) = 0
    }

    CGINCLUDE

        #include "UnityCG.cginc"
        #include "Common.cginc"

        float4 _Center;
        float _Factor;
        float _Strength;
        fixed4 _bgColor;

        struct Varyings
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        Varyings VertBlit(AttributesDefault v)
        {
            Varyings o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord, _MainTex_ST);
            return o;
        }

        half4 FragBlit(Varyings i) : SV_Target
        {
            float2 dir = i.uv - _Center.xy;
            float rot = _Factor * 0.1745 / (length(dir) + 0.001);
            fixed sinval, cosval;
            sincos(rot, sinval, cosval);
            float2x2  rotmatrix = float2x2(cosval, -sinval, sinval, cosval);
            dir = mul(dir, rotmatrix);
            dir += _Center.xy;
            fixed4 color = tex2D(_MainTex, dir);
            color.a *= lerp(1,0, _Factor / _Strength);
            float lerpVal = _Factor / 10;
            color.rgb = lerp(color.rgb, _bgColor.rgb, lerpVal);
            
            return color;
        }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM

                #pragma vertex VertBlit
                #pragma fragment FragBlit

            ENDCG
        }
    }
}
