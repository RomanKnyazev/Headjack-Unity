﻿Shader "Hidden/Headjack/UnlitLaser"
{
	Properties
	{
		_Color("Color",Color)=(1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			fixed4 _Color;
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			float4 vert (float4 vertex : POSITION) : SV_POSITION{return UnityObjectToClipPos(vertex);}
			fixed4 frag () : SV_Target{return _Color;}
			ENDCG
		}
	}
}
