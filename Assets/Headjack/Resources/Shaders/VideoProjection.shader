Shader "Hidden/Headjack/EquiToCubeProjection" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "black" {}
	}
	SubShader { 
		Tags { "RenderType"="Opaque" "Queue"="Background" }
		LOD 100
		cull back
		ZWrite off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.worldPos=  mul(unity_ObjectToWorld, vertex).xyz;
				return o;
			}
			float2 WorldToEqui(float3 wp)
			{
				return float2(atan2(wp.x,wp.z)*0.15915495087 ,asin(wp.y)*0.3183099524)+0.5;
			}
			fixed4 frag (v2f i) : SV_Target{
				return tex2D(_MainTex, _MainTex_ST.zw+(WorldToEqui(normalize(i.worldPos-_WorldSpaceCameraPos))*_MainTex_ST.xy));
			}
			ENDCG
		}
	}
}
 