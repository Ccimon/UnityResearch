//Light Calculations
#ifndef MK_GLASS_LIGHT
	#define MK_GLASS_LIGHT

	#include "../Common/MKGlassDef.cginc"

	inline half FastPow4(half v)
	{
		return v * v * v * v;
	}

	////////////
	// LIGHT
	////////////
	void MKGlassLightMain(inout MKGlassSurface mkts, in VertexOutputForward o)
	{
		#if MKGLASS_LIT
			half4 c;
			//diffuse lighting
			c.rgb = mkts.Color_Albedo;

			c.rgb = lerp(c.rgb, c.rgb * mkts.Pcp.NdotL * mkts.Pcp.LightColorXAttenuation, _ShadowIntensity);
			
			//Specular
			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				//halv vector or viewDir
				half spec;
				_Shininess *= mkts.Color_Specular.g;

				#if _MK_LIGHTMODEL_BLINN_PHONG
					spec = GetSpecular(mkts.Pcp.NdotHV, _Shininess);
				#elif _MK_LIGHTMODEL_PHONG
					spec = GetSpecular(_Shininess, mkts.Pcp.MLrefNdotV);
				#endif
				
				mkts.Color_Specular = spec;
				mkts.Color_Specular = mkts.Color_Specular * _SpecColor * (_SpecularIntensity *  mkts.Color_Specular.r);
			#endif

			//handle translucency
			#if MKGLASS_TLD || MKGLASS_TLM
				half3 lnd = mkts.Pcp.LightDirection + mkts.Pcp.NormalDirection * (_TranslucentShininess * mkts.Color_Translucent.g);
				half translucent = FastPow4(saturate(dot(mkts.Pcp.ViewDirection, -lnd))) * mkts.Pcp.DistanceAttenuation;
				mkts.Color_Translucent = translucent * _TranslucentColor.rgb * mkts.Pcp.LightColor * (_TranslucentIntensity * mkts.Color_Translucent.r);

				/*
				half translucent;
				translucent = GetTranslucency(mkts.Pcp.MLdotV, mkts.Pcp.NormalDirection, _TranslucentShininess);
				#ifdef USING_DIRECTIONAL_LIGHT
					_TranslucentIntensity *= T_H;
				#endif

				mkts.Color_Translucent = (translucent * _TranslucentColor * mkts.Pcp.LightColor * T_D * (_TranslucentIntensity * mkts.Color_Translucent.r));
				*/
			#endif

			#if _MKGLASS_REFLECTIVE
				//reflection map based lightintensity
				#if _MK_REFLECTIVE_MAP
					_ReflectIntensity *= tex2D(_ReflectMap, o.uv_Main).r;
				#endif
				//basic reflection
				#if _MK_REFLECTIVE_FRESNEL
					c.rgb = lerp(c.rgb, lerp(mkts.Color_Reflect, c.rgb, pow(mkts.Pcp.VdotN, _FresnelFactor)), _ReflectIntensity);
				#else
					c.rgb = lerp(c.rgb, mkts.Color_Reflect, _ReflectIntensity);
				#endif
			#endif
			//apply lightcolor
			//c.rgb *= _LightColor0.rgb;
			#if MKGLASS_TLD || MKGLASS_TLM
				//apply translucency
				c.rgb += mkts.Color_Translucent * mkts.Color_Albedo;
			#endif

			#if _MK_LIGHTMODEL_PHONG || _MK_LIGHTMODEL_BLINN_PHONG
				//apply specular
				c.rgb += mkts.Color_Specular * mkts.Pcp.LightColorXAttenuation;
			#endif

			//add occlusion
			#if MKGLASS_OCCLUSION
				c.rgb *= mkts.Occlusion;
			#endif

			//apply alpha
			c.a = mkts.Alpha;

			mkts.Color_Out = c;
		#else
			//correct blending on unlit && fwd add pass
			//TODO skip albedo calculation
			#if MK_GLASS_FWD_ADD_PASS
				mkts.Color_Albedo.rgb = 0.0;
			#endif
			//non lit color output
			mkts.Color_Out = half4(mkts.Color_Albedo.rgb, mkts.Color_Out.a);
		#endif
	}

	void MKGlassLightLMCombined(inout MKGlassSurface mkts, in VertexOutputForward o)
	{
		//apply lighting to surface
		MKGlassLightMain(mkts, o);
		
		#if MKGLASS_LIT
			#ifdef MK_GLASS_FWD_BASE_PASS
				//add ambient light
				half3 amb = mkts.Color_Albedo * o.aLight;
				#if _MKGLASS_REFLECTIVE
					mkts.Color_Out.rgb = lerp(mkts.Color_Out.rgb + amb * _MainTint, mkts.Color_Out.rgb + amb * T_H, _ReflectIntensity);
				#else
					//mkts.Color_Out.rgb += amb;
				#endif
			#endif

			#ifdef MK_GLASS_FWD_BASE_PASS
				#if LIGHTMAP_ON || DYNAMICLIGHTMAP_ON
					half3 lm = 0;
					#ifdef LIGHTMAP_ON
						 half4 lmBCT = UNITY_SAMPLE_TEX2D(unity_Lightmap, o.uv_Lm.xy);
						 half3 bC = DecodeLightmap(lmBCT);
						 //handle directional lightmaps
						#if DIRLIGHTMAP_COMBINED
							// directional lightmaps
							half4 bDT = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, o.uv_Lm.xy);
							lm = DecodeDirectionalLightmap (bC, bDT, o.normalWorld);

							#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
								lm = SubtractMainLightWithRealtimeAttenuationFromLightmap (lm, mkts.Pcp.LightAttenuation, lmBCT, o.normalWorld);
							#endif
						//handle not directional lightmaps
						#else
							lm = bC;
							#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
								lm = SubtractMainLightWithRealtimeAttenuationFromLightmap(lm, mkts.Pcp.LightAttenuation, lmBCT, o.normalWorld);
							#endif
						#endif
					#endif

					//handle dynamic lightmaps
					#ifdef DYNAMICLIGHTMAP_ON
						half4 lmRTCT = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, o.uv_Lm.zw);
						half3 rTC = DecodeRealtimeLightmap (lmRTCT);

						#ifdef DIRLIGHTMAP_COMBINED
							half4 rDT = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, o.uv_Lm.zw);
							lm += DecodeDirectionalLightmap (rTC, rDT, o.normalWorld);
						#else
							lm += rTC;
						#endif
					#endif

					//add occlusion to lightmap
					#if MKGLASS_OCCLUSION
						lm *= mkts.Occlusion;
					#endif

					//apply lightmap
					mkts.Color_Out.rgb += mkts.Color_Albedo * lm;
				#endif
			#endif
		#endif
		
	}

	void MKGlassLightFinal(inout MKGlassSurface mkts, in VertexOutputForward o)
	{
		#if MKGLASS_LIT
			#if _MKGLASS_EMISSION
				#if _MKGLASS_RIM && MK_GLASS_FWD_BASE_PASS
					//apply rim lighting
					mkts.Color_Emission += RimDefault(_RimSize, mkts.Pcp.VdotN, _RimColor.rgb, _RimIntensity);
				#endif
				mkts.Color_Out.rgb += mkts.Color_Emission;
			#endif
		#else
			//return unlit color
			mkts.Color_Out = half4(mkts.Color_Out.rgb, mkts.Color_Out.a);
		#endif
	}
#endif