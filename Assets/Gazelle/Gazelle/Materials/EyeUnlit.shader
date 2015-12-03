Shader "Custom/Blink" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Blink("Blink (RGB)", 2D) = "white" {}
		_Clip("Clip",Range(0,1)) = 1
	}
	SubShader {
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}
		//LOD 200
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
				#pragma exclude_renderers ps3 xbox360
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				sampler2D _MainTex;
				sampler2D _Blink;
				fixed _Clip;
				
				struct vertexInput{
					float4 vertex : POSITION;
					float4 texCoord : TEXCOORD0;
					
				};
				struct vertexOutput{ 
					float4 pos: SV_POSITION;
					half2 uv: TEXCOORD0;
				};

				vertexOutput vert(vertexInput i){	//returns a fragmentInput
					vertexOutput o;
					o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
					o.uv = i.texCoord;
					return o;
				}
				half4 frag(vertexOutput i): COLOR{
					if(i.uv.y > _Clip){
						return tex2D(_MainTex,i.uv);
					}
					else{
						return tex2D(_Blink,i.uv);
					}
				}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
