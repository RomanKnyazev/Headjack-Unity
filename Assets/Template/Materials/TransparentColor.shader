Shader "Unlit/TransparentColor"
{
	Properties
	{
		_Color("Tint", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR0;
			};

			struct v2f
			{
				float4 color : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color * _Color;
			}
			ENDCG
		}
	}
}
