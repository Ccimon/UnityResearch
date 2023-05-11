//shadow rendering input and output
#ifndef MK_GLASS_SHADOWCASTER
	#define MK_GLASS_SHADOWCASTER

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	void vertShadowCaster (
		VertexInputShadowCaster v,
		 out VertexOutputShadowCaster o
		 #ifdef UNITY_STEREO_INSTANCING_ENABLED
			,out VertexOutputStereoShadowCaster os
		 #endif
		 ,out float4 pos : SV_POSITION
		)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(VertexOutputShadowCaster, o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		#ifdef UNITY_STEREO_INSTANCING_ENABLED
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(os);
		#endif
		#if defined(MKGLASS_TC)
			//texcoords using untiy macro
			o.uv = TRANSFORM_TEX(v.texcoord0, _MainTex);
		#endif
		
		/*
		#ifdef SHADOWS_CUBE //point light shadows
			pos = UnityObjectToClipPos(v.vertex);
			o.sv = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
		#else //other shadows
			//pos with unity macros
			pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);
			pos = UnityApplyLinearShadowBias(pos);
		#endif
		*/

		TRANSFER_SHADOW_CASTER_NOPOS(o, pos)
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 fragShadowCaster 
		(
			VertexOutputShadowCaster o
			#if UNITY_VERSION >= 20171
				,UNITY_POSITION(vpos)
			#else
				,UNITY_VPOS_TYPE vpos : VPOS
			#endif
		) : SV_Target
	{	
		UNITY_SETUP_INSTANCE_ID(o);
		//UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);

		#if MKGLASS_LIT
			half alpha = tex2D(_MainTex, o.uv).a * _ShadowIntensity;
			// dither mask alpha blending
			half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
			clip (alphaRef - 0.01);

			SHADOW_CASTER_FRAGMENT(o)
		#else
			return 0;
		#endif
	}			
#endif