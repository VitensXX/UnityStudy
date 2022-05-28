Shader "UI/ColorGrading"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _grade("grade", range(0,1)) = 1

        _Exposure("exposure", range(0,10)) = 1
        _ColorFilter("ColorFilter", color) = (1,1,1,1)
        _HueOffset("hue", range(-0.5,0.5)) = 0
        _Saturation("saturation", range(0,3)) = 0
        _Contrast("contrast", range(0,1)) = 0
    }
    SubShader
    {
        blend srcAlpha OneMinusSrcAlpha
        Tags { "RenderType"="Transparent" }
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color :COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            float _grade;
            float _Exposure;
            float _Contrast;
            fixed4 _ColorFilter;
            float _HueOffset;
            float _Saturation;

            fixed3 ColorGrade(fixed3 color){
                color = min(color, _grade);
                return color;
            }

            fixed3 Exposure(fixed3 color, float exposure){
                return color * exposure;
            }


    fixed3 HSVToRGB( fixed3 c)
	{
		float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
		fixed3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
		return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
	}
    
	fixed3 RGBToHSV(fixed3 c)
	{
		float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
		float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
		float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
		float d = q.x - min( q.w, q.y );
		float e = 1.0e-10;
		return fixed3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
	}



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;


                col.rgb = ColorGrade(col.rgb);
                col.rgb = Exposure(col.rgb, _Exposure);
                
                

                col.rbg *= _ColorFilter.rbg;


                float lu = Luminance(col.rbg);
                col.rbg = lerp(col.rbg, lu.xxx, _Contrast);

                float3 hsv = RGBToHSV(col.rbg);
                hsv.r += _HueOffset;
                hsv.g *= _Saturation;
                col.rgb = HSVToRGB(hsv);



                

                return col;
            }
            ENDCG
        }
    }
}
