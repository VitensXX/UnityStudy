Shader "Hidden/KillEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "TransparentCutout" "Queue" = "Transparent"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_2d
			#pragma fragment frag_2d_flash
			#include "UnityCG.cginc"
			#include "BattleKillSceneInclude.cginc"
			ENDCG
		}
	}

	SubShader
	{
		Tags { "RenderType" = "Army" "Queue" = "Transparent"}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_2d
			#pragma fragment frag_2d_black_army
			#include "UnityCG.cginc"
			#include "BattleKillSceneInclude.cginc"

			ENDCG
		}
	}
}
