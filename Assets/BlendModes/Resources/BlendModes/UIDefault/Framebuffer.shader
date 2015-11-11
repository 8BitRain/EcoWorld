Shader "BlendModes/UIDefault/Framebuffer"
{
	Properties
	{
		[PerRendererData] 
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"			
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#include "../BlendModes.cginc"
			
			#pragma multi_compile BMDarken BMMultiply BMColorBurn BMLinearBurn BMDarkerColor BMLighten BMScreen BMColorDodge BMLinearDodge BMLighterColor BMOverlay BMSoftLight BMHardLight BMVividLight BMLinearLight BMPinLight BMHardMix BMDifference BMExclusion BMSubtract BMDivide
			#pragma vertex ComputeVertex
			#pragma fragment ComputeFragment
			
			sampler2D _MainTex;
			fixed4 _Color;
			
			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};
			
			VertexOutput ComputeVertex (VertexInput vertexInput)
			{
				VertexOutput vertexOutput;
				
				vertexOutput.vertex = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
				vertexOutput.texcoord = vertexInput.texcoord;
				#ifdef UNITY_HALF_TEXEL_OFFSET
				vertexOutput.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1.0, 1.0);
				#endif
				vertexOutput.color = vertexInput.color * _Color;
							
				return vertexOutput;
			}
			
			fixed4 ComputeFragment (VertexOutput vertexOutput
				#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
				, inout fixed4 fetchColor : COLOR1
				#endif
				) : SV_Target
			{
				half4 color = tex2D(_MainTex, vertexOutput.texcoord) * vertexOutput.color;
				clip(color.a - .01);
				
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

	Fallback "UI/Default"
	CustomEditor "BMMaterialEditor"
}
