// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// 注意：手动更改此数据可能会妨碍您在 Shader Forge 中打开它
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32725,y:32675,varname:node_4795,prsc:2|emission-4183-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32093,y:32510,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2393,x:32325,y:32552,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32093,y:32681,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32093,y:32839,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:4183,x:32527,y:32777,varname:node_4183,prsc:2|A-6074-A,B-2053-A,C-797-A,D-2393-OUT,E-2002-OUT;n:type:ShaderForge.SFN_DepthBlend,id:2002,x:32320,y:33026,varname:node_2002,prsc:2|DIST-7706-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7706,x:32093,y:33086,ptovrint:False,ptlb:Depth,ptin:_Depth,varname:node_7706,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:6074-797-7706;pass:END;sub:END;*/

Shader "Custom/Add_Smooth" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
		[HDR]_TintColor("Color", Color) = (1,1,1,1)
		_FadeRange("range", range(-1,0)) = -0.5
		_FadeFactor("factor", range(0,10)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
			//blend srcalpha oneminussrcalpha
			blend one one
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            //#pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
			float _Range;
			float _FadeRange;
			float _FadeFactor;
            //uniform float _Depth;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
				float3 normal:NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float modelPosZ : TEXCOORD5;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
				o.modelPosZ = v.vertex.z;
				
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                
				float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				
				//边缘虚化
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);
				_TintColor.a *= abs(dot(viewDir, worldNormal));

				////底部虚化
				if (i.modelPosZ < _FadeRange) {
					_TintColor.a = lerp(_TintColor.a, 0, saturate(abs(i.modelPosZ - _FadeRange) * _FadeFactor));
				}

                float3 emissive = (_MainTex_var.a*i.vertexColor.a*_TintColor.a*(_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));

                return finalRGBA;
            }
            ENDCG
        }
    }
    //CustomEditor "ShaderForgeMaterialInspector"
}
