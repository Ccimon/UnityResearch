Shader "Hidden/MK/Common"
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

	SubShader
	{
		//single Framebuffer Grab Refraction
		GrabPass {Tags { "LightMode" = "ForwardBase" } "_MKGlassRefraction" Name "GRAB_SHARED_FWD" }

		//Multiple Framebuffer Grab Refraction
		GrabPass {Tags { "LightMode" = "ForwardBase" }  Name "GRAB_FWD" }
	}
}
