//Basic definitions for the rendering
#ifndef MK_GLASS_DEF
	#define MK_GLASS_DEF
	/////////////////////////////////////////////////////////////////////////////////////////////
	// DEF
	/////////////////////////////////////////////////////////////////////////////////////////////
	#ifndef T_H
		#define T_H 0.25
	#endif
	#ifndef T_V
		#define T_V 0.5
	#endif
	#ifndef T_O
		#define T_O 1.0
	#endif
	#ifndef T_G
		#define T_G 1.75
	#endif
	#ifndef T_D
		#define T_D 2.0
	#endif
	#ifndef T_T
		#define T_T 10.0
	#endif

	#ifndef SHINE_MULT
		#define SHINE_MULT 128
	#endif

	#ifndef SHINE_MULTXX
		#define SHINE_MULTXX 512
	#endif

	//Handle Vertex lit platforms
	#if _MK_VERTEXNLM_LIT || _MK_VERTEXLM_LIT || _MK_VERTEXLMRGBM_LIT
		#ifndef _MK_VERTEX_LIT_ENABLED
			#define _MK_VERTEX_LIT_ENABLED 1
		#endif
	#endif

	//Lit
	#if _MK_LIGHTMODEL_LAMBERT || _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
		#ifndef MKGLASS_LIT
			#define MKGLASS_LIT 1
		#endif
	#else
		#ifndef MKGLASS_UNLIT
			#define MKGLASS_UNLIT 1
		#endif
	#endif

	#if _MK_BUMP_MAP || _MK_DETAIL_BUMP_MAP
		#ifndef MKGLASS_DISTORTION
			#define MKGLASS_DISTORTION 1
		#endif
	#endif

	//Rim
	#if MKGLASS_LIT && MK_GLASS_FWD_BASE_PASS && !defined(DOUBLE_SIDED_BACK)
		#if _MK_RIM
			#ifndef _MKGLASS_RIM
				#define _MKGLASS_RIM 1
			#endif
		#endif
	#endif

	//Emission
	#if MKGLASS_LIT
		#if (_MK_EMISSION_DEFAULT || _MK_EMISSION_MAP || _MKGLASS_RIM) && (MK_GLASS_META_PASS || MK_GLASS_FWD_BASE_PASS)
			#ifndef _MKGLASS_EMISSION
				#define _MKGLASS_EMISSION 1
			#endif
		#endif
	#endif

	//Reflective
	#if (_MK_REFLECTIVE_MAP || _MK_REFLECTIVE_DEFAULT) && MK_GLASS_FWD_BASE_PASS
		#ifndef _MKGLASS_REFLECTIVE
			#define _MKGLASS_REFLECTIVE 1
		#endif
	#endif

	//Translucent
	#if _MK_TRANSLUCENT_DEFAULT && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)
		#ifndef MKGLASS_TLD
			#define MKGLASS_TLD 1
		#endif
	#elif _MK_TRANSLUCENT_MAP && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)
		#ifndef MKGLASS_TLM
			#define MKGLASS_TLM 1
		#endif
	#endif

	//Color src
	#if _MK_ALBEDO_MAP || _MK_DETAIL_MAP
		#ifndef MKGLASS_TEXCLR
			#define MKGLASS_TEXCLR 1
		#endif
	#else
		#ifndef MKGLASS_VERTCLR
			#define MKGLASS_VERTCLR 1
		#endif
	#endif

	//Occlusion
	#if MKGLASS_LIT
		#if _MK_OCCLUSION
			#ifndef MKGLASS_OCCLUSION
				#define MKGLASS_OCCLUSION 1
			#endif
		#endif
	#endif
	//Texcoords
	#if MKGLASS_LIT || MKGLASS_TEXCLR || (_MK_SPECULAR_MAP && (_MK_LIGHTMODEL_BLINN_PHONG || _MK_LIGHTMODEL_PHONG)) || _MK_BUMP_MAP || MKGLASS_TLD || MKGLASS_TLM || _MK_REFLECTIVE_MAP || _MK_EMISSION_MAP || _MK_ALBEDO_MAP || MKGLASS_OCCLUSION || MKGLASS_DISTORTION
		#ifndef MKGLASS_TC
			#define MKGLASS_TC 1
		#endif
	#endif

	//Texcoord detail
	#if _MK_DETAIL_MAP || _MK_DETAIL_BUMP_MAP
		#ifndef MKGLASS_TC_D
			#define MKGLASS_TC_D 1
		#endif
	#endif

	//Normals
	#if DIRLIGHTMAP_COMBINED || UNITY_SHOULD_SAMPLE_SH || MKGLASS_DISTORTION
		#ifndef	MKGLASS_TBN	
			#define MKGLASS_TBN 1
		#endif
	#else
		#ifndef MKGLASS_WN
			#define MKGLASS_WN 1
		#endif
	#endif

	//VD
	#if defined(_MK_INDEX_OF_REFRACTION) || DIRLIGHTMAP_COMBINED || _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG || _MKGLASS_RIM || _MKGLASS_REFLECTIVE || MKGLASS_TLM
		#ifndef MKGLASS_VD
			#define MKGLASS_VD 1
		#endif
	#endif

	//Pcp enable
	#if MKGLASS_LIT
		#if _MKGLASS_RIM
			#ifndef MKGLASS_V_DOT_N
				#define MKGLASS_V_DOT_N 1
			#endif
		#endif
		#if MKGLASS_LIT
			#ifndef MKGLASS_N_DOT_L
				#define MKGLASS_N_DOT_L 1
			#endif
		#endif
		#if _MK_LIGHTMODEL_BLINN_PHONG
			#ifndef MKGLASS_HV
				#define MKGLASS_HV 1
			#endif
			#ifndef MKGLASS_N_DOT_HV
				#define MKGLASS_N_DOT_HV 1 
			#endif
		#endif
		#if _MK_LIGHTMODEL_PHONG
			#ifndef MKGLASS_ML_REF_N
				#define MKGLASS_ML_REF_N 1
			#endif
		#endif
		#if _MKGLASS_REFLECTIVE
			#ifndef MKGLASS_MV_REF_N
				#define MKGLASS_MV_REF_N 1
			#endif
			#if _MK_REFLECTIVE_FRESNEL
				#ifndef MKGLASS_V_DOT_N
					#define MKGLASS_V_DOT_N 1
				#endif
			#endif
		#endif
		#if MKGLASS_TLD || MKGLASS_TLM
			#ifndef MKGLASS_ML_DOT_V
				#define MKGLASS_ML_DOT_V 1
			#endif
		#endif
		#if _MK_LIGHTMODEL_PHONG
			#ifndef MKGLASS_ML_REF_N_DOT_V
				#define MKGLASS_ML_REF_N_DOT_V 1
			#endif
		#endif
	#endif

	#if defined(MKGLASS_VD) || MKGLASS_UNLIT || (MKGLASS_ML_REF_N_DOT_V || MKGLASS_ML_DOT_V || MKGLASS_MV_REF_N || MKGLASS_ML_REF_N || MKGLASS_N_DOT_HV || MKGLASS_HV || MKGLASS_N_DOT_L || MKGLASS_V_DOT_L || MKGLASS_V_DOT_N) || MKGLASS_LIT || MKGLASS_TC_D
		#ifndef MKGLASS_PRECALC
			#define MKGLASS_PRECALC 1
		#endif
	#endif
#endif