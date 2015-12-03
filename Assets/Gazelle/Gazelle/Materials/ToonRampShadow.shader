Shader "Custom/Toon Ramp Cast Shadow" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_SColor ("Shadow Color", Color) = (0.0,0.0,0.0,1)
		_LColor ("Highlight Color", Color) = (0.5,0.5,0.5,1)
		_Intensity("Intensity",Range(0,2)) = 1
	}
	SubShader {
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}
		//LOD 200
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
				#pragma exclude_renderers ps3 xbox360
				#pragma fragmentoption ARB_fog_exp2
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				sampler2D _MainTex;
				sampler2D _Ramp;
				fixed4 _LColor;
				fixed4 _Color;
				fixed4 _SColor;
				fixed _Intensity;
				struct vertexInput{
					float4 vertex : POSITION;
					float3 normal: NORMAL;
					float4 texCoord : TEXCOORD0;
					
				};
				struct vertexOutput{
					float4 pos: SV_POSITION;
					half2 uv: TEXCOORD0;
					half3 normal: TEXCOORD1;
				};

				vertexOutput vert(vertexInput i){	//returns a fragmentInput
					vertexOutput o;
					o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
					o.uv = i.texCoord.xy;
					o.normal = normalize(mul(float4(i.normal,1.0),_World2Object).xyz);
					return o;
				}
				half4 frag(vertexOutput i): COLOR{
					fixed3 lightDir = _WorldSpaceLightPos0.xyz;
					fixed NdotL = dot(i.normal,lightDir) *0.5;
					NdotL += 0.5;
					fixed3 ramp = tex2D(_Ramp, float2(NdotL,NdotL)).rgb;
					ramp = lerp(_SColor,_LColor,ramp) * _Intensity;
					fixed3 texcolor = tex2D(_MainTex,i.uv).rgb*ramp;
					return half4(texcolor,1) * _Color;
				}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
