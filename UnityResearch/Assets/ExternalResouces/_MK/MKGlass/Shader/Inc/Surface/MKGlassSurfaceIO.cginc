//surface input and output
#ifndef MK_GLASS_SURFACE_IO
	#define MK_GLASS_SURFACE_IO

	/////////////////////////////////////////////////////////////////////////////////////////////
	// MKGLASS SURFACE
	/////////////////////////////////////////////////////////////////////////////////////////////

	//Dynamic precalc struct
	#if MKGLASS_PRECALC
	struct MKGlassPCP
	{
		#if MKGLASS_V_DOT_N
			half VdotN;
		#endif
		#if MKGLASS_V_DOT_L
			half VdotL;
		#endif
		#if MKGLASS_N_DOT_L
			half NdotL;
		#endif
		#if MKGLASS_HV
			half3 HV;
		#endif
		#if MKGLASS_N_DOT_HV
			half NdotHV;
		#endif
		#if MKGLASS_ML_REF_N
			half3 MLrefN;
		#endif
		#if MKGLASS_ML_DOT_V
			half MLdotV;
		#endif
		#if MKGLASS_ML_REF_N_DOT_V
			half MLrefNdotV;
		#endif
		float4 UvRefraction;
		#if MKGLASS_TC_D
			float2 UvDetail;
		#endif
		half3 NormalDirection;
		#if _MK_DETAIL_BUMP_MAP
			half3 DetailNormalDirection;
			half3 RDetailNormalDirection;
		#endif
		half3 RNormalDirection;
		#if MKGLASS_LIT
			half3 LightDirection;
			half3 LightColor;
			half3 LightColorXAttenuation;
			half LightAttenuation;
			#if MKGLASS_TLD || MKGLASS_TLM
				half DistanceAttenuation;
			#endif
			#if MKGLASS_MV_REF_N
				half3 MVrefN;
			#endif
		#endif
		#if MKGLASS_VD
			half3 ViewDirection;
		#endif
	};
	#endif

	//dynamic surface struct
	struct MKGlassSurface
	{
		#if MKGLASS_PRECALC
			MKGlassPCP Pcp;
		#endif
		half4 Color_Out;
		half3 Color_Albedo;
		#if _MK_DETAIL_MAP
			half3 Color_DetailAlbedo;
		#endif
		#if MKGLASS_OCCLUSION
			half Occlusion;
		#endif
		half3 Color_Refraction;
		half Alpha;
		#if MKGLASS_LIT
			#if _MKGLASS_EMISSION
				half3 Color_Emission;
			#endif
			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				half3 Color_Specular;
			#endif
			#if MKGLASS_TLD || MKGLASS_TLM
				half3 Color_Translucent;
			#endif
			#if _MKGLASS_REFLECTIVE
				half3 Color_Reflect;
			#endif
		#endif
	};
#endif