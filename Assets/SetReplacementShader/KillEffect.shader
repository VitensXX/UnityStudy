Shader "Hidden/KillEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "BattleType" = "Scene" "Queue" = "Transparent"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_3d
			#pragma fragment frag_3d_flash
			#include "UnityCG.cginc"
			#include "BattleKillSceneInclude.cginc"
			ENDCG
		}
	}

	SubShader
	{
		Tags { "BattleType" = "Army" "Queue" = "Transparent"}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_3d_black
			#pragma fragment frag_3d_black
			#include "UnityCG.cginc"
			#include "BattleKillSceneInclude.cginc"
			ENDCG
		}
	}
}
