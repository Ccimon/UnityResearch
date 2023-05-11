using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glass
{
    public static class MKGlassMaterialHelper
    {
        public static class PropertyNames
        {
            public const string USE_FAST_MODE = "_UseFastMode";

            //Editor Properties
            public const string SHOW_MAIN_BEHAVIOR = "_MKEditorShowMainBehavior";
            public const string SHOW_DETAIL_BEHAVIOR = "_MKEditorShowDetailBehavior";
            public const string SHOW_LIGHT_BEHAVIOR = "_MKEditorShowLightBehavior";
            public const string SHOW_RENDER_BEHAVIOR = "_MKEditorShowRenderBehavior";
            public const string SHOW_SPECULAR_BEHAVIOR = "_MKEditorShowSpecularBehavior";
            public const string SHOW_TRANSLUCENT_BEHAVIOR = "_MKEditorShowTranslucentBehavior";
            public const string SHOW_RIM_BEHAVIOR = "_MKEditorShowRimBehavior";
            public const string SHOW_REFLECTION_BEHAVIOR = "_MKEditorShowReflectionBehavior";

            //Main
            public const string MAIN_TEXTURE = "_MainTex";
            public const string CULL_MODE = "_CullMode";
            public const string MAIN_COLOR = "_Color";
            public const string MAIN_TINT = "_MainTint";

            //Detail
            public const string DETAIL_ALBEDO_MAP = "_DetailAlbedoMap";
            public const string DETAIL_TINT = "_DetailTint";
            public const string DETAIL_BUMP_SCALE = "_DetailNormalMapScale";
            public const string DETAIL_BUMP_MAP = "_DetailNormalMap";
            public const string DETAIL_COLOR = "_DetailColor";

            //Normalmap
            public const string BUMP_MAP = "_BumpMap";
            public const string BUMP_SCALE = "_BumpScale";
            public const string DISTORTION = "_Distortion";
            public const string REFRACT_FACTOR = "_RefractFactor";
            public const string REFRACT_INTENSITY = "_RefractIntensity";
            public const string INDEX_OF_REFRACTION = "_IndexOfRefraction";

            //Light
            public const string LIGHT_MODEL = "_LightModel";
            public const string OCCLUSION_MAP = "_OcclusionMap";
            public const string OCCLUSION_STRENGTH = "_OcclusionStrength";

            //Render
            public const string RENDER_MODE = "_RenderMode";
            public const string SHADOW_INTENSITY = "_ShadowIntensity";

            //Rim
            public const string USE_RIM = "_UseRim";
            public const string RIM_COLOR = "_RimColor";
            public const string RIM_SIZE = "_RimSize";
            public const string RIM_INTENSITY = "_RimIntensity";

            //Specular
            public const string SPECULAR_SHININESS = "_Shininess";
            public const string SPEC_COLOR = "_SpecColor";
            public const string SPEC_GLOSS_MAP = "_SpecGlossMap";
            public const string SPECULAR_INTENSITY = "_SpecularIntensity";

            //Reflection
            public const string USE_FRESNEL = "_UseFresnel";
            public const string USE_REFLECTION = "_UseReflection";
            public const string REFLECT_COLOR = "_ReflectColor";
            public const string REFLECT_INTENSITY = "_ReflectIntensity";
            public const string REFLECT_MAP = "_ReflectMap";
            public const string FRESNEL_FACTOR = "_FresnelFactor";

            //Translucent
            public const string USE_TRANSLUCENT = "_UseTranslucent";
            public const string TRANSLUCENT_COLOR = "_TranslucentColor";
            public const string TRANSLUCENT_MAP = "_TranslucentMap";
            public const string TRANSLUCENT_INTENSITY = "_TranslucentIntensity";
            public const string TRANSLUCENT_SHININESS = "_TranslucentShininess";

            //Emission
            public const string EMISSION_COLOR = "_EmissionColor";
            public const string EMISSION_MAP = "_EmissionMap";
            public const string EMISSION = "_Emission";
        }

        //Main
        public static void SetMainTint(Material material, float tint)
        {
            material.SetFloat(PropertyNames.MAIN_TINT, tint);
        }
        public static float GetMainTint(Material material)
        {
            return material.GetFloat(PropertyNames.MAIN_TINT);
        }

        public static void SetShadowIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.SHADOW_INTENSITY, intensity);
        }
        public static float GetShadowIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.SHADOW_INTENSITY);
        }

        public static void SetMainTexture(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.MAIN_TEXTURE, tex);
        }
        public static Texture GetMainTexture(Material material)
        {
            return material.GetTexture(PropertyNames.MAIN_TEXTURE);
        }

        public static void SetMainColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.MAIN_COLOR, color);
        }
        public static Color GetMainColor(Material material)
        {
            return material.GetColor(PropertyNames.MAIN_COLOR);
        }

        public static void SetCullMode(Material material, UnityEngine.Rendering.CullMode cull)
        {
            material.SetFloat(PropertyNames.CULL_MODE, (int)cull);
        }
        public static UnityEngine.Rendering.CullMode GetCullMode(Material material)
        {
            return (UnityEngine.Rendering.CullMode)material.GetFloat(PropertyNames.CULL_MODE);
        }

        //Detail
        public static void SetDetailTint(Material material, float tint)
        {
            material.SetFloat(PropertyNames.DETAIL_TINT, tint);
        }
        public static float GeDetailTint(Material material)
        {
            return material.GetFloat(PropertyNames.DETAIL_TINT);
        }
        public static void SetDetailTexture(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DETAIL_ALBEDO_MAP, tex);
        }
        public static Texture GetDetailTexture(Material material)
        {
            return material.GetTexture(PropertyNames.DETAIL_ALBEDO_MAP);
        }
        public static void SetDetailNormalMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.DETAIL_BUMP_MAP, tex);
        }
        public static Texture GetDetailNormalMap(Material material)
        {
            return material.GetTexture(PropertyNames.DETAIL_BUMP_MAP);
        }
        public static void SetDetailBumpScale(Material material, float bumpScale)
        {
            material.SetFloat(PropertyNames.DETAIL_BUMP_SCALE, bumpScale);
        }
        public static float GetDetailBumpScale(Material material)
        {
            return material.GetFloat(PropertyNames.DETAIL_BUMP_SCALE);
        }
        public static void SetDetailColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.DETAIL_COLOR, color);
        }
        public static Color GetDetailColor(Material material)
        {
            return material.GetColor(PropertyNames.DETAIL_COLOR);
        }

        //Normalmap
        public static void SetNormalmap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.BUMP_MAP, tex);
        }
        public static Texture GetBumpMap(Material material)
        {
            return material.GetTexture(PropertyNames.BUMP_MAP);
        }

        public static void SetBumpScale(Material material, float bumpScale)
        {
            material.SetFloat(PropertyNames.BUMP_SCALE, bumpScale);
        }
        public static float GetBumpScale(Material material)
        {
            return material.GetFloat(PropertyNames.BUMP_SCALE);
        }

        public static void SetDistortion(Material material, float distortion)
        {
            material.SetFloat(PropertyNames.DISTORTION, distortion);
        }
        public static float GetDistortion(Material material)
        {
            return material.GetFloat(PropertyNames.DISTORTION);
        }
        public static void SetRefractFactor(Material material, float refractFactor)
        {
            material.SetFloat(PropertyNames.REFRACT_FACTOR, refractFactor);
        }
        public static float GetRefractFactor(Material material)
        {
            return material.GetFloat(PropertyNames.REFRACT_FACTOR);
        }
        public static void SetRefractIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.REFRACT_INTENSITY, intensity);
        }
        public static float GetRefractIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.REFRACT_INTENSITY);
        }
        public static void SetIndexOfRefraction(Material material, float ior)
        {
            material.SetFloat(PropertyNames.INDEX_OF_REFRACTION, ior);
        }
        public static float GetIndexOfRefraction(Material material)
        {
            return material.GetFloat(PropertyNames.INDEX_OF_REFRACTION);
        }

        //Rim
        public static void SetRimColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.RIM_COLOR, color);
        }
        public static Color GetRimColor(Material material)
        {
            return material.GetColor(PropertyNames.RIM_COLOR);
        }

        public static void SetRimSize(Material material, float size)
        {
            material.SetFloat(PropertyNames.RIM_SIZE, size);
        }
        public static float GetRimSize(Material material)
        {
            return material.GetFloat(PropertyNames.RIM_SIZE);
        }

        public static void SetRimIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.RIM_INTENSITY, intensity);
        }
        public static float GetRimIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.RIM_INTENSITY);
        }

        //Specular
        public static void SetSpecularShininess(Material material, float shininess)
        {
            material.SetFloat(PropertyNames.SPECULAR_SHININESS, shininess);
        }
        public static float GetSpecularShininess(Material material)
        {
            return material.GetFloat(PropertyNames.SPECULAR_SHININESS);
        }

        public static void SetSpecularColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.SPEC_COLOR, color);
        }
        public static Color GetSpecularColor(Material material)
        {
            return material.GetColor(PropertyNames.SPEC_COLOR);
        }

        public static void SetSpecularMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.SPEC_GLOSS_MAP, tex);
        }
        public static Texture GetSpecularMap(Material material)
        {
            return material.GetTexture(PropertyNames.SPEC_GLOSS_MAP);
        }

        public static void SetSpecularIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.SPECULAR_INTENSITY, intensity);
        }
        public static float GetSpecularIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.SPECULAR_INTENSITY);
        }

        //Reflection
        public static void SetReflectMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.REFLECT_MAP, tex);
        }
        public static Texture GetReflectMap(Material material)
        {
            return material.GetTexture(PropertyNames.REFLECT_MAP);
        }

        public static void SetReflectColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.REFLECT_COLOR, color);
        }
        public static Color GetReflectColor(Material material)
        {
            return material.GetColor(PropertyNames.REFLECT_COLOR);
        }

        public static void SetReflectIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.REFLECT_INTENSITY, intensity);
        }
        public static float GetReflectIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.REFLECT_INTENSITY);
        }
        public static void SetReflectionFresnelFactor(Material material, float factor)
        {
            material.SetFloat(PropertyNames.FRESNEL_FACTOR, factor);
        }
        public static float GetReflectionFresnelFactor(Material material)
        {
            return material.GetFloat(PropertyNames.FRESNEL_FACTOR);
        }

        //Translucent
        public static void SetTranslucentMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.TRANSLUCENT_MAP, tex);
        }
        public static Texture GetTranslucentMap(Material material)
        {
            return material.GetTexture(PropertyNames.TRANSLUCENT_MAP);
        }

        public static void SetTranslucentColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.TRANSLUCENT_COLOR, color);
        }
        public static Color GetTranslucentColor(Material material)
        {
            return material.GetColor(PropertyNames.TRANSLUCENT_COLOR);
        }

        public static void SetTranslucentIntensity(Material material, float intensity)
        {
            material.SetFloat(PropertyNames.TRANSLUCENT_INTENSITY, intensity);
        }
        public static float GetTranslucentIntensity(Material material)
        {
            return material.GetFloat(PropertyNames.TRANSLUCENT_INTENSITY);
        }

        public static void SetTranslucentShininess(Material material, float shininess)
        {
            material.SetFloat(PropertyNames.TRANSLUCENT_SHININESS, shininess);
        }
        public static float GetTranslucentShininess(Material material)
        {
            return material.GetFloat(PropertyNames.TRANSLUCENT_SHININESS);
        }

        //Emission
        public static void SetEmissionMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.EMISSION_MAP, tex);
        }
        public static Texture GetEmissionMap(Material material)
        {
            return material.GetTexture(PropertyNames.EMISSION_MAP);
        }

        public static void SetEmissionColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.EMISSION_COLOR, color);
        }
        public static Color GetEmissionColor(Material material)
        {
            return material.GetColor(PropertyNames.EMISSION_COLOR);
        }

        //Occlusion
        public static void SetOcclusionMap(Material material, Texture tex)
        {
            material.SetTexture(PropertyNames.OCCLUSION_MAP, tex);
        }
        public static Texture GetOcclusionMap(Material material)
        {
            return material.GetTexture(PropertyNames.OCCLUSION_MAP);
        }
        public static void SetOcclusionStrength(Material material, float strength)
        {
            material.SetFloat(PropertyNames.OCCLUSION_STRENGTH, strength);
        }
        public static float GetOcclusionStrength(Material material)
        {
            return material.GetFloat(PropertyNames.OCCLUSION_STRENGTH);
        }
    }
}