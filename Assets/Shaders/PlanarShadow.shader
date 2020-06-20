
//平面阴影 
Shader "Battle/PlanarShadow"
{
	Properties
	{
		_ShadowAlpha("阴影透明度", range(0,1)) = 0.6
		_FakeLightPosX ("阴影光源位置X", float) = 0
		_FakeLightPosY ("阴影光源位置Y", float) = 31
		_FakeLightPosZ ("阴影光源位置Z", float) = -210
	}
	SubShader
	{
		pass 
		{   
			Tags
			{
				"LightMode" = "ForwardBase"
				"Queue" = "Transparent" 
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent" 
			}

			Stencil
			{
				Ref 1           //参考值为1，stencilBuffer值默认为0  
				Comp Greater    //stencil比较方式是大于
				Pass replace    //通过的处理是替换，就是拿1替换buffer 的值  
				Fail Keep       //深度检测和模板检测双失败的处理是保持
				ZFail keep      //深度检测失败的处理是保持
			}

			blend srcalpha oneminussrcalpha
			zWrite off
			Offset -1,-1
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag
			#include "UnityCG.cginc"
			//float4x4 _World2Ground;
			//float4x4 _Ground2World;

			sampler2D _MainTex;
			half4 _MainTex_ST;
			fixed _Cutoff;
			float _FakeLightPosX,_FakeLightPosY,_FakeLightPosZ;
			float _Intensity;
			fixed _ShadowAlpha;

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos:SV_POSITION;
				//float4 col : COLOR;
				half2 uv : TEXCOORD0;
				float worldY : TEXCOORD1;
				//half atten : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				float4 wPos = mul(unity_ObjectToWorld, v.vertex);				
				float3 litDir;
				//litDir= WorldSpaceLightDir(v.vertex);
				float3 litDirrr = float3(_FakeLightPosX,_FakeLightPosY,_FakeLightPosZ);
				litDir = litDirrr - wPos;
				//litDir=mul(_World2Ground,float4(litDir,0)).xyz;
				litDir=normalize(litDir);
				float4 vt;
				vt= mul(unity_ObjectToWorld, v.vertex);
				//vt=mul(_World2Ground,vt);
				vt.xz=vt.xz-(vt.y/litDir.y)*litDir.xz;
				vt.y=0;
				//vt=mul(_Ground2World,vt);
				vt=mul(unity_WorldToObject,vt);
				v2f o;
				o.worldY = wPos.y;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				//o.pos = mul(unity_MatrixVP, vt);
				//o.atten=length(vt)/_Intensity;
				//o.atten=length(vt)/_Intensity;
				o.pos = UnityObjectToClipPos(vt);//输出到裁剪空间
				//o.atten = distance(vertex, vt) / _Instensity;// 根据物体顶点到阴影的距离计算衰减
				return o;
			}

			float4 frag(v2f i) : SV_Target 
			{
				clip(i.worldY);

				return fixed4(0,0,0,_ShadowAlpha);
			}
			ENDCG 
		}
	}
}
