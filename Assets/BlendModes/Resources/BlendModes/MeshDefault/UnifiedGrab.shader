Shader "BlendModes/MeshDefault/UnifiedGrab" 
{
	Properties 
	{
		_Color ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader 
	{
		Tags 
		{ 
			"Queue" = "Transparent" 
			"RenderType" = "Transparent" 
		}
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		
		GrabPass { "_BMMeshSharedGT" }
		
		Pass 
		{  
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#include "../BlendModes.cginc"
			
			#pragma multi_compile BMDarken BMMultiply BMColorBurn BMLinearBurn BMDarkerColor BMLighten BMScreen BMColorDodge BMLinearDodge BMLighterColor BMOverlay BMSoftLight BMHardLight BMVividLight BMLinearLight BMPinLight BMHardMix BMDifference BMExclusion BMSubtract BMDivide
			#pragma vertex ComputeVertex
			#pragma fragment ComputeFragment
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BMMeshSharedGT;

			struct VertexInput 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct VertexOutput 
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};
			
			VertexOutput ComputeVertex(VertexInput vertexInput)
			{
				VertexOutput vertexOutput;
				
				vertexOutput.vertex = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
				vertexOutput.screenPos = vertexOutput.vertex;
				vertexOutput.texcoord = TRANSFORM_TEX(vertexInput.texcoord, _MainTex);
				
				return vertexOutput;
			}
			
			fixed4 ComputeFragment(VertexOutput vertexOutput) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, vertexOutput.texcoord) * _Color;
				
				float2 grabTexcoord = vertexOutput.screenPos.xy / vertexOutput.screenPos.w; 
				grabTexcoord.x = (grabTexcoord.x + 1.0) * .5;
				grabTexcoord.y = (grabTexcoord.y + 1.0) * .5; 
				#if UNITY_UV_STARTS_AT_TOP
				grabTexcoord.y = 1.0 - grabTexcoord.y;
				#endif
				
				fixed4 grabColor = tex2D(_BMMeshSharedGT, grabTexcoord); 
				
				#include "../BlendOps.cginc"
			}
			
			ENDCG
		}
	}
	
	FallBack "Diffuse"
	CustomEditor "BMMaterialEditor"
}
