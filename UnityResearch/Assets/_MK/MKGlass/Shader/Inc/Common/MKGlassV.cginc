// Upgrade NOTE: upgraded instancing buffer 'MK_GLASS_PROPERTIES' to new syntax.

//uniform variables
#ifndef MK_GLASS_V
	#define MK_GLASS_V

	/////////////////////////////////////////////////////////////////////////////////////////////
	// UNIFORM VARIABLES
	/////////////////////////////////////////////////////////////////////////////////////////////

	//enabled uniform variables only if needed
	//UNITY_INSTANCING_BUFFER_START(MK_GLASS_PROPERTIES)
          
    //UNITY_INSTANCING_BUFFER_END(MK_GLASS_PROPERTIES)

	//Main
	half _IndexOfRefraction;
	#ifndef UNITY_STANDARD_INPUT_INCLUDED
		uniform half3 _Color;
	#endif
	#ifdef MKGLASS_TEXCLR
		#ifndef UNITY_STANDARD_INPUT_INCLUDED
			uniform sampler2D _MainTex;
		#endif
	#endif
	uniform half _MainTint;
	#if MKGLASS_TC || _MK_VERTEXNLM_LIT
		#ifndef UNITY_STANDARD_INPUT_INCLUDED
			uniform float4 _MainTex_ST;
		#endif
	#endif

	//Detail
	#if _MK_DETAIL_MAP || _MK_DETAIL_BUMP_MAP
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform half _DetailTint;
		uniform half3 _DetailColor;
	#endif

	#if _MK_DETAIL_BUMP_MAP
		uniform half _DetailNormalMapScale;
		uniform sampler2D _DetailNormalMap;
	#endif

	//Normalmap
	#if _MK_BUMP_MAP
		uniform sampler2D _BumpMap;
		uniform half _BumpScale;
	#endif

	#if MKGLASS_OCCLUSION
		uniform sampler2D _OcclusionMap;
		uniform half _OcclusionStrength;
	#endif

	#if MKGLASS_DISTORTION
		uniform half _Distortion;
	#endif

	//Light
	#if MKGLASS_LIT
		#ifndef UNITY_LIGHTING_COMMON_INCLUDED
			uniform half4 _LightColor0;
		#endif
	#endif

	//Rim
	#if _MKGLASS_RIM && MK_GLASS_FWD_BASE_PASS
		uniform half3 _RimColor;
		uniform half _RimSize;
		uniform half _RimIntensity;
	#endif

	//Specular
	#if MKGLASS_LIT
		#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
			#if _MK_SPECULAR_MAP
				uniform sampler2D _SpecGlossMap;
			#endif
			uniform half _Shininess;
			#ifndef UNITY_LIGHTING_COMMON_INCLUDED
				uniform half3 _SpecColor;
			#endif
			uniform half _SpecularIntensity;
		#endif
	#endif

	//Reflection
	#if MKGLASS_LIT
		#if _MK_REFLECTIVE_MAP
			uniform sampler2D _ReflectMap;
		#endif
		#if _MKGLASS_REFLECTIVE
			uniform half3 _ReflectColor;
			uniform half _ReflectIntensity;
			#if _MK_REFLECTIVE_FRESNEL
				uniform half _FresnelFactor;
			#endif
		#endif
	#endif

	//Translucent
	#if MKGLASS_LIT
		#if MKGLASS_TLD || MKGLASS_TLM
			uniform half3 _TranslucentColor;
			uniform half _TranslucentIntensity;
			uniform half _TranslucentShininess;
		#endif
		#if MKGLASS_TLM
			uniform sampler2D _TranslucentMap;
		#endif
	#endif

	//Emission
	#if MKGLASS_LIT || MK_GLASS_META_PASS
		#if _MKGLASS_EMISSION
			uniform half3 _EmissionColor;
		#endif
		#if _MK_EMISSION_MAP
			uniform sampler2D _EmissionMap;
		#endif
	#endif

	//Other
	#if MKGLASS_LIT
		#if MK_GLASS_SHADOWCASTER_PASS
			uniform sampler3D _DitherMaskLOD;
		#endif
		uniform half _ShadowIntensity;
	#endif
	
	#if FAST_MODE
		UNITY_DECLARE_SCREENSPACE_TEXTURE(_MKGlassRefraction);
		//uniform sampler2D _MKGlassRefraction;
		#if MKGLASS_DISTORTION
			uniform half4 _MKGlassRefraction_TexelSize;
		#endif
	#else
		UNITY_DECLARE_SCREENSPACE_TEXTURE(_GrabTexture);
		//uniform sampler2D _GrabTexture;
		#if MKGLASS_DISTORTION
			uniform half4 _GrabTexture_TexelSize;
		#endif
	#endif
#endif