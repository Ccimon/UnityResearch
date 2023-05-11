//include file for important calculations during rendering
#ifndef MK_GLASS_INC
	#define MK_GLASS_INC

	#include "../Common/MKGlassDef.cginc"

	/////////////////////////////////////////////////////////////////////////////////////////////
	// INC
	/////////////////////////////////////////////////////////////////////////////////////////////

	inline float4 ComputeNDC(float4 positionClip) 
	{
		float4 ndc;

		#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
		#else
			float scale = 1.0;
		#endif

		ndc = positionClip * 0.5;
		ndc.xy = float2(ndc.x, ndc.y * scale) + ndc.w;
    	ndc.zw = positionClip.zw;

		#if defined(UNITY_SINGLE_PASS_STEREO)
			ndc.xy = TransformStereoScreenSpaceTex(ndc.xy, ndc.w);
		#endif

		ndc.xyz = ndc.xyz / (ndc.w + 0.00001);

		return ndc;
	}

	//normal in world space
	#ifdef MKGLASS_TBN
	inline half3 WorldNormal(half3 encodedNormal, half bumpiness, half3x3 tbn)
	{
		encodedNormal.xy *= bumpiness;
		//local.z = sqrt(1.0 - dot(local, local));
		#if !defined(UNITY_NO_DXT5nm)
			encodedNormal.z = 1.0 - 0.5 * dot(encodedNormal.xy, encodedNormal.xy); //approximation
		#endif
		return normalize(mul(encodedNormal, tbn));
	}
	#endif

	inline half3 EncodeNormalMap(sampler2D normalMap, float2 uv)
	{
		half4 encode = tex2D(normalMap, uv);
		#if defined(UNITY_NO_DXT5nm)
			return encode.rgb * 2.0 - 1.0;
		#else
			encode.r *= encode.a;
			return half3(2.0 * encode.a - 1.0, 2.0 * encode.g - 1.0, 0.0);
		#endif
	}

	#if _MK_LIGHTMODEL_BLINN_PHONG
		//specular blinn phong
		inline half GetSpecular(half ndhv, half shine)
		{
			//doublesided spec
			return pow(ndhv, shine * SHINE_MULT);
		}
	#elif _MK_LIGHTMODEL_PHONG
		//specular phong
		inline half GetSpecular(half shine, half mlrefndotv)
		{
			//doublesided spec
			return pow(mlrefndotv, shine * SHINE_MULT);
		}
	#endif

	//translucency
	inline half GetTranslucency(half mldotv, half3 normal, half shine)
	{
		return pow (mldotv, shine * SHINE_MULT);
	}

	//Rim with smooth interpolation
	inline half3 RimDefault(half size, half vdn, half3 col, half intensity)
	{
		half r = pow ((1.0 - saturate(vdn)), size);
		return r * intensity * col.rgb * step(0.01, vdn);
	}

	#if _MKGLASS_REFLECTIVE
		//get environmentcube
		inline float3 BoxProjection (
			float3 dir, float3 posW,
			float4 cubePos, float3 boxMin, float3 boxMax
			) 
		{
			#if UNITY_SPECCUBE_BOX_PROJECTION
				UNITY_BRANCH
				if (cubePos.w > 0) 
				{
					float3 factors = ((dir > 0 ? boxMax : boxMin) - posW) / dir;
					float scalar = min(min(factors.x, factors.y), factors.z);
					dir = dir * scalar + (posW - cubePos);
				}
			#endif
			return dir;
		}

		inline half3 GetReflection(half3 reflectionDir, half3 worldPos)
		{
			Unity_GlossyEnvironmentData environment;
			#if MKGLASS_DISTORTION
				environment.roughness = _Distortion * 0.1;
			#else
				environment.roughness = 0.0;
			#endif
			environment.reflUVW = BoxProjection(
				reflectionDir, worldPos,
				unity_SpecCube0_ProbePosition,
				unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax
			);
			half3 probe0 = Unity_GlossyEnvironment(
				UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, environment
			);
			environment.reflUVW = BoxProjection(
				reflectionDir, worldPos,
				unity_SpecCube1_ProbePosition,
				unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax
			);
			#if UNITY_SPECCUBE_BLENDING
				UNITY_BRANCH
				if (unity_SpecCube0_BoxMin.w < 0.99999) 
				{
					half3 probe1 = Unity_GlossyEnvironment(
						UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0),
						unity_SpecCube0_HDR, environment
					);
					probe0 = lerp(probe1, probe0, unity_SpecCube0_BoxMin.w);
				}
			#endif
			return probe0;
		}
	#endif
#endif