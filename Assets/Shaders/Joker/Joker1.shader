Shader "Joker/Joker 1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask("Mask(R)",2d) = "white" {}
        _TextureDetails("details", vector) = (0,0,0,0)
        _ImageDetails("uvScale", vector) = (0,0,0,0)
        _RayOffset("rayOffset", vector) = (0,0,0,0)
        _Light("light", range(0,0.2)) = 0.1
        _LightCol("light color", color) = (1,1,1,1)
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
            #include "JokerInclude.cginc"

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
 
            float4 _TextureDetails;
            float2 _RayOffset;
            float2 _ImageDetails;
            float _Light;
            fixed4 _LightCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 RayEffect(fixed4 tex, half2 uv)
            {
                uv = (uv*(_ImageDetails.xy) - _TextureDetails.xy*_TextureDetails.zw)/_TextureDetails.zw;
                half2 adjusted_uv = uv - 0.5; 
                adjusted_uv.x = adjusted_uv.x*_TextureDetails.z/_TextureDetails.w;

                //圆
                half fac = max
                    (min(2*sin((length(90*adjusted_uv) + _RayOffset.x*2) + 3*(1+0.8*cos(length(113.1121*adjusted_uv) - _RayOffset.x*3.121))) 
                    - 1 - max(5.-length(90*adjusted_uv), 0), 1), 0);

                half2 rotater = half2(cos(_RayOffset.x*0.1221), sin(_RayOffset.x*0.3512));

                half angle = dot(normalize(rotater),normalize(adjusted_uv));

                //放射高光
                half fac2 = max(
                    min(5*cos(_RayOffset.y*0.3 + angle*3.14*(2.2+0.9*sin(_RayOffset.x*1.65 + 0.2*_RayOffset.y))) 
                    - 4
                    - max(2-length(20*adjusted_uv), 0), 1)
                    , 0);

                //横向高光
                half fac3 = 0.3*max(min(2.*sin(_RayOffset.x*5. + uv.x*3. + 3.*(1.+0.5*cos(_RayOffset.x*7.))) - 1, 1), -1);
                //竖向高光
                half fac4 = 0.3*max(min(2.*sin(_RayOffset.x*6.66 + uv.y*3.8 + 3.*(1.+0.5*cos(_RayOffset.x*3.414))) - 1, 1), -1);
                half maxfac = max(max(fac, max(fac2, max(fac3, max(fac4, 0.0)))) + 2.2*(fac+fac2+fac3+fac4), 0);

                fixed4 origin = tex;

                half low = min(tex.r, min(tex.g, tex.b));
                half high = max(tex.r, max(tex.g, tex.b));
                half delta = min(high, max(0.5, 1 - low));

                tex.r = tex.r-delta + delta*maxfac*0.5;
                tex.g = tex.g-delta + delta*maxfac*0.5;
                tex.b = tex.b + delta*maxfac*1.9;
                // tex.a = min(tex.a, 0.3*tex.a + 0.9*min(1, maxfac*0.1));

                return tex;
            }

            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 ray = fixed4( _LightCol.rgb * _Light ,1);
                ray = RayEffect(ray, i.uv);
                
                MASK(ray, i.uv)
                
                col.rgb += ray.rgb;
                return col;
            }
            ENDCG
        }
    }
}
