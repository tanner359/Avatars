﻿// <copyright file="TestUtils.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using KRT.VRCQuestTools.Models.Unity;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace KRT.VRCQuestTools
{
    /// <summary>
    /// Test utility.
    /// </summary>
    internal static class TestUtils
    {
        /// <summary>
        /// Gets a value indicating whether DynamicBone is imported.
        /// </summary>
        internal static bool HasDynamicBone => AssetDatabase.GUIDToAssetPath("f9ac8d30c6a0d9642a11e5be4c440740") != string.Empty; // DynamicBone.cs

        /// <summary>
        /// Gets a value indicating whether FinalIK is imported.
        /// </summary>
        internal static bool HasFinalIK => AssetDatabase.GUIDToAssetPath("e8ad84abaddc346b9a51365d3dc292e7") != string.Empty; // IK.cs

        /// <summary>
        /// Gets Test fixtures folder.
        /// </summary>
        internal static string FixturesFolder => VRCQuestTools.AssetRoot + "/Tests/Fixtures";

        /// <summary>
        /// Gets Materials folder.
        /// </summary>
        internal static string MaterialsFolder => FixturesFolder + "/Materials";

        /// <summary>
        /// Gets Textures folder.
        /// </summary>
        internal static string TexturesFolder => FixturesFolder + "/Textures";

        /// <summary>
        /// Load material from materials folder.
        /// </summary>
        /// <param name="file">File name.</param>
        /// <returns>Material.</returns>
        internal static Material LoadMaterial(string file)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(MaterialsFolder + "/" + file);
            Assert.NotNull(material);
            return material;
        }

        /// <summary>
        /// Load uncompressed texture from textures folder.
        /// </summary>
        /// <param name="file">File name.</param>
        /// <returns>Texture.</returns>
        internal static Texture2D LoadUncompressedTexture(string file)
        {
            Assert.True(file.EndsWith(".png"));
            var bytes = System.IO.File.ReadAllBytes(TexturesFolder + "/" + file);
            Assert.NotZero(bytes.Length);
            var tex = new Texture2D(4, 4);
            tex.LoadImage(bytes);
            return tex;
        }

        /// <summary>
        /// Calculates average difference between two textures.
        /// </summary>
        /// <param name="tex1">Texture 1.</param>
        /// <param name="tex2">Texture 2.</param>
        /// <returns>Average of pixel difference.</returns>
        internal static float Difference(Texture2D tex1, Texture2D tex2)
        {
            var pixels1 = tex1.GetPixels32();
            var pixels2 = tex2.GetPixels32();

            Assert.AreEqual(pixels1.Length, pixels2.Length);

            long dsum = 0;
            for (var i = 0; i < pixels1.Length; i++)
            {
                var c1 = pixels1[i];
                var c2 = pixels2[i];
                var r = c1.r - c2.r;
                var g = c1.g - c2.g;
                var b = c1.b - c2.b;
                var a = c1.a - c2.a;
                var diff = r * r + g * g + b * b + a * a;
                dsum += diff;
                if (diff > 0)
                {
                    // Debug.Log($"{i}, {diff}");
                }
            }

            return dsum / (float)(255L * 255L * 4L * pixels1.Length);
        }

        /// <summary>
        /// Load MaterialBase from materials folder.
        /// </summary>
        /// <param name="file">File name.</param>
        /// <returns>MaterialBase.</returns>
        internal static MaterialBase LoadMaterialWrapper(string file)
        {
            var material = LoadMaterial(file);
            var wrapper = new MaterialWrapperBuilder().Build(material);
            Assert.NotNull(wrapper);
            return wrapper;
        }

        /// <summary>
        /// Get GUID for an asset.
        /// </summary>
        /// <param name="obj">Object of target asset.</param>
        /// <returns>GUID.</returns>
        internal static string GetAssetGUID(Object obj)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localId);
            return guid;
        }

        /// <summary>
        /// Check the shader exists. If the shader is missing, test is ignored.
        /// </summary>
        /// <param name="name">Shader name.</param>
        internal static void AssertIgnoreOnMissingShader(string name)
        {
            var shader = Shader.Find(name);
            if (shader == null)
            {
                Assert.Ignore($"\"{name}\" shader not found");
            }
        }
    }
}
