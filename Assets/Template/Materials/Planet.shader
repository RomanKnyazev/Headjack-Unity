Shader "Unlit/Planet"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EmissionTex("Emission", 2D) = "black" {}
		_EmissionValue("Emission value", float) = 0.1
		_LightDir("Light dir", Vector) = (0, 0, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL0;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(3)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _EmissionTex;
			float4 _EmissionTex_ST;
			float _EmissionValue;
			float4 _LightDir;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.uv, _EmissionTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float diffuse = clamp(dot(normalize(i.normal), -_LightDir.xyz), 0, 1);
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col * diffuse + tex2D(_EmissionTex, i.uv1) * _EmissionValue;
			}
			ENDCG
		}
	}
}
