using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.Utils;
using UnityEditorInternal;

#if UNITY_EDITOR
namespace MK.Glass
{
    #pragma warning disable CS0612, CS0618, CS1692
    public static class GuiStyles
    {
        public static GUIStyle header = new GUIStyle("ShurikenModuleTitle")
        {
            font = (new GUIStyle("Label")).font,
            border = new RectOffset(15, 7, 4, 4),
            fixedHeight = 22,
            contentOffset = new Vector2(20f, -2f),
        };

        public static GUIStyle headerCheckbox = new GUIStyle("ShurikenCheckMark");
        public static GUIStyle headerCheckboxMixed = new GUIStyle("ShurikenCheckMarkMixed");
    }
    public class MKGlassEditor : ShaderGUI
    {
        private static class GUIContentCollection
        {
            public static GUIContent mainColor = new GUIContent("Color", "Basic color tint");
            public static GUIContent detailColor = new GUIContent("Color", "Basic detail tint");
            public static GUIContent mainTint = new GUIContent("Main tint", "Tint amount");
            public static GUIContent shadowIntensity = new GUIContent("Shadow intensity", "Intensity of the shadow");
            public static GUIContent cullMode = new GUIContent("Cull", "Controls which sides of the polygon shuld not be drawn");
            public static GUIContent lightModel = new GUIContent("Light model", "Change the lightmodel \n Unlit - No lights are calculated \n Lambert - Diffuse shading wihout specular \n Blinn-Phong - Diffuse and specular shading");
            public static GUIContent reflectSmoothness = new GUIContent("Reflect smoothness", "Smoothness of the reflection the surface");
            public static GUIContent specularShininess = new GUIContent("Shininess", "The level of blur for the specular highlight");
            public static GUIContent specularColor = new GUIContent("Color", "Color tint of specular highlights");
            public static GUIContent translucentShininess = new GUIContent("Shininess", "The level of blur for the translucent highlight");
            public static GUIContent translucentColor = new GUIContent("Color", "Color tint of translucent highlights");
            public static GUIContent rimSize = new GUIContent("Size", "Amount of highlighted areas by rim");
            public static GUIContent rimColor = new GUIContent("Color", "Color of the rim highlight");
            public static GUIContent rimIntensity = new GUIContent("Intensity", "Intensity of the rim highlight");
            public static GUIContent reflectColor = new GUIContent("Color", "Color tint of the reflection");
            public static GUIContent reflection = new GUIContent("Intensity", "Reflect Map (R)");
            public static GUIContent emission = new GUIContent("Emission", "Emission Map (RGB)");
            public static GUIContent specular = new GUIContent("intensity", "Spec (R) Gloss (G) Aniso (B)");
            public static GUIContent translucent = new GUIContent("intensity", "Power (R) Gloss (G)");
            public static GUIContent normalMap = new GUIContent("Normal map", "Normal map (Bump)");
            public static GUIContent useFresnel = new GUIContent("Fresnel effect", "Enable or disable the fresnel effect");
            public static GUIContent FresnelFactor = new GUIContent("Fresnel factor", "Fresnel interpolation between refraction and reflection");
            public static GUIContent indexOfRefraction = new GUIContent("IOR", "Index of refraction, controls how much the refraction is bending over the surface.");
            public static GUIContent distortion = new GUIContent("Distortion", "Distortion");
            public static GUIContent mainTex = new GUIContent("Albedo", "Albedo (RGBA)");
            public static GUIContent detailTex = new GUIContent("Detail", "Detail Albedo 1X, multiplied to the main albedo (RGB)");
            public static GUIContent detailNormalMap = new GUIContent("Normal map", "Detail normal map (Bump)");
            public static GUIContent renderMode = new GUIContent("Render mode", "Change the render mode \n \n Basic - Culling back faces \n \n Double_Sided - Blend front and back faces together, this doubles the drawcalls \n Double_Sided_Alpha - Culling is disabled and back faces are shown through alpha");
            public static GUIContent fastMode = new GUIContent("Shared frame grab", "Stop framebuffer grabbing for each object to make glass faster");
            public static GUIContent occlusion = new GUIContent("Occlusion", "Occlusion map (G) and strength");
        }

        #region const
        private const string OCCLUSION = "_MK_OCCLUSION";
        private const string BUMP_MAP = "_MK_BUMP_MAP";
        private const string EMISSION_DEFAULT = "_MK_EMISSION_DEFAULT";
        private const string EMISSION_MAP = "_MK_EMISSION_MAP";
        private const string ALBEDO_MAP = "_MK_ALBEDO_MAP";
        private const string INDEX_OF_REFRACTION = "_MK_INDEX_OF_REFRACTION";
        private const string REFLECTIVE_MAP = "_MK_REFLECTIVE_MAP";
        private const string REFLECTIVE_DEFAULT = "_MK_REFLECTIVE_DEFAULT";
        private const string REFLECTIVE_FRESNEL = "_MK_REFLECTIVE_FRESNEL";
        private const string TRANSLUCENT_DEFAULT = "_MK_TRANSLUCENT_DEFAULT";
        private const string TRANSLUCENT_MAP = "_MK_TRANSLUCENT_MAP";
        private const string SPECULAR_MAP = "_MK_SPECULAR_MAP";
        private const string DETAIL_MAP = "_MK_DETAIL_MAP";
        private const string DETAIL_BUMP_MAP = "_MK_DETAIL_BUMP_MAP";
        private readonly string[] LIGHT_MODEL = new string[4] {"_MK_LIGHTMODEL_UNLIT", "_MK_LIGHTMODEL_LAMBERT", "_MK_LIGHTMODEL_PHONG", "_MK_LIGHTMODEL_BLINN_PHONG" };
        private const string RIM = "_MK_RIM";
        #endregion
        #region pe
        internal enum LightModel
        {
            Unlit = 0,
            Lambert = 1,
            Phong = 2,
            Blinn_Phong = 3
        }
        internal enum RenderMode
        {
            Basic = 0,
            Double_Sided = 1,
            Double_Sided_Alpha = 2
        }
        private enum KeywordsToManage
        {
            COLOR_SOURCE,
            BUMP,
            LIGHT_MODEL,
            RIM,
            DETAIL_ALBEDO,
            DETAIL_BUMP,
            SPECULAR,
            EMISSION,
            REFLECTIVE,
            TRANSLUCENT,
            OCCLUSION,
            INDEX_OF_REFRACTION,
            ALL
        };
        #endregion

        //hdr config
        private ColorPickerHDRConfig colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

        //shaders
        private static Shader defaultGlass = Shader.Find("MK/Glass/Default");
        private static Shader defaultDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided");
        private static Shader DefaultDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided Alpha");

        private static Shader fastGlass = Shader.Find("Hidden/MK/Glass/Fast");
        private static Shader fastDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided");
        private static Shader fastDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided Alpha");

        private MaterialProperty useFastMode = null;

        //Editor Properties
        private MaterialProperty showMainBehavior = null;
        private MaterialProperty showDetailBehavior = null;
        private MaterialProperty showLightBehavior = null;
        private MaterialProperty showRenderBehavior = null;
        private MaterialProperty showSpecularBehavior = null;
        private MaterialProperty showTranslucentBehavior = null;
        private MaterialProperty showRimBehavior = null;
        private MaterialProperty showReflectionBehavior = null;

        //Main
        private MaterialProperty mainColor = null;
        private MaterialProperty mainTex = null;
        private MaterialProperty mainTint = null;

        //detail
        private MaterialProperty detailAlbedoMap = null;
        private MaterialProperty detailTint = null;
        private MaterialProperty detailNormalMapScale = null;
        private MaterialProperty detailNormalMap = null;
        private MaterialProperty detailColor = null;

        //Normalmap
        private MaterialProperty normalMap = null;
        private MaterialProperty bumpScale = null;
        private MaterialProperty distortion = null;
        private MaterialProperty indexOfRefraction = null;

        //Light
        private MaterialProperty lightModel = null;
        private MaterialProperty occlusionMap = null;
        private MaterialProperty occlusionStrength = null;

        //Render
        private MaterialProperty shadowIntensity = null;
        private MaterialProperty renderMode = null;

        //Rim
        private MaterialProperty rimColor = null;
        private MaterialProperty rimSize = null;
        private MaterialProperty rimIntensity = null;
        private MaterialProperty useRim = null;

        //Specular
        private MaterialProperty specularIntensity = null;
        private MaterialProperty reflectIntensity = null;
        private MaterialProperty specularMap = null;
        private MaterialProperty shininess = null;
        private MaterialProperty specularColor = null;

        //Reflection
        private MaterialProperty useReflection = null;
        private MaterialProperty useFresnel = null;
        private MaterialProperty reflectionColor = null;
        private MaterialProperty reflectMap = null;
        private MaterialProperty fresnelFactor = null;

        //Translucent
        private MaterialProperty translucentMap = null;
        private MaterialProperty translucentColor = null;
        private MaterialProperty useTranslucent = null;
        private MaterialProperty translucentIntensity = null;
        private MaterialProperty translucentShininess = null;

        //Emission
        private MaterialProperty emissionColor = null;
        private MaterialProperty emissionTex = null;

        public void FindProperties(MaterialProperty[] props, Material mat)
        {
            defaultGlass = Shader.Find("MK/Glass/Default");
            defaultDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided");
            DefaultDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided Alpha");

            fastGlass = Shader.Find("Hidden/MK/Glass/Fast");
            fastDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided");
            fastDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided Alpha");

            useFastMode = FindProperty(MKGlassMaterialHelper.PropertyNames.USE_FAST_MODE, props);

            //Editor Properties
            showMainBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_MAIN_BEHAVIOR, props);
            showDetailBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_DETAIL_BEHAVIOR, props);
            showLightBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_LIGHT_BEHAVIOR, props);
            showRenderBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_RENDER_BEHAVIOR, props);
            showSpecularBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_SPECULAR_BEHAVIOR, props);
            showTranslucentBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_TRANSLUCENT_BEHAVIOR, props);
            showRimBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_RIM_BEHAVIOR, props);
            showReflectionBehavior = FindProperty(MKGlassMaterialHelper.PropertyNames.SHOW_REFLECTION_BEHAVIOR, props);

            //Main
            mainColor = FindProperty(MKGlassMaterialHelper.PropertyNames.MAIN_COLOR, props);
            mainTint = FindProperty(MKGlassMaterialHelper.PropertyNames.MAIN_TINT, props);
            mainTex = FindProperty(MKGlassMaterialHelper.PropertyNames.MAIN_TEXTURE, props);

            //detail
            detailAlbedoMap = FindProperty(MKGlassMaterialHelper.PropertyNames.DETAIL_ALBEDO_MAP, props);
            detailTint = FindProperty(MKGlassMaterialHelper.PropertyNames.DETAIL_TINT, props);
            detailNormalMapScale = FindProperty(MKGlassMaterialHelper.PropertyNames.DETAIL_BUMP_SCALE, props);
            detailNormalMap = FindProperty(MKGlassMaterialHelper.PropertyNames.DETAIL_BUMP_MAP, props);
            detailColor = FindProperty(MKGlassMaterialHelper.PropertyNames.DETAIL_COLOR, props);

            //Normalmap
            bumpScale = FindProperty(MKGlassMaterialHelper.PropertyNames.BUMP_SCALE, props);
            normalMap = FindProperty(MKGlassMaterialHelper.PropertyNames.BUMP_MAP, props);
            distortion = FindProperty(MKGlassMaterialHelper.PropertyNames.DISTORTION, props);
            indexOfRefraction = FindProperty(MKGlassMaterialHelper.PropertyNames.INDEX_OF_REFRACTION, props);

            //Light
            lightModel = FindProperty(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL, props);
            renderMode = FindProperty(MKGlassMaterialHelper.PropertyNames.RENDER_MODE, props);
            occlusionMap = FindProperty(MKGlassMaterialHelper.PropertyNames.OCCLUSION_MAP, props);
            occlusionStrength = FindProperty(MKGlassMaterialHelper.PropertyNames.OCCLUSION_STRENGTH, props);

            //Render
            shadowIntensity = FindProperty(MKGlassMaterialHelper.PropertyNames.SHADOW_INTENSITY, props);

            //Rim
            rimColor = FindProperty(MKGlassMaterialHelper.PropertyNames.RIM_COLOR, props);
            rimSize = FindProperty(MKGlassMaterialHelper.PropertyNames.RIM_SIZE, props);
            useRim = FindProperty(MKGlassMaterialHelper.PropertyNames.USE_RIM, props);
            rimIntensity = FindProperty(MKGlassMaterialHelper.PropertyNames.RIM_INTENSITY, props);

            //Specular
            shininess = FindProperty(MKGlassMaterialHelper.PropertyNames.SPECULAR_SHININESS, props);
            specularColor = FindProperty(MKGlassMaterialHelper.PropertyNames.SPEC_COLOR, props);
            specularMap = FindProperty(MKGlassMaterialHelper.PropertyNames.SPEC_GLOSS_MAP, props);
            specularIntensity = FindProperty(MKGlassMaterialHelper.PropertyNames.SPECULAR_INTENSITY, props);

            //Reflection
            useReflection = FindProperty(MKGlassMaterialHelper.PropertyNames.USE_REFLECTION, props);
            fresnelFactor = FindProperty(MKGlassMaterialHelper.PropertyNames.FRESNEL_FACTOR, props);
            useFresnel = FindProperty(MKGlassMaterialHelper.PropertyNames.USE_FRESNEL, props);
            reflectionColor = FindProperty(MKGlassMaterialHelper.PropertyNames.REFLECT_COLOR, props);
            reflectMap = FindProperty(MKGlassMaterialHelper.PropertyNames.REFLECT_MAP, props);
            reflectIntensity = FindProperty(MKGlassMaterialHelper.PropertyNames.REFLECT_INTENSITY, props);

            //Emission
            emissionColor = FindProperty(MKGlassMaterialHelper.PropertyNames.EMISSION_COLOR, props);
            emissionTex = FindProperty(MKGlassMaterialHelper.PropertyNames.EMISSION_MAP, props);

            //Translucent
            translucentColor = FindProperty(MKGlassMaterialHelper.PropertyNames.TRANSLUCENT_COLOR, props);
            translucentMap = FindProperty(MKGlassMaterialHelper.PropertyNames.TRANSLUCENT_MAP, props);
            useTranslucent = FindProperty(MKGlassMaterialHelper.PropertyNames.USE_TRANSLUCENT, props);
            translucentIntensity = FindProperty(MKGlassMaterialHelper.PropertyNames.TRANSLUCENT_INTENSITY, props);
            translucentShininess = FindProperty(MKGlassMaterialHelper.PropertyNames.TRANSLUCENT_SHININESS, props);

        }

        static void Reset(MenuCommand command)
        {
            try
            {
                defaultGlass = Shader.Find("MK/Glass/Default");
                defaultDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided");
                DefaultDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Default Double Sided Alpha");

                fastGlass = Shader.Find("Hidden/MK/Glass/Fast");
                fastDoubleSidedGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided");
                fastDoubleSidedAlphaGlass = Shader.Find("Hidden/MK/Glass/Fast Double Sided Alpha");

                Material mat = null;
                mat = (Material)command.context;
                Undo.RecordObject(mat, "Reset Material");
                string[] kws = mat.shaderKeywords;
                Material tmp_mat = new Material(mat.shader);
                mat.CopyPropertiesFromMaterial(tmp_mat);

                if (mat.GetFloat(MKGlassMaterialHelper.PropertyNames.RENDER_MODE) == 1.0f)
                {
                    if (mat.shader != defaultDoubleSidedGlass)
                        mat.shader = defaultDoubleSidedGlass;
                }
                else if (mat.GetFloat(MKGlassMaterialHelper.PropertyNames.RENDER_MODE) == 2.0f)
                {
                    if (mat.shader != DefaultDoubleSidedAlphaGlass)
                        mat.shader = DefaultDoubleSidedAlphaGlass;
                }
                else
                {
                    if (mat.shader != defaultGlass)
                        mat.shader = defaultGlass;
                }

                if (mat.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_FAST_MODE) == 1.0f)
                {
                    if (mat.shader == defaultDoubleSidedGlass)
                    {
                        mat.shader = fastDoubleSidedGlass;
                    }
                    else if (mat.shader == DefaultDoubleSidedAlphaGlass)
                    {
                        mat.shader = fastDoubleSidedAlphaGlass;
                    }
                    else if (mat.shader == defaultGlass)
                    {
                        mat.shader = fastGlass;
                    }
                }
                else
                {
                    if (mat.shader == fastDoubleSidedGlass)
                    {
                        mat.shader = defaultDoubleSidedGlass;
                    }
                    else if (mat.shader == fastDoubleSidedAlphaGlass)
                    {
                        mat.shader = DefaultDoubleSidedAlphaGlass;
                    }
                    else if (mat.shader == fastGlass)
                    {
                        mat.shader = defaultGlass;
                    }
                }

                mat.shaderKeywords = kws;
            }
            catch { }
        }

        //Colorfield
        private void ColorProperty(MaterialProperty prop, bool showAlpha, bool hdrEnabled, GUIContent label)
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            Color c = EditorGUILayout.ColorField(label, prop.colorValue, false, showAlpha, hdrEnabled, colorPickerHDRConfig);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                prop.colorValue = c;
        }

        //Setup GI emission
        private void SetGIFlags(Material material)
        {
            bool emissive = true;
            if (MKGlassMaterialHelper.GetEmissionColor(material) == Color.black)
            {
                emissive = false;
            }
            MaterialGlobalIlluminationFlags flags = material.globalIlluminationFlags;
            if ((flags & (MaterialGlobalIlluminationFlags.BakedEmissive | MaterialGlobalIlluminationFlags.RealtimeEmissive)) != 0)
            {
                flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                if (!emissive)
                    flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                material.globalIlluminationFlags = flags;
            }
        }

        //BoldToggle
        private void ToggleBold(MaterialEditor materialEditor, MaterialProperty prop)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(prop.displayName, EditorStyles.boldLabel, GUILayout.Width(100));
            materialEditor.ShaderProperty(prop, "");
            EditorGUILayout.EndHorizontal();
        }

        //Lightmodel
        private void LightModelPopup(MaterialEditor materialEditor, Material material)
        {
            EditorGUI.showMixedValue = lightModel.hasMixedValue;
            LightModel lm = new LightModel();
            lm = (LightModel)lightModel.floatValue;
            EditorGUI.BeginChangeCheck();
            lm = (LightModel)EditorGUILayout.EnumPopup(GUIContentCollection.lightModel, lm);
            if (EditorGUI.EndChangeCheck())
            {
                lightModel.floatValue = (float)lm;
                materialEditor.RegisterPropertyChangeUndo("Lightmodel");
                material.SetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL, lightModel.floatValue);
            }
            EditorGUI.showMixedValue = false;
        }

        //Rendermode
        private void RenderModePopup(MaterialEditor materialEditor, Material material)
        {
            EditorGUI.showMixedValue = renderMode.hasMixedValue;

            RenderMode rm = new RenderMode();
            rm = (RenderMode)renderMode.floatValue;
            EditorGUI.BeginChangeCheck();
            rm = (RenderMode)EditorGUILayout.EnumPopup(GUIContentCollection.renderMode, rm);
            if (EditorGUI.EndChangeCheck())
            {
                renderMode.floatValue = (float)rm;
                materialEditor.RegisterPropertyChangeUndo("Render mode");
                material.SetFloat(MKGlassMaterialHelper.PropertyNames.RENDER_MODE, renderMode.floatValue);
                foreach(Material mat in renderMode.targets)
                    ManageVariantsInternal(mat);
            }
            EditorGUI.showMixedValue = false;
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material.HasProperty(MKGlassMaterialHelper.PropertyNames.EMISSION))
            {
                MKGlassMaterialHelper.SetEmissionColor(material, material.GetColor(MKGlassMaterialHelper.PropertyNames.EMISSION));
            }
           
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            MaterialProperty[] properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
            FindProperties(properties, material);

            UpdateKeywords(KeywordsToManage.ALL, material);
        }

        private bool HandleBehavior(string title, ref MaterialProperty behavior, MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = behavior.hasMixedValue;
            var rect = GUILayoutUtility.GetRect(16f, 22f, GuiStyles.header);
            rect.x -= 10;
            rect.width += 10;
            var e = Event.current;

            GUI.Box(rect, title, GuiStyles.header);

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y + 2, 13f, 13f);
            if (behavior.hasMixedValue)
            {
                foldoutRect.x -= 13;
                foldoutRect.y -= 2;
            }

            EditorGUI.BeginChangeCheck();
            if (e.type == EventType.MouseDown)
            {
                if (rect.Contains(e.mousePosition))
                {
                    if (behavior.hasMixedValue)
                        behavior.floatValue = 0.0f;
                    else
                        behavior.floatValue = Convert.ToSingle(!Convert.ToBoolean(behavior.floatValue));
                    e.Use();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Convert.ToBoolean(behavior.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Show");
                else
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Hide");
            }

            EditorGUI.showMixedValue = false;

            if (e.type == EventType.Repaint && behavior.hasMixedValue)
                EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, Convert.ToBoolean(behavior.floatValue), "");

            if (behavior.hasMixedValue)
                return true;
            else
                return Convert.ToBoolean(behavior.floatValue);
        }

        private bool HandleBehavior(string title, ref MaterialProperty behavior, ref MaterialProperty feature, MaterialEditor materialEditor, string featureName)
        {
            var rect = GUILayoutUtility.GetRect(16f, 22f, GuiStyles.header);
            rect.x -= 10;
            rect.width += 10;
            var e = Event.current;

            GUI.Box(rect, title, GuiStyles.header);

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y + 2, 13f, 13f);
            if (behavior.hasMixedValue)
            {
                foldoutRect.x -= 13;
                foldoutRect.y -= 2;
            }

            EditorGUI.showMixedValue = feature.hasMixedValue;
            var toggleRect = new Rect(rect.x + 4f, rect.y + ((feature.hasMixedValue) ? 0.0f : 4.0f), 13f, 13f);
            bool fn = Convert.ToBoolean(feature.floatValue);
            EditorGUI.BeginChangeCheck();

            fn = EditorGUI.Toggle(toggleRect, "", fn, GuiStyles.headerCheckbox);

            if (EditorGUI.EndChangeCheck())
            {
                feature.floatValue = Convert.ToSingle(fn);
                if (Convert.ToBoolean(feature.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(feature.displayName + " enabled");
                else
                    materialEditor.RegisterPropertyChangeUndo(feature.displayName + " disabled");
                foreach (Material mat in feature.targets)
                {
                    mat.SetFloat(featureName, feature.floatValue);
                }
            }
            EditorGUI.showMixedValue = false;

            EditorGUI.showMixedValue = behavior.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            if (e.type == EventType.MouseDown)
            {
                if (rect.Contains(e.mousePosition))
                {
                    if (behavior.hasMixedValue)
                        behavior.floatValue = 0.0f;
                    else
                        behavior.floatValue = Convert.ToSingle(!Convert.ToBoolean(behavior.floatValue));
                    e.Use();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Convert.ToBoolean(behavior.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " show");
                else
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " hide");
            }

            EditorGUI.showMixedValue = false;

            if (e.type == EventType.Repaint && behavior.hasMixedValue)
                EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, Convert.ToBoolean(behavior.floatValue), "");

            if (behavior.hasMixedValue)
                return true;
            else
                return Convert.ToBoolean(behavior.floatValue);
        }

        override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;
            //MeshRenderer[] targetMeshRenderers = materialEditor.targets as MeshRenderer[];
            //get properties
            FindProperties(properties, targetMat);

            #if !UNITY_2021_2_OR_NEWER
            EditorGUI.BeginChangeCheck();
            #endif
            //main settings
            if (HandleBehavior("Main", ref showMainBehavior, materialEditor))
            {
                ColorProperty(mainColor, false, false, GUIContentCollection.mainColor);
                materialEditor.TexturePropertySingleLine(GUIContentCollection.mainTex, mainTex, mainTint);
                if (normalMap.textureValue == null)
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.normalMap, normalMap);
                else
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.normalMap, normalMap, bumpScale);
                }
                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    materialEditor.TexturePropertyWithHDRColor(GUIContentCollection.emission, emissionTex, emissionColor, colorPickerHDRConfig, false);
                    if (emissionColor.colorValue != Color.black)
                        materialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
                }
                materialEditor.TextureScaleOffsetProperty(mainTex);
            }

            //Detail
            if (HandleBehavior("Detail", ref showDetailBehavior, materialEditor))
            {
                if (detailAlbedoMap.textureValue != null)
                    ColorProperty(detailColor, false, false, GUIContentCollection.detailColor);
                if (detailAlbedoMap.textureValue == null)
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailTex, detailAlbedoMap);
                }
                else
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailTex, detailAlbedoMap, detailTint);
                }
                if (detailNormalMap.textureValue == null)
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailNormalMap, detailNormalMap);
                }
                else
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.detailNormalMap, detailNormalMap, detailNormalMapScale);
                }
                materialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
            }

            //light settings
            if (HandleBehavior("Light", ref showLightBehavior, materialEditor))
            {
                LightModelPopup(materialEditor, targetMat);
                if (lightModel.floatValue == (int)(LightModel.Unlit))
                {
                    EditorGUILayout.HelpBox("To completly unlit, please disable shadow casting & recieving on the MeshRenderer", MessageType.Info);
                }
                if (occlusionMap.textureValue == null)
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.occlusion, occlusionMap);
                else
                {
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.occlusion, occlusionMap, occlusionStrength);
                }
            }

            //Render settings
            if (HandleBehavior("Render", ref showRenderBehavior, materialEditor))
            {
                materialEditor.ShaderProperty(useFastMode, GUIContentCollection.fastMode);
                materialEditor.EnableInstancingField();

                if (normalMap.textureValue != null)
                {
                    materialEditor.ShaderProperty(distortion, GUIContentCollection.distortion);
                }
                else
                {
                    EditorGUILayout.HelpBox("Please set up a normal map to use distortion feature", MessageType.Info);
                }
                materialEditor.ShaderProperty(indexOfRefraction, GUIContentCollection.indexOfRefraction);
                RenderModePopup(materialEditor, targetMat);
                if (lightModel.floatValue != (int)(LightModel.Unlit))
                {
                    materialEditor.ShaderProperty(shadowIntensity, GUIContentCollection.shadowIntensity);
                }
            }

            //specular settings
            if (lightModel.floatValue == (int)(LightModel.Blinn_Phong) || lightModel.floatValue == (int)(LightModel.Phong))
            {
                if (HandleBehavior("Specular", ref showSpecularBehavior, materialEditor))
                {
                    ColorProperty(specularColor, false, false, GUIContentCollection.specularColor);
                    materialEditor.ShaderProperty(shininess, GUIContentCollection.specularShininess);
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.specular, specularMap, specularIntensity);
                }
            }

            //translucent settings
            if (lightModel.floatValue == (int)(LightModel.Blinn_Phong) || lightModel.floatValue == (int)(LightModel.Phong))
            {
                if (HandleBehavior("Translucent", ref showTranslucentBehavior, ref useTranslucent, materialEditor, MKGlassMaterialHelper.PropertyNames.USE_TRANSLUCENT))
                {
                    ColorProperty(translucentColor, false, false, GUIContentCollection.translucentColor);
                    materialEditor.ShaderProperty(translucentShininess, GUIContentCollection.translucentShininess);
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.translucent, translucentMap, translucentIntensity);
                }
            }

            //rim settings
            if (lightModel.floatValue != (int)(LightModel.Unlit))
            {
                if (HandleBehavior("Rim", ref showRimBehavior, ref useRim, materialEditor, MKGlassMaterialHelper.PropertyNames.USE_RIM))
                {
                    ColorProperty(rimColor, false, false, GUIContentCollection.rimColor);
                    materialEditor.ShaderProperty(rimSize, GUIContentCollection.rimSize);
                    materialEditor.ShaderProperty(rimIntensity, GUIContentCollection.rimIntensity);
                }
            }

            //reflection settings
            if (lightModel.floatValue != (int)(LightModel.Unlit))
            {
                if (HandleBehavior("Reflection", ref showReflectionBehavior, ref useReflection, materialEditor, MKGlassMaterialHelper.PropertyNames.USE_REFLECTION))
                {
                    materialEditor.ShaderProperty(useFresnel, GUIContentCollection.useFresnel);
                    if (useFresnel.floatValue == 1.0)
                        materialEditor.ShaderProperty(fresnelFactor, GUIContentCollection.FresnelFactor);
                    ColorProperty(reflectionColor, false, false, GUIContentCollection.reflectColor);
                    materialEditor.TexturePropertySingleLine(GUIContentCollection.reflection, reflectMap, reflectIntensity);
                }
            }
            #if !UNITY_2021_2_OR_NEWER
            if(EditorGUI.EndChangeCheck())
            {
                foreach (Material mat in distortion.targets)
                    ValidateMaterial(mat);
            }
            #endif
        }

        private void ManageVariantsInternal(Material material)
        {
            //Manage variants
            if (material.GetFloat(MKGlassMaterialHelper.PropertyNames.RENDER_MODE) == 1.0f)
            {
                if (material.shader != defaultDoubleSidedGlass)
                    material.shader = defaultDoubleSidedGlass;
            }
            else if (material.GetFloat(MKGlassMaterialHelper.PropertyNames.RENDER_MODE) == 2.0f)
            {
                if (material.shader != DefaultDoubleSidedAlphaGlass)
                    material.shader = DefaultDoubleSidedAlphaGlass;
            }
            else
            {
                if (material.shader != defaultGlass)
                    material.shader = defaultGlass;
            }

            if(material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_FAST_MODE) == 1.0f)
            {
                if (material.shader == defaultDoubleSidedGlass)
                {
                    material.shader = fastDoubleSidedGlass;
                }
                else if (material.shader == DefaultDoubleSidedAlphaGlass)
                {
                    material.shader = fastDoubleSidedAlphaGlass;
                }
                else if (material.shader == defaultGlass)
                {
                    material.shader = fastGlass;
                }
            }
            else
            {
                if (material.shader == fastDoubleSidedGlass)
                {
                    material.shader = defaultDoubleSidedGlass;
                }
                else if (material.shader == fastDoubleSidedAlphaGlass)
                {
                    material.shader = DefaultDoubleSidedAlphaGlass;
                }
                else if (material.shader == fastGlass)
                {
                    material.shader = defaultGlass;
                }
            }
        }

        private void ManageKeywordsColorSource(Material material)
        {
            //Colorsource
            SetKeyword(MKGlassMaterialHelper.GetMainTexture(material), ALBEDO_MAP, material);
        }

        private void ManageKeywordsIOR(Material material)
        {
            //Index Of Refraction
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.INDEX_OF_REFRACTION) > 0, INDEX_OF_REFRACTION, material);
        }

        private void ManageKeywordsLightModel(Material material)
        {
            //Lightmodel
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL) == 1.0f, LIGHT_MODEL[1], material);
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f, LIGHT_MODEL[2], material);
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, LIGHT_MODEL[3], material);
        }

        private void ManageKeywordsOcclusion(Material material)
        {
            //Occlusion
            SetKeyword(MKGlassMaterialHelper.GetOcclusionMap(material), OCCLUSION, material);
        }

        private void ManageKeywordsRim(Material material)
        {
            //Rim
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_RIM) == 1.0f, RIM, material);
        }

        private void ManageKeywordsDetailAlbedo(Material material)
        {
            //detail albedo
            SetKeyword(MKGlassMaterialHelper.GetDetailTexture(material), DETAIL_MAP, material);
        }

        private void ManageKeywordsDetailBump(Material material)
        {
            //detail bump
            SetKeyword(MKGlassMaterialHelper.GetDetailNormalMap(material), DETAIL_BUMP_MAP, material);
        }

        private void ManageKeywordsBump(Material material)
        {
            //Bumpmap
            SetKeyword(MKGlassMaterialHelper.GetBumpMap(material), BUMP_MAP, material);
        }

        private void ManageKeywordsSpecular(Material material)
        {
            //spec
            SetKeyword(MKGlassMaterialHelper.GetSpecularMap(material) != null && material.GetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL) == 2.0f || material.GetFloat(MKGlassMaterialHelper.PropertyNames.LIGHT_MODEL) == 3.0f, SPECULAR_MAP, material);
        }

        private void ManageKeywordsEmission(Material material)
        {
            //emission
            SetKeyword(MKGlassMaterialHelper.GetEmissionMap(material) != null && MKGlassMaterialHelper.GetEmissionColor(material) != Color.black, EMISSION_MAP, material);
            SetKeyword(MKGlassMaterialHelper.GetEmissionMap(material) == null && MKGlassMaterialHelper.GetEmissionColor(material) != Color.black, EMISSION_DEFAULT, material);
            SetGIFlags(material);
        }

        private void ManageKeywordsReflective(Material material)
        {
            //Reflective
            SetKeyword(MKGlassMaterialHelper.GetReflectMap(material) != null && material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, REFLECTIVE_MAP, material);
            SetKeyword(MKGlassMaterialHelper.GetReflectMap(material) == null && material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_REFLECTION) == 1.0f, REFLECTIVE_DEFAULT, material);
            SetKeyword(material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_FRESNEL) == 1.0, REFLECTIVE_FRESNEL, material);
        }

        private void ManageKeywordsTranslucent(Material material)
        {
            //Translucent
            SetKeyword(MKGlassMaterialHelper.GetTranslucentMap(material) == null && material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, TRANSLUCENT_DEFAULT, material);
            SetKeyword(MKGlassMaterialHelper.GetTranslucentMap(material) != null && material.GetFloat(MKGlassMaterialHelper.PropertyNames.USE_TRANSLUCENT) == 1.0f, TRANSLUCENT_MAP, material);
        }

        private void UpdateKeywords(KeywordsToManage kw, Material material)
        {
            switch(kw)
            {
                case KeywordsToManage.ALL:
                    ManageKeywordsBump(material);
                    ManageKeywordsColorSource(material);
                    ManageKeywordsIOR(material);
                    ManageKeywordsDetailAlbedo(material);
                    ManageKeywordsDetailBump(material);
                    ManageKeywordsEmission(material);
                    ManageKeywordsLightModel(material);
                    ManageKeywordsReflective(material);
                    ManageKeywordsRim(material);
                    ManageKeywordsSpecular(material);
                    ManageKeywordsTranslucent(material);
                    ManageKeywordsOcclusion(material);
                    break;
                case KeywordsToManage.BUMP:
                    ManageKeywordsBump(material);
                    break;
                case KeywordsToManage.INDEX_OF_REFRACTION:
                    ManageKeywordsIOR(material);
                break;
                case KeywordsToManage.COLOR_SOURCE:
                    ManageKeywordsColorSource(material);
                    break;
                case KeywordsToManage.DETAIL_ALBEDO:
                    ManageKeywordsDetailAlbedo(material);
                    break;
                case KeywordsToManage.DETAIL_BUMP:
                    ManageKeywordsDetailBump(material);
                    break;
                case KeywordsToManage.EMISSION:
                    ManageKeywordsEmission(material);
                    break;
                case KeywordsToManage.LIGHT_MODEL:
                    ManageKeywordsLightModel(material);
                    break;
                case KeywordsToManage.REFLECTIVE:
                    ManageKeywordsReflective(material);
                    break;
                case KeywordsToManage.RIM:
                    ManageKeywordsRim(material);
                    break;
                case KeywordsToManage.SPECULAR:
                    ManageKeywordsSpecular(material);
                    break;
                case KeywordsToManage.TRANSLUCENT:
                    ManageKeywordsTranslucent(material);
                    break;
                case KeywordsToManage.OCCLUSION:
                    ManageKeywordsOcclusion(material);
                    break;
            }
        }

        #if UNITY_2021_2_OR_NEWER
        public override void ValidateMaterial(Material material)
        {
            UpdateKeywords(KeywordsToManage.ALL, material);
        }
        #else
        public void ValidateMaterial(Material material)
        {
            UpdateKeywords(KeywordsToManage.ALL, material);
        }
        #endif

        private static void SetKeyword(bool enable, string keyword, Material mat)
        {
            if (enable)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
        }

        private void Divider()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }
    }
}
#endif