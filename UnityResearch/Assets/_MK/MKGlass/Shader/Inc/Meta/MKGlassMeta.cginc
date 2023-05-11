//meta vertex and fragment shader
#ifndef MK_GLASS_META
#define MK_GLASS_META

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputMeta metavert (VertexInputMeta v)
	{
		VertexOutputMeta o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputMeta, o);
		//vertexposition
		o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
		//texcoords
		o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
		#ifdef MKGLASS_VERTCLR
			o.color = v.color;
		#endif

		#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
		#else
		float scale = 1.0;
		#endif
		o.uv_Refraction.xy = (float2(o.pos.x, o.pos.y*scale) + o.pos.w) * 0.5;
		o.uv_Refraction.zw = o.pos.zw;

		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 metafrag (VertexOutputMeta o) : SV_Target
	{
		UnityMetaInput umi;
		half alpha;
		UNITY_INITIALIZE_OUTPUT(UnityMetaInput, umi);

		//modified meta albedo
		#if MKGLASS_TEXCLR
			SurfaceAlbedo(umi.Albedo, alpha, o.uv);
		#elif MKGLASS_VERTCLR
			SurfaceAlbedo(umi.Albedo, alpha, o.color);
		#endif

		//apply emission color
		#if MKGLASS_LIT
			#if _MK_EMISSION_DEFAULT
				umi.Emission = _EmissionColor * umi.Albedo;
			#elif _MK_EMISSION_MAP
				umi.Emission = _EmissionColor * tex2D(_EmissionMap, o.uv).rgb * umi.Albedo;
			#endif
		#endif

		//unity meta macro to apply gi
		return UnityMetaFragment(umi);
	}

#endif