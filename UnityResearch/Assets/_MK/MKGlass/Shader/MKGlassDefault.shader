Shader "MK/Glass/Default"
{
	Properties
	{
		[Toggle] _UseFastMode ("Skip multiple framebuffer grabbing", int) = 1

		//Main
		_Color ("Color", Color) = (1,1,1,0.1)
		_MainTex ("Color (RGB)", 2D) = "white" {}
		_MainTint("Main Tint", Range(0,2)) = 0.0
		[Toggle] _AlbedoMap ("Color source map", int) = 0

		//Detail
		_DetailAlbedoMap("Detail Albedo", 2D) = "white" {}
		_DetailTint("Detail Tint", Range (0, 2)) = 0.25
		_DetailNormalMapScale("Detail Normal Map Scale", Float) = 1.0
		_DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
		_DetailColor ("Detail Color", Color) = (1,1,1,1)

		//Normalmap
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpScale("Scale", Float) = 1
		_Distortion("Distortion", Range(0,1)) = 0.30
		_IndexOfRefraction ("", Range(0, 0.5)) = 0.0

		//Light
		[Enum(LightModel)] _LightModel ("Light Model", int) = 3
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		//Render
		[Enum(RenderMode)] _RenderMode ("Render Mode", int) = 0
		_ShadowIntensity ("Shadow Intensity", Range(0,1)) = 0.33

		//Rim
		[Toggle] _UseRim ("Rim", int) = 0
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimSize ("Rim Size", Range(0.0,5.0)) = 2.3
		_RimIntensity("Intensity", Range (0, 1)) = 0.3

		//Specular
		_Shininess ("Shininess",  Range (0.01, 1)) = 0.275
		_SpecColor ("Specular Color", Color) = (1,1,1,0.5)
		_SpecGlossMap("Spec (R) Gloss (G)", 2D) = "white" {}
		_SpecularIntensity("Intensity", Range (0, 2)) = 0.5
		
		//Reflection
		[Toggle] _UseReflection ("Reflection", int) = 1
		[Toggle] _UseFresnel ("Fresnel", int) = 0
		_ReflectColor ("Reflection Color", Color) = (1, 1, 1, 0.5)
		_ReflectIntensity("Intensity", Range (0, 1)) = 0.5
		_ReflectMap ("Reflectivity (R)", 2D) = "white" {}
		_FresnelFactor("Fresnel Factor", Range (0.01, 5)) = 1.4

		//Translucent
		[Toggle] _UseTranslucent ("Translucent", int) = 0
		_TranslucentColor ("Translucent Color", Color) = (1,1,1,0.5)
		_TranslucentMap ("Power (R)", 2D) = "white" {}
		_TranslucentIntensity("Intensity", Range(0, 10)) = 0.5
		_TranslucentShininess ("Shininess",  Range (0.01, 1)) = 0.275

		//Emission
		_EmissionColor("Emission Color", Color) = (0,0,0)
		_EmissionMap("Emission (RGB)", 2D) = "white" {}

		//Editor
		[HideInInspector] _MKEditorShowMainBehavior ("Main Behavior", int) = 1
		[HideInInspector] _MKEditorShowDetailBehavior ("Detail Behavior", int) = 0
		[HideInInspector] _MKEditorShowLightBehavior ("Light Behavior", int) = 0
		[HideInInspector] _MKEditorShowRenderBehavior ("Render Behavior", int) = 0
		[HideInInspector] _MKEditorShowSpecularBehavior ("Specular Behavior", int) = 0
		[HideInInspector] _MKEditorShowTranslucentBehavior ("Translucent Behavior", int) = 0
		[HideInInspector] _MKEditorShowRimBehavior ("Rim Behavior", int) = 0
		[HideInInspector] _MKEditorShowReflectionBehavior ("Reflection Behavior", int) = 0
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 3.0
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		LOD 300
		Tags {"RenderType"="Transparent" "Queue"="Transparent+21" "PerformanceChecks"="False" "IgnoreProjector"="True"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Grab Refraction
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Get multi refraction
		UsePass "Hidden/MK/Common/GRAB_FWD"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// VERTEX LIT
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Get Vertex Lit from Fallback shader

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull Back
			Blend One Zero
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature_local __ _MK_OCCLUSION
			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#pragma shader_feature_local __ _MK_RIM
			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_REFLECTIVE_FRESNEL
			#pragma shader_feature_local __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature_local __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_SPECULAR_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature_local __ _MK_DETAIL_BUMP_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#pragma multi_compile_instancing

			#include "Inc/Forward/MKGlassForwardBaseSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" "PerformanceChecks"="False"} 
			Name "FORWARDADD"
			Cull Back
			Blend One One 
			ZWrite Off
			ZTest LEqual
			Fog { Color (0,0,0,0) }

			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature_local __ _MK_OCCLUSION
			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_RIM
			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_REFLECTIVE_FRESNEL
			#pragma shader_feature_local __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature_local __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_SPECULAR_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature_local __ _MK_DETAIL_BUMP_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd
			#pragma multi_compile_fwdadd_fullshadows

			#pragma multi_compile_fog

			#include "Inc/Forward/MKGlassForwardAddSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		//TODO deferred shading pass
		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1

			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#pragma multi_compile_instancing

			#include "Inc/ShadowCaster/MKGlassShadowCasterSetup.cginc"
			#include "Inc/ShadowCaster/MKGlassShadowCaster.cginc"

			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" "PerformanceChecks"="False"}
			Name "META" 

			Cull Off

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex metavert
			#pragma fragment metafrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature __ EDITOR_VISUALIZATION

			#include "Inc/Meta/MKGlassMetaSetup.cginc"
			#include "Inc/Meta/MKGlassMeta.cginc"
			ENDCG
		}
    }


	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 2.5 - Mobile
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		LOD 150
		Tags {"RenderType"="Transparent" "Queue"="Transparent+21" "PerformanceChecks"="False" "ForceNoShadowCasting"="True"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Grab Refraction
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Get multi refraction
		UsePass "Hidden/MK/Common/GRAB_FWD"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// VERTEX LIT
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Get Vertex Lit from Fallback shader

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull Back
			Blend One Zero
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#pragma shader_feature_local __ _MK_OCCLUSION
			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_INDEX_OF_REFRACTION
			#pragma shader_feature_local __ _MK_RIM
			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_REFLECTIVE_FRESNEL
			#pragma shader_feature_local __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature_local __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_SPECULAR_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature_local __ _MK_DETAIL_BUMP_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "Inc/Forward/MKGlassForwardBaseSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" "PerformanceChecks"="False"} 
			Name "FORWARDADD"
			Cull Back
			Blend One One 
			ZWrite Off
			ZTest LEqual
			Fog { Color (0,0,0,0) }

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT

			#pragma shader_feature_local __ _MK_OCCLUSION
			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_RIM
			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_REFLECTIVE_FRESNEL
			#pragma shader_feature_local __ _MK_REFLECTIVE_DEFAULT _MK_REFLECTIVE_MAP
			#pragma shader_feature_local __ _MK_TRANSLUCENT_DEFAULT _MK_TRANSLUCENT_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_SPECULAR_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature_local __ _MK_DETAIL_BUMP_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd
			#pragma multi_compile_fwdadd_fullshadows

			#pragma multi_compile_fog

			#include "Inc/Forward/MKGlassForwardAddSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		//TODO deferred shading pass
		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Skip shadow pass because dithering requires shader model 3.0

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" "PerformanceChecks"="False"}
			Name "META" 

			Cull Off

			CGPROGRAM
			#pragma target 2.5
			#pragma vertex metavert
			#pragma fragment metafrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature __ EDITOR_VISUALIZATION

			#include "Inc/Meta/MKGlassMetaSetup.cginc"
			#include "Inc/Meta/MKGlassMeta.cginc"
			ENDCG
		}
    }

	/////////////////////////////////////////////////////////////////////////////////////////////
	// SM 2.0 Very Old - Skip some features
	/////////////////////////////////////////////////////////////////////////////////////////////
	SubShader
	{
		LOD 150
		Tags {"RenderType"="Transparent" "Queue"="Transparent+21" "PerformanceChecks"="False" "ForceNoShadowCasting"="True"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// Grab Refraction
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Get multi refraction
		UsePass "Hidden/MK/Common/GRAB_FWD"
		UsePass "Hidden/MK/Common/GRAB_SHARED_VRTLT"
		UsePass "Hidden/MK/Common/GRAB_SHARED_VRTLTLM"
		UsePass "Hidden/MK/Common/GRAB_SHARED_VRTLTLMRGBM"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// VERTEX LIT
		/////////////////////////////////////////////////////////////////////////////////////////////
		UsePass "Hidden/MK/Common/VERTEXNLM_LIT"
		UsePass "Hidden/MK/Common/VERTEXLM_LIT"
		UsePass "Hidden/MK/Common/VERTEXLMRGBM_LIT"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull Back
			Blend One Zero
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.0

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "Inc/Forward/MKGlassForwardBaseSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" "PerformanceChecks"="False"} 
			Name "FORWARDADD"
			Cull Back
			Blend One One 
			ZWrite Off
			ZTest LEqual
			Fog { Color (0,0,0,0) }

			CGPROGRAM
			#pragma target 2.0

			#pragma skip_variants SHADOWS_SOFT

			#pragma shader_feature_local __ _MK_BUMP_MAP
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vertfwd
			#pragma fragment fragfwd
			#pragma multi_compile_fwdadd_fullshadows

			#pragma multi_compile_fog

			#include "Inc/Forward/MKGlassForwardAddSetup.cginc"
			#include "Inc/Forward/MKGlassForward.cginc"
			
			ENDCG
		}

		//TODO deferred shading pass
		/////////////////////////////////////////////////////////////////////////////////////////////
		// DEFERRED
		/////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		//Skip shadow pass because dithering requires shader model 3.0

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode"="Meta" "PerformanceChecks"="False"}
			Name "META" 

			Cull Off

			CGPROGRAM
			#pragma target 2.0
			#pragma vertex metavert
			#pragma fragment metafrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma shader_feature_local __ _MK_LIGHTMODEL_LAMBERT _MK_LIGHTMODEL_PHONG _MK_LIGHTMODEL_BLINN_PHONG
			#pragma shader_feature_local __ _MK_EMISSION_DEFAULT _MK_EMISSION_MAP
			#pragma shader_feature_local __ _MK_ALBEDO_MAP
			#pragma shader_feature_local __ _MK_DETAIL_MAP
			#pragma shader_feature __ EDITOR_VISUALIZATION

			#include "Inc/Meta/MKGlassMetaSetup.cginc"
			#include "Inc/Meta/MKGlassMeta.cginc"
			ENDCG
		}
    }
	FallBack "Legacy Shaders/Transparent/Diffuse"
	CustomEditor "MK.Glass.MKGlassEditor"
}
