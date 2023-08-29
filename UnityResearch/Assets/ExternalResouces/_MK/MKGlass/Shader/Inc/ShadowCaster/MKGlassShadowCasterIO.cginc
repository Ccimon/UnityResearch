//shadow input and output
#ifndef MK_GLASS_SHADOWCASTER_IO
	#define MK_GLASS_SHADOWCASTER_IO

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputShadowCaster
	{
		float4 vertex : POSITION;
		//use normals for cubemapped shadows (point lights)
		//#ifndef SHADOWS_CUBE
		float3 normal : NORMAL;
		//#endif
		//texcoords0 if needed
		float2 texcoord0 : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// OUTPUT
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexOutputShadowCaster
	{	
		V2F_SHADOW_CASTER_NOPOS
		//float3 sv : TEXCOORD0;
		float2 uv : TEXCOORD7;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	#ifdef UNITY_STEREO_INSTANCING_ENABLED
	struct VertexOutputStereoShadowCaster
	{
		UNITY_VERTEX_OUTPUT_STEREO
	};
	#endif
#endif