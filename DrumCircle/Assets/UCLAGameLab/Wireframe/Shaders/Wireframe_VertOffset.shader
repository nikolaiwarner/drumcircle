// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "VertOffset" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_SecondTex("Second Texture", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1
	}

	SubShader 
	{
		Pass
				{
					Name "RiverDepth"
					Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays
//#pragma exclude_renderers d3d11 xbox360 gles

#pragma vertex vert  
#pragma fragment frag 

#include "UnityCG.cginc"

			uniform float4 _LightColor0;
		// color of light source (from "Lighting.cginc")

		uniform float4 _Color; // define shader property for shaders
		float4 _SecondTex_ST;			// For the Main Tex UV transform
		sampler2D _SecondTex;			// Texture used for the line
		

		//float4 _WavePosition[10];
		float4 _WavePosition;
		float _WaveRadius = -1;
		float _Scale;

		struct vertexInput {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};
		struct vertexOutput {
			float4 pos : SV_POSITION;
			float2	uv		: TEXCOORD0;	// fragment uv coordinate
			float4 col : COLOR;
		};

		vertexOutput vert(vertexInput v)
		{
			vertexOutput output;

			float amplitude = 0;
			/*int i;
			for (i = 0; i < 10; i++)
			{
				if (_WavePosition[i].w > 0)
				{
					float dist = distance(v.vertex, _WavePosition[i].xyz);
					dist = abs(dist - _WavePosition[i].w); //this math is fucking dumb
					dist = clamp(dist, 0, .1) * 1 / .1;
					amplitude += pow(1 - dist, 1) * .1 * _Scale;
				}
			}*/

			if (_WavePosition.w > 0)
			{
				float dist = distance(v.vertex, _WavePosition.xyz);
				dist = abs(dist - _WavePosition.w); //this math is fucking dumb
				dist = clamp(dist, 0, .1) * 1 / .1;
				amplitude += pow(1 - dist, 1) * .1 * _Scale;
			}

			v.vertex = float4(v.vertex + (v.normal * amplitude), 1);

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 normalDirection = normalize(
				mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
			float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

			float3 diffuseReflection = _LightColor0.rgb * _Color.rgb
				* max(0.0, dot(normalDirection, lightDirection));

			output.col = float4(diffuseReflection, 1.0);
			output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			output.uv = TRANSFORM_TEX(v.uv, _SecondTex);//v.texcoord;

			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			return input.col * tex2D(_SecondTex, input.uv);;
		}

			ENDCG
		}
			/*Pass
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

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

				float4 _WavePosition = (1,1,1,1);
				float _WaveRadius = -1;

				// Vertex Shader
				UCLAGL_v2g vert(appdata_base v)
				{
					UCLAGL_v2g output;

					float amplitude = 0;

					if (_WaveRadius > 0)
					{
						float dist = distance(v.vertex, _WavePosition);
						dist = abs(dist - _WaveRadius); //this math is fucking dumb
						dist = clamp(dist, 0, .5) * 1/.5;
						amplitude += pow(1 - dist, 2) * .2;
					}

					v.vertex = float4(v.vertex + (v.normal * amplitude),1);

					output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					output.uv = TRANSFORM_TEX(v.texcoord, _MainTex);//v.texcoord;

					return output;
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
		}*/

			
	} 

}
