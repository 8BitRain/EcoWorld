Shader "BlendModes/SpriteDefault/Framebuffer"
{
	Properties
	{
		[PerRendererData] 
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		[MaterialToggle] 
		PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			
			#include "UnityCG.cginc"
			#include "../BlendModes.cginc"
			
			#pragma multi_compile BMDarken BMMultiply BMColorBurn BMLinearBurn BMDarkerColor BMLighten BMScreen BMColorDodge BMLinearDodge BMLighterColor BMOverlay BMSoftLight BMHardLight BMVividLight BMLinearLight BMPinLight BMHardMix BMDifference BMExclusion BMSubtract BMDivide
			#pragma multi_compile DUMMY PIXELSNAP_ON
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
				vertexOutput.color = vertexInput.color * _Color;
				#ifdef PIXELSNAP_ON
				vertexOutput.vertex = UnityPixelSnap(vertexOutput.vertex);
				#endif
							
				return vertexOutput;
			}
			
			fixed4 ComputeFragment (VertexOutput vertexOutput
				#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
				, inout fixed4 fetchColor : COLOR1
				#endif
				) : SV_Target
			{
				half4 color = tex2D(_MainTex, vertexOutput.texcoord) * vertexOutput.color;
				//color.rgb *= color.a;
				
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
	
	Fallback "Sprites/Default"
	CustomEditor "BMMaterialEditor"
}
