Shader "Unlit/rain"
{
	Properties
	{
		_Color ("Color", Color) = (0.5, 0.5, 0.6, 0.3)
		_DepthStart("Depth start", float) = 0.1
		_DepthEnd("Depth end", float) = 0.3
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

		Pass
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float depth : TEXCOORD0;
			};

			float4 _Color;
			float _DepthEnd, _DepthStart;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = o.vertex.z / o.vertex.w;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float d = clamp(i.depth, _DepthStart, _DepthEnd);
				float k = 1 - (d - _DepthStart) / (_DepthEnd - _DepthStart);
				return float4(_Color.xyz, _Color.a);
			}
			ENDCG
		}
	}
}
