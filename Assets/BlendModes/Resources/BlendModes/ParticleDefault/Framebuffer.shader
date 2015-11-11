Shader "BlendModes/ParticleDefault/Framebuffer" 
{
	Properties 
	{
		_Color ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category 
	{
		Tags 
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}
		
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off 
		Lighting Off 
		ZWrite Off 
		Fog { Color (0,0,0,0) }
		Blend SrcAlpha OneMinusSrcAlpha
		
		SubShader 
		{
			Pass 
			{
				CGPROGRAM
			
				#include "UnityCG.cginc"
				#include "../BlendModes.cginc"
				
				#pragma multi_compile BMDarken BMMultiply BMColorBurn BMLinearBurn BMDarkerColor BMLighten BMScreen BMColorDodge BMLinearDodge BMLighterColor BMOverlay BMSoftLight BMHardLight BMVividLight BMLinearLight BMPinLight BMHardMix BMDifference BMExclusion BMSubtract BMDivide
				#pragma vertex ComputeVertex
				#pragma fragment ComputeFragment

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				
				struct VertexInput 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct VertexOutput 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				VertexOutput ComputeVertex(VertexInput vertexInput)
				{
					VertexOutput vertexOutput;
					
					vertexOutput.vertex = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
					vertexOutput.color = vertexInput.color;
					vertexOutput.texcoord = TRANSFORM_TEX(vertexInput.texcoord, _MainTex);
					
					return vertexOutput;
				}
				
				fixed4 ComputeFragment (VertexOutput vertexOutput
					#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
					, inout fixed4 fetchColor : COLOR1
					#endif
					) : SV_Target
				{
					fixed4 color = tex2D(_MainTex, vertexOutput.texcoord) * vertexOutput.color * _Color;
					
					#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
					fixed4 grabColor = fetchColor;
					#else
					fixed4 grabColor = fixed4(1, 1, 1, 1);
					#endif
					
					#include "../BlendOps.cginc"
				}
				
				ENDCG 
			}
		}	
	}
	
	FallBack "Particles/Additive"
	CustomEditor "BMMaterialEditor"
}
