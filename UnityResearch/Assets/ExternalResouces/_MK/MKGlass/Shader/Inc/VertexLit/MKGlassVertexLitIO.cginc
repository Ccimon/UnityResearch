//Vertexshader Input and Output
#ifndef MK_GLASS_IO_VERTEX_LIT
	#define MK_GLASS_VERTEX_LIT

	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	/////////////////////////////////////////////////////////////////////////////////////////////
	// INPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputVertexLit
	{
		#if MKGLASS_VERTCLR
			//use vertexcolors if enabled
			half4 color : COLOR;
		#endif
		//Just use very basic input
		float4 vertex : POSITION;
		#if MKGLASS_TC
			float4 texcoord : TEXCOORD0;
		#endif
		#if _MK_VERTEXLMM_LIT || _MK_VERTEXLMRGBM_LIT
			float4 texcoord1 : TEXCOORD1;
		#endif
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputVertexLit
	{
		#ifdef MKGLASS_VERTCLR
			half4 color : COLOR0;
		#endif
		float4 pos : SV_POSITION;
		#if MKGLASS_TC
			float2 uv_Main : TEXCOORD0;
		#endif
		#if _MK_VERTEXLMM_LIT || _MK_VERTEXLMRGBM_LIT
			float2 uv_Lm : TEXCOORD1;
		#endif
		float4 uv_Refraction : TEXCOORD5;
		UNITY_FOG_COORDS(6)
		UNITY_VERTEX_OUTPUT_STEREO
	};
#endif