// <copyright file="LilToonMaterial.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using KRT.VRCQuestTools.Utils;
using UnityEditor;
using UnityEngine;

namespace KRT.VRCQuestTools.Models.Unity
{
    /// <summary>
    /// lilToon material.
    /// </summary>
    internal class LilToonMaterial : MaterialBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LilToonMaterial"/> class.
        /// </summary>
        /// <param name="material">Material.</param>
        internal LilToonMaterial(Material material)
            : base(material)
        {
        }

        /// <inheritdoc/>
        internal override Shader BakeShader => Shader.Find("Hidden/VRCQuestTools/lilToon");

        /// <inheritdoc/>
        internal override Texture2D GenerateToonLitImage(TextureGeneratorSetting setting)
        {
            try
            {
                using (var main = DisposableObject.New(TextureBake(Material, 0)))
                {
                    var baked = EmissionBake(main.Object, Material, setting);
                    return baked;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw new System.Exception($"Compatibility issue for lilToon {AssetUtility.LilToonVersion}. Please report this. (Inner Exception: {e.Message})", e);
            }
        }

        private static LilToonSetting LoadShaderSetting()
        {
            var path = LilToonInspector.GetShaderSettingPath();
            var lilToonSettingType = SystemUtility.GetTypeByName("lilToonSetting");
            var lilToonSetting = AssetDatabase.LoadAssetAtPath(path, lilToonSettingType);
            return new LilToonSetting(lilToonSetting);
        }

        /// <summary>
        /// Reused codes from lilInspector.cs v1.2.12 with some modification.
        /// </summary>
        /// <remarks>
        /// lilToon: Licensed under MIT License by lilxyzw. See NOTICE.txt.
        /// </remarks>
        /// <param name="material">Material to bake main textures.</param>
        /// <param name="bakeType">Bake type: 0: All.</param>
        private Texture2D TextureBake(Material material, int bakeType)
        {
            var shaderSetting = LoadShaderSetting();
            var ltsbaker = Shader.Find("Hidden/ltsother_baker");
            var defaultHSVG = new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
            var mats = new[] { material };
            var mainColor = MaterialEditor.GetMaterialProperty(mats, "_Color");
            var mainTex = MaterialEditor.GetMaterialProperty(mats, "_MainTex");
            var mainTexHSVG = MaterialEditor.GetMaterialProperty(mats, "_MainTexHSVG");
            var mainGradationStrength = MaterialEditor.GetMaterialProperty(mats, "_MainGradationStrength");
            var mainGradationTex = MaterialEditor.GetMaterialProperty(mats, "_MainGradationTex");

            var useMain2ndTex = MaterialEditor.GetMaterialProperty(mats, "_UseMain2ndTex");
            var mainColor2nd = MaterialEditor.GetMaterialProperty(mats, "_Color2nd");
            var main2ndTex = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTex");
            var main2ndTexAngle = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexAngle");
            var main2ndTexIsDecal = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexIsDecal");
            var main2ndTexIsLeftOnly = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexIsLeftOnly");
            var main2ndTexIsRightOnly = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexIsRightOnly");
            var main2ndTexShouldCopy = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexShouldCopy");
            var main2ndTexShouldFlipMirror = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexShouldFlipMirror");
            var main2ndTexShouldFlipCopy = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexShouldFlipCopy");
            var main2ndTexIsMSDF = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexIsMSDF");
            var main2ndBlendMask = MaterialEditor.GetMaterialProperty(mats, "_Main2ndBlendMask");
            var main2ndTexBlendMode = MaterialEditor.GetMaterialProperty(mats, "_Main2ndTexBlendMode");

            var useMain3rdTex = MaterialEditor.GetMaterialProperty(mats, "_UseMain3rdTex");
            var mainColor3rd = MaterialEditor.GetMaterialProperty(mats, "_Color3rd");
            var main3rdTex = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTex");
            var main3rdTexAngle = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexAngle");
            var main3rdTexIsDecal = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexIsDecal");
            var main3rdTexIsLeftOnly = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexIsLeftOnly");
            var main3rdTexIsRightOnly = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexIsRightOnly");
            var main3rdTexShouldCopy = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexShouldCopy");
            var main3rdTexShouldFlipMirror = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexShouldFlipMirror");
            var main3rdTexShouldFlipCopy = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexShouldFlipCopy");
            var main3rdTexIsMSDF = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexIsMSDF");
            var main3rdBlendMask = MaterialEditor.GetMaterialProperty(mats, "_Main3rdBlendMask");
            var main3rdTexBlendMode = MaterialEditor.GetMaterialProperty(mats, "_Main3rdTexBlendMode");

            // bool shouldBake1st = (bakeType == 1 || bakeType == 4) && mainTex.textureValue != null;
            bool shouldNotBakeColor = (bakeType == 1 || bakeType == 4) && mainColor.colorValue == Color.white && mainTexHSVG.vectorValue == defaultHSVG;
            bool cannotBake1st = mainTex.textureValue == null;
            bool shouldNotBake2nd = (bakeType == 2 || bakeType == 5) && useMain2ndTex.floatValue == 0.0;
            bool shouldNotBake3rd = (bakeType == 3 || bakeType == 6) && useMain3rdTex.floatValue == 0.0;
            bool shouldNotBakeAll = bakeType == 0 && mainColor.colorValue == Color.white && mainTexHSVG.vectorValue == defaultHSVG && useMain2ndTex.floatValue == 0.0 && useMain3rdTex.floatValue == 0.0;
            /*
            if (cannotBake1st)
            {
                return null;
            }
            else if (shouldNotBakeColor)
            {
                Debug.Log("Should not need to bake");
                return null;
            }
            else if (shouldNotBake2nd)
            {
                Debug.Log("Should not bake 2nd");
                return null;
            }
            else if (shouldNotBake3rd)
            {
                Debug.Log("Should not bake 3rd");
                return null;
            }
            else if (shouldNotBakeAll)
            {
                Debug.Log("Should not bake all");
                return null;
            }*/
            if (false)
            {
            }
            else
            {
                bool bake2nd = (bakeType == 0 || bakeType == 2 || bakeType == 5) && useMain2ndTex.floatValue != 0.0;
                bool bake3rd = (bakeType == 0 || bakeType == 3 || bakeType == 6) && useMain3rdTex.floatValue != 0.0;

                // run bake
                Texture2D bufMainTexture = (Texture2D)mainTex.textureValue;
                Material hsvgMaterial = new Material(ltsbaker);

                Texture srcTexture = new Texture2D(2, 2);
                Texture srcMain2 = new Texture2D(2, 2);
                Texture srcMain3 = new Texture2D(2, 2);
                Texture srcMask2 = new Texture2D(2, 2);
                Texture srcMask3 = new Texture2D(2, 2);

                hsvgMaterial.SetColor(mainColor.name, mainColor.colorValue);
                hsvgMaterial.SetVector(mainTexHSVG.name, mainTexHSVG.vectorValue);
                hsvgMaterial.SetFloat(mainGradationStrength.name, 0.0f);

                if (CheckFeature(shaderSetting.LIL_FEATURE_MAIN_GRADATION_MAP))
                {
                    hsvgMaterial.SetFloat(mainGradationStrength.name, mainGradationStrength.floatValue);
                    hsvgMaterial.SetTexture(mainGradationTex.name, mainGradationTex.textureValue);
                }

                srcTexture = AssetUtility.LoadUncompressedTexture(material.GetTexture(mainTex.name));
                if (srcTexture != null)
                {
                    hsvgMaterial.SetTexture(mainTex.name, srcTexture);
                }
                else
                {
                    srcTexture = new Texture2D(2, 2);
                    hsvgMaterial.SetTexture(mainTex.name, Texture2D.whiteTexture);
                }

                if (bake2nd)
                {
                    hsvgMaterial.SetFloat(useMain2ndTex.name, useMain2ndTex.floatValue);
                    hsvgMaterial.SetColor(mainColor2nd.name, mainColor2nd.colorValue);
                    hsvgMaterial.SetFloat(main2ndTexAngle.name, main2ndTexAngle.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexIsDecal.name, main2ndTexIsDecal.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexIsLeftOnly.name, main2ndTexIsLeftOnly.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexIsRightOnly.name, main2ndTexIsRightOnly.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexShouldCopy.name, main2ndTexShouldCopy.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexShouldFlipMirror.name, main2ndTexShouldFlipMirror.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexShouldFlipCopy.name, main2ndTexShouldFlipCopy.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexIsMSDF.name, main2ndTexIsMSDF.floatValue);
                    hsvgMaterial.SetFloat(main2ndTexBlendMode.name, main2ndTexBlendMode.floatValue);
                    hsvgMaterial.SetTextureOffset(main2ndTex.name, material.GetTextureOffset(main2ndTex.name));
                    hsvgMaterial.SetTextureScale(main2ndTex.name, material.GetTextureScale(main2ndTex.name));
                    hsvgMaterial.SetTextureOffset(main2ndBlendMask.name, material.GetTextureOffset(main2ndBlendMask.name));
                    hsvgMaterial.SetTextureScale(main2ndBlendMask.name, material.GetTextureScale(main2ndBlendMask.name));

                    Object.DestroyImmediate(srcMain2);
                    srcMain2 = AssetUtility.LoadUncompressedTexture(material.GetTexture(main2ndTex.name));
                    if (srcMain2 != null)
                    {
                        hsvgMaterial.SetTexture(main2ndTex.name, srcMain2);
                    }
                    else
                    {
                        srcMain2 = new Texture2D(2, 2);
                        hsvgMaterial.SetTexture(main2ndTex.name, Texture2D.whiteTexture);
                    }

                    Object.DestroyImmediate(srcMask2);
                    srcMask2 = AssetUtility.LoadUncompressedTexture(material.GetTexture(main2ndBlendMask.name));
                    if (srcMask2 != null)
                    {
                        hsvgMaterial.SetTexture(main2ndBlendMask.name, srcMask2);
                    }
                    else
                    {
                        srcMask2 = new Texture2D(2, 2);
                        hsvgMaterial.SetTexture(main2ndBlendMask.name, Texture2D.whiteTexture);
                    }
                }

                if (bake3rd)
                {
                    hsvgMaterial.SetFloat(useMain3rdTex.name, useMain3rdTex.floatValue);
                    hsvgMaterial.SetColor(mainColor3rd.name, mainColor3rd.colorValue);
                    hsvgMaterial.SetFloat(main3rdTexAngle.name, main3rdTexAngle.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexIsDecal.name, main3rdTexIsDecal.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexIsLeftOnly.name, main3rdTexIsLeftOnly.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexIsRightOnly.name, main3rdTexIsRightOnly.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexShouldCopy.name, main3rdTexShouldCopy.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexShouldFlipMirror.name, main3rdTexShouldFlipMirror.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexShouldFlipCopy.name, main3rdTexShouldFlipCopy.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexIsMSDF.name, main3rdTexIsMSDF.floatValue);
                    hsvgMaterial.SetFloat(main3rdTexBlendMode.name, main3rdTexBlendMode.floatValue);
                    hsvgMaterial.SetTextureOffset(main3rdTex.name, material.GetTextureOffset(main3rdTex.name));
                    hsvgMaterial.SetTextureScale(main3rdTex.name, material.GetTextureScale(main3rdTex.name));
                    hsvgMaterial.SetTextureOffset(main3rdBlendMask.name, material.GetTextureOffset(main3rdBlendMask.name));
                    hsvgMaterial.SetTextureScale(main3rdBlendMask.name, material.GetTextureScale(main3rdBlendMask.name));

                    Object.DestroyImmediate(srcMain3);
                    srcMain3 = AssetUtility.LoadUncompressedTexture(material.GetTexture(main3rdTex.name));
                    if (srcMain3 != null)
                    {
                        hsvgMaterial.SetTexture(main3rdTex.name, srcMain3);
                    }
                    else
                    {
                        srcMain3 = new Texture2D(2, 2);
                        hsvgMaterial.SetTexture(main3rdTex.name, Texture2D.whiteTexture);
                    }

                    Object.DestroyImmediate(srcMask3);
                    srcMask3 = AssetUtility.LoadUncompressedTexture(material.GetTexture(main3rdBlendMask.name));
                    if (srcMask3 != null)
                    {
                        hsvgMaterial.SetTexture(main3rdBlendMask.name, srcMask3);
                    }
                    else
                    {
                        srcMask3 = new Texture2D(2, 2);
                        hsvgMaterial.SetTexture(main3rdBlendMask.name, Texture2D.whiteTexture);
                    }
                }

                RenderTexture dstTexture = new RenderTexture(srcTexture.width, srcTexture.height, 0, RenderTextureFormat.ARGB32);

                // Remember active render texture
                var activeRenderTexture = RenderTexture.active;
                Graphics.Blit(srcTexture, dstTexture, hsvgMaterial);

                Texture2D outTexture = new Texture2D(srcTexture.width, srcTexture.height);
                outTexture.ReadPixels(new Rect(0, 0, srcTexture.width, srcTexture.height), 0, 0);
                outTexture.Apply();

                // Restore active render texture
                RenderTexture.active = activeRenderTexture;
                Object.DestroyImmediate(hsvgMaterial);
                Object.DestroyImmediate(srcTexture);
                Object.DestroyImmediate(srcMain2);
                Object.DestroyImmediate(srcMain3);
                Object.DestroyImmediate(srcMask2);
                Object.DestroyImmediate(srcMask3);
                Object.DestroyImmediate(dstTexture);

                return outTexture;
            }
        }

        /// <summary>
        /// Additional step for emission.
        /// </summary>
        /// <param name="main">Baked main texture.</param>
        /// <param name="material">Material to bake.</param>
        private Texture2D EmissionBake(Texture2D main, Material material, TextureGeneratorSetting setting)
        {
            var shaderSetting = LoadShaderSetting();

            if (!shaderSetting.LIL_FEATURE_EMISSION_1ST && !shaderSetting.LIL_FEATURE_EMISSION_2ND)
            {
                var baked = new Texture2D(2, 2);
                baked.LoadImage(main.EncodeToPNG());
                baked.filterMode = FilterMode.Bilinear;
                return baked;
            }

            var mats = new[] { material };
            var emissionMap = MaterialEditor.GetMaterialProperty(mats, "_EmissionMap");
            var emissionBlendMask = MaterialEditor.GetMaterialProperty(mats, "_EmissionBlendMask");
            var emissionGradTex = MaterialEditor.GetMaterialProperty(mats, "_EmissionGradTex");
            var emission2ndMap = MaterialEditor.GetMaterialProperty(mats, "_Emission2ndMap");
            var emission2ndBlendMask = MaterialEditor.GetMaterialProperty(mats, "_Emission2ndBlendMask");
            var emission2ndGradTex = MaterialEditor.GetMaterialProperty(mats, "_Emission2ndGradTex");

            using (var baker = DisposableObject.New(Object.Instantiate(material)))
            using (var srcEmissionMap = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emissionMap.textureValue)))
            using (var srcEmissionBlendMask = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emissionBlendMask.textureValue)))
            using (var srcEmissionGradTex = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emissionGradTex.textureValue)))
            using (var srcEmission2ndMap = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emission2ndMap.textureValue)))
            using (var srcEmission2ndBlendMask = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emission2ndBlendMask.textureValue)))
            using (var srcEmission2ndGradTex = DisposableObject.New(AssetUtility.LoadUncompressedTexture(emission2ndGradTex.textureValue)))
            using (var dstTexture = DisposableObject.New(new RenderTexture(main.width, main.height, 0, RenderTextureFormat.ARGB32)))
            {
                var lilBaker = Shader.Find("Hidden/VRCQuestTools/lilToon");
                baker.Object.shader = lilBaker;
                baker.Object.SetFloat("_VQT_MainTexBrightness", setting.MainTextureBrightness);
                baker.Object.mainTexture = main;
                baker.Object.mainTextureOffset = new Vector2(0.0f, 0.0f);
                baker.Object.mainTextureScale = new Vector2(1.0f, 1.0f);
                baker.Object.color = Color.white;
                baker.Object.SetFloat("_LIL_FEATURE_EMISSION_1ST", shaderSetting.LIL_FEATURE_EMISSION_1ST ? 1.0f : 0.0f);
                baker.Object.SetFloat("_LIL_FEATURE_EMISSION_2ND", shaderSetting.LIL_FEATURE_EMISSION_2ND ? 1.0f : 0.0f);
                baker.Object.SetFloat("_LIL_FEATURE_ANIMATE_EMISSION_UV", shaderSetting.LIL_FEATURE_ANIMATE_EMISSION_UV ? 1.0f : 0.0f);
                baker.Object.SetFloat("_LIL_FEATURE_ANIMATE_EMISSION_MASK_UV", shaderSetting.LIL_FEATURE_ANIMATE_EMISSION_MASK_UV ? 1.0f : 0.0f);
                baker.Object.SetFloat("_LIL_FEATURE_EMISSION_GRADATION", shaderSetting.LIL_FEATURE_EMISSION_GRADATION ? 1.0f : 0.0f);

                baker.Object.SetTexture(emissionMap.name, srcEmissionMap.Object);
                baker.Object.SetTexture(emissionBlendMask.name, srcEmissionBlendMask.Object);
                baker.Object.SetTexture(emissionGradTex.name, srcEmissionGradTex.Object);
                baker.Object.SetTexture(emission2ndMap.name, srcEmission2ndMap.Object);
                baker.Object.SetTexture(emission2ndBlendMask.name, srcEmission2ndBlendMask.Object);
                baker.Object.SetTexture(emission2ndGradTex.name, srcEmission2ndGradTex.Object);

                // Remember active render texture
                var activeRenderTexture = RenderTexture.active;
                Graphics.Blit(main, dstTexture.Object, baker.Object);

                Texture2D outTexture = new Texture2D(main.width, main.height);
                outTexture.ReadPixels(new Rect(0, 0, main.width, main.height), 0, 0);
                outTexture.Apply();

                // Restore active render texture
                RenderTexture.active = activeRenderTexture;
                return outTexture;
            }
        }

        private bool CheckFeature(bool feature)
        {
            var isMulti = false;
            return isMulti || feature;
        }

        private class LilToonSetting
        {
            private static readonly SemVer LilToon130 = new SemVer("1.3.0");

            private Object settingObject;

            public LilToonSetting(Object obj)
            {
                settingObject = obj;
            }

            public bool LIL_FEATURE_MAIN_GRADATION_MAP => IsFeatureEnabled("LIL_FEATURE_MAIN_GRADATION_MAP");

            public bool LIL_FEATURE_EMISSION_1ST => IsFeatureEnabled("LIL_FEATURE_EMISSION_1ST");

            public bool LIL_FEATURE_EMISSION_2ND => IsFeatureEnabled("LIL_FEATURE_EMISSION_2ND");

            public bool LIL_FEATURE_ANIMATE_EMISSION_UV => IsFeatureEnabled("LIL_FEATURE_ANIMATE_EMISSION_UV");

            public bool LIL_FEATURE_ANIMATE_EMISSION_MASK_UV => IsFeatureEnabled("LIL_FEATURE_ANIMATE_EMISSION_MASK_UV");

            public bool LIL_FEATURE_EMISSION_GRADATION => IsFeatureEnabled("LIL_FEATURE_EMISSION_GRADATION");

            private bool IsFeatureEnabled(string name)
            {
                // lilToon v1.3.0 no longer needs shader setting in edit mode.
                if (AssetUtility.LilToonVersion >= LilToon130)
                {
                    return true;
                }
                return GetFieldValue<bool>(name);
            }

            private T GetFieldValue<T>(string name)
            {
                if (!AssetUtility.IsLilToonImported())
                {
                    throw new System.InvalidOperationException("lilToon not found in Assets.");
                }
                var lilToonSetting = SystemUtility.GetTypeByName("lilToonSetting");
                if (lilToonSetting == null)
                {
                    throw new System.InvalidOperationException($"lilToon found, but lilToonSetting not found");
                }
                var field = lilToonSetting.GetField(name);
                if (field == null)
                {
                    throw new System.MissingFieldException($"Field {lilToonSetting.Name}.{name} not found");
                }
                return (T)field.GetValue(settingObject);
            }
        }

        private class LilToonInspector
        {
            public static string GetShaderSettingPath()
            {
                return Invoke<string>("GetShaderSettingPath");
            }

            private static T Invoke<T>(string name)
                where T : class
            {
                if (!AssetUtility.IsLilToonImported())
                {
                    throw new System.InvalidOperationException("lilToon not found in Assets");
                }
                var lilToonInspector = SystemUtility.GetTypeByName("lilToon.lilToonInspector");
                if (lilToonInspector == null)
                {
                    throw new System.InvalidOperationException("lilToon found, but lilToon.lilToonInspector not found");
                }
                var method = lilToonInspector.GetMethod(name);
                if (method == null)
                {
                    throw new System.MissingMethodException($"{lilToonInspector.Name}.{name} not found");
                }
                return method.Invoke(null, null) as T;
            }
        }
    }
}
