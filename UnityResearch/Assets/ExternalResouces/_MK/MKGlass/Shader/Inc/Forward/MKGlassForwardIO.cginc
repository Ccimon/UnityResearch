// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

//Vertexshader Input and Output
#ifndef MK_GLASS_IO_FORWARD
	#define MK_GLASS_IO_FORWARD

	#include "UnityCG.cginc"
	#include "AutoLight.cginc"

/////////////////////////////////////////////////////////////////////////////////////////////
// INPUT
/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputForward
	{
		#if MKGLASS_VERTCLR
			//use vertexcolors if enabled
			half4 color : COLOR;
		#endif
		//vertex position - always needed
		float4 vertex : POSITION;
		#if MKGLASS_TC || MKGLASS_TC_D
			//texcoords0 if needed
			float4 texcoord0 : TEXCOORD0;
		#endif
		#if MK_GLASS_FWD_BASE_PASS
			//ambient and lightmap0 texcoords
			#if UNITY_SHOULD_SAMPLE_SH || LIGHTMAP_ON
				float4 texcoord1 : TEXCOORD1;
			#endif
			#if DYNAMICLIGHTMAP_ON
				//dynammic lightmap texcoords
				float4 texcoord2 : TEXCOORD2;
			#endif
		#endif
		//use normals light is enabled
		half3 normal : NORMAL;
		//use tangents for tspace matrix calculation
		#if MKGLASS_TBN || MKGLASS_WN
			half4 tangent : TANGENT;
		#endif
		
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputForward
	{
		float4 pos : SV_POSITION;
		#if MKGLASS_TBN
			float4 uv_Main : TEXCOORD0;
		#elif MKGLASS_TC
			float2 uv_Main : TEXCOORD0;
		#endif

		#ifdef MKGLASS_VERTCLR
			half4 color : COLOR0;
		#endif

		#if MKGLASS_TC_D && MKGLASS_TBN
			float4 posWorld : TEXCOORD1;
		#else
			float3 posWorld : TEXCOORD1;
		#endif

		#ifdef MKGLASS_TBN
			float4 normalWorld : TEXCOORD2;
			float4 tangentWorld : TEXCOORD3;
			#if MKGLASS_TC_D
				float4 binormalWorld : TEXCOORD4;
			#else
				half3 binormalWorld : TEXCOORD4;
			#endif
		#elif MKGLASS_WN
			half3 normalWorld : TEXCOORD2;
			half4 uv_Refraction : TEXCOORD3;
			#if MKGLASS_TC_D
				float2 uv_Detail : TEXCOORD4;
			#endif
		#endif
				
		#if MKGLASS_LIT
			#ifdef MK_GLASS_FWD_BASE_PASS
				half3 aLight : COLOR1;
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
					float4 uv_Lm : TEXCOORD5;
				#endif
			#endif
		#endif

		#if UNITY_VERSION >= 201810
			UNITY_LIGHTING_COORDS(6,7)
		#else
			UNITY_SHADOW_COORDS(6)
		#endif
		#if SHADER_TARGET >= 30
    		UNITY_FOG_COORDS(8)
		#endif
		
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};
#endif