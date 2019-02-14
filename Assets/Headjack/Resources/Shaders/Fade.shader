Shader "Hidden/Headjack/Fade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Fade("Fade",range(0,1))=0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, vertex);
				o.uv = uv;
				return o;
			}
			sampler2D _MainTex;
			half _Fade;
			fixed4 frag (v2f i) : SV_Target{ return tex2D(_MainTex, i.uv)*(1 - _Fade); }
			ENDCG
		}
	}
}
