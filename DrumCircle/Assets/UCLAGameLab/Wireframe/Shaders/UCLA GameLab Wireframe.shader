// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "UCLA Game Lab/Wireframe/Single-Sided" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1
	}

	SubShader 
	{
		Pass
			{
					Name "RiverDepth"
					Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

#pragma vertex vert  
#pragma fragment frag 

#include "UnityCG.cginc"

			uniform float4 _LightColor0;
		// color of light source (from "Lighting.cginc")

		uniform float4 _Color; // define shader property for shaders

		struct vertexInput {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};
		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 col : COLOR;
		};

		vertexOutput vert(vertexInput input)
		{
			vertexOutput output;

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 normalDirection = normalize(
				mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

			float3 diffuseReflection = _LightColor0.rgb * _Color.rgb
				* max(0.0, dot(normalDirection, lightDirection));

			output.col = float4(diffuseReflection, 1.0);
			output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			return input.col;
		}

			ENDCG
		}
		Pass
		{
			Tags { "RenderType"="Transparent" "Queue"="Transparent" }

			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite Off
			LOD 200
			
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc"
				#include "UCLA GameLab Wireframe Functions.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				// Vertex Shader
				UCLAGL_v2g vert(appdata_base v)
				{
					return UCLAGL_vert(v);
				}
				
				// Geometry Shader
				[maxvertexcount(3)]
				void geom(triangle UCLAGL_v2g p[3], inout TriangleStream<UCLAGL_g2f> triStream)
				{
					UCLAGL_geom( p, triStream);
				}
				
				// Fragment Shader
				float4 frag(UCLAGL_g2f input) : COLOR
				{	
					return UCLAGL_frag(input);
				}
			
			ENDCG
		}

			
	} 

}
