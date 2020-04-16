// Created by Vitens
#ifndef KILL_SCENE_INCLUDE  
#define KILL_SCENE_INCLUDE  

//闪光 黄色
#define FLASH_COLOR_YELLOE fixed3(1, 1, 0)
//闪光 白色
#define FLASH_COLOR_WHITE fixed3(1, 1, 1)
//剪影 黑色
#define COLOR_BLACK fixed3(0, 0, 0)

//闪烁间隔 黄色持续时间 单位秒,但这个时间会受timeScale影响
#define FLASH_COLOR_YELLOE_DURATION 0.4h
//白色持续时间
#define FLASH_COLOR_WHITE_DURATION 0.2h


// linear change 0 -> 1 -> 0...
inline half2 PerFun010(half T1, half T2)
{
	half T = T1 + T2;
	half x = fmod(_Time.y, T);
	int stepValue = step(T1, x);

	return half2((1 - stepValue) * x / T1 + stepValue * (1 - (x - T1) / T2), stepValue);
}

sampler2D _MainTex;
float4 _MainTex_ST;

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
};


//2d
v2f vert_2d(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	return o;
}

fixed4 frag_2d_black(v2f i) : SV_Target
{
	fixed4 texCol = tex2D(_MainTex, i.uv);
	fixed alpha = max(max(texCol.r, texCol.g), texCol.b) *texCol.a;
	fixed4 col = fixed4(COLOR_BLACK, alpha);
	return col;
}

fixed4 frag_2d_black_army(v2f i) : SV_Target
{
	return fixed4(COLOR_BLACK, 1);
}

fixed4 frag_2d_flash(v2f i) : SV_Target
{
	fixed alpha = tex2D(_MainTex, i.uv).a;
	half2 time = PerFun010(FLASH_COLOR_YELLOE_DURATION, FLASH_COLOR_WHITE_DURATION);
	if (time.y == 0)
	{
		return fixed4(FLASH_COLOR_YELLOE, alpha);
	}
	else {
		return fixed4(FLASH_COLOR_WHITE, alpha);
	}
}


//3d
struct v2f_3d
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	half specular : TEXCOORD1;
};


v2f_3d vert_3d(appdata v)
{
	v2f_3d o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);

	fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	fixed3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
	o.specular = pow(1.0 - abs(dot(worldViewDir, worldNormal)), 0.3h);
	
	return o;
}

fixed4 frag_3d_flash(v2f_3d i) : SV_Target
{
	fixed4 col;

	fixed alpha = tex2D(_MainTex, i.uv).a;

	half2 time = PerFun010(FLASH_COLOR_YELLOE_DURATION, FLASH_COLOR_WHITE_DURATION);
	if (time.y == 0)
	{
		col = fixed4(FLASH_COLOR_YELLOE, alpha);
		col.rgb *= i.specular;
		return col;
	}
	else {
		col = fixed4(FLASH_COLOR_WHITE, alpha);
		col.rgb *= i.specular;
		return col;
	}
}


v2f_3d vert_3d_black(appdata v)
{
	v2f_3d o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);

	fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	fixed3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
	o.specular = pow(1.0 - abs(dot(worldViewDir, worldNormal)), 3);

	return o;
}

fixed4 frag_3d_black(v2f_3d i) : SV_Target
{
	fixed alpha = tex2D(_MainTex, i.uv).a;
	fixed4 col = fixed4(1, 1, 1, alpha);
	col.rgb *= i.specular;

	return col;
}

#endif