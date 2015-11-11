// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "BlendModes/MeshBumpGlass/Framebuffer" {
Properties {
	_BumpAmt  ("Distortion", range (0,128)) = 10
	_MainTex ("Tint Color (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "Queue"="Transparent" "RenderType"="Opaque" }


	SubShader {
 		
 		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

#include "../BlendModes.cginc"
#pragma multi_compile BMDarken BMMultiply BMColorBurn BMLinearBurn BMDarkerColor BMLighten BMScreen BMColorDodge BMLinearDodge BMLighterColor BMOverlay BMSoftLight BMHardLight BMVividLight BMLinearLight BMPinLight BMHardMix BMDifference BMExclusion BMSubtract BMDivide

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
};

float _BumpAmt;
float4 _BumpMap_ST;
float4 _MainTex_ST;

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.zw;
	o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	return o;
}

sampler2D _BumpMap;
sampler2D _MainTex;

half4 frag( v2f i 
#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
				, inout fixed4 fetchColor : COLOR1
#endif
) : COLOR
{
	// calculate perturbed coordinates
	half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; // we could optimize this by just reading the x & y without reconstructing the Z
	float2 offset = bump * _BumpAmt;
	i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	
	half4 tint = tex2D( _MainTex, i.uvmain );
	
	#ifdef UNITY_FRAMEBUFFER_FETCH_AVAILABLE
	fixed4 grabColor = fetchColor;
	#else
	fixed4 grabColor = fixed4(1, 1, 1, 1);
	#endif
	fixed4 color = tint;
	#include "../BlendOps.cginc"
}
ENDCG
		}
	}

	// ------------------------------------------------------------------
	// Fallback for older cards and Unity non-Pro
	
	SubShader {
		Blend DstColor Zero
		Pass {
			Name "BASE"
			SetTexture [_MainTex] {	combine texture }
		}
	}
}
	CustomEditor "BMMaterialEditor"
}
