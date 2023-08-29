// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//vertex and fragment shader
#ifndef MK_GLASS_FORWARD
	#define MK_GLASS_FORWARD

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputForward vertfwd (VertexInputForward v)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		VertexOutputForward o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputForward, o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		//vertex positions
		#if MKGLASS_LIT
			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.pos =  UnityObjectToClipPos(v.vertex);
		#else
			o.pos =  UnityObjectToClipPos(v.vertex);
		#endif
		//texcoords
		#ifdef MKGLASS_TC
			o.uv_Main.xy = TRANSFORM_TEX(v.texcoord0, _MainTex);
		#endif
		//normal binormal tangent
		#ifdef MKGLASS_TBN
			//create tangent space matrix
			o.tangentWorld.xyz = normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz, 0.0)).xyz);
			o.normalWorld.xyz = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
			o.binormalWorld.xyz= normalize(cross(o.normalWorld.xyz, o.tangentWorld.xyz) * v.tangent.w);
		#elif MKGLASS_WN
			//if tangent space matrix not needed calculate only world normal
			o.normalWorld.xyz = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
		#endif
		#ifdef MKGLASS_VERTCLR
			o.color = v.color;
		#endif

		#if MKGLASS_LIT
			#ifdef MK_GLASS_FWD_BASE_PASS
				//lightmaps and ambient
				#ifdef DYNAMICLIGHTMAP_ON
					o.uv_Lm.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				#ifdef LIGHTMAP_ON
					o.uv_Lm.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				#ifdef MK_GLASS_FWD_BASE_PASS
					#ifndef LIGHTMAP_ON
						#if UNITY_SHOULD_SAMPLE_SH
						//unity ambient light per vertex
							o.aLight = ShadeSH9(half4(o.normalWorld.xyz,1.0));
						#else
							o.aLight = 0.0;
						#endif
						#ifdef VERTEXLIGHT_ON
							//vertexlight
							o.aLight += Shade4PointLights (
							unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
							unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
							unity_4LightAtten0, o.posWorld, 
							#ifdef MKGLASS_TBN
								o.normalWorld.xyz);
							#elif MKGLASS_WN
								o.normalWorld.xyz);
							#endif
						#endif
					#endif
				#endif
			#endif
		#endif

		//vertex shadow
		#if UNITY_VERSION >= 201810
			UNITY_TRANSFER_LIGHTING(o, v.texcoord0);
		#else
			UNITY_TRANSFER_SHADOW(o, v.texcoord0);
		#endif
		#if SHADER_TARGET >= 30
			//vertex fog
			UNITY_TRANSFER_FOG(o,o.pos);
		#endif

		float4 uvRefraction;

		uvRefraction = o.pos;

		#if MKGLASS_TBN
			o.uv_Main.zw = uvRefraction.xy;
			o.normalWorld.w = uvRefraction.z;
			o.tangentWorld.w = uvRefraction.w;
		#elif MKGLASS_WN
			o.uv_Refraction = uvRefraction;
		#endif

		#if MKGLASS_TC_D
			float2 uvDetail = TRANSFORM_TEX(v.texcoord0, _DetailAlbedoMap);
			#if MKGLASS_TBN
				o.posWorld.w = uvDetail.x;
				o.binormalWorld.w = uvDetail.y;
			#elif MKGLASS_WN
				o.uv_Detail = uvDetail;
			#endif
		#endif 

		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 fragfwd (VertexOutputForward o) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(o);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
		
		//init surface struct for rendering
		MKGlassSurface mkts = InitSurface(o);

		//apply lights, ambient and lightmap
		MKGlassLightLMCombined(mkts, o);

		//finalize the output
		MKGlassLightFinal(mkts, o);
		
		half fade;
		#ifdef DOUBLE_SIDED
			fade = 0.5;
		#else	
			fade = 0.625;
		#endif
		#if _MK_ALBEDO_MAP
			mkts.Color_Out.a = lerp(fade, 0.95, _MainTint);
		#else
			mkts.Color_Out.a = fade;
		#endif

		//if enabled add some fog - forward rendering only
		#if SHADER_TARGET >= 30
		UNITY_APPLY_FOG(o.fogCoord, mkts.Color_Out);
		#endif

		return mkts.Color_Out;
	}
#endif