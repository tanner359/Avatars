﻿// <copyright file="I18nEnglish.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable SA1600 // Elements should be documented

namespace KRT.VRCQuestTools.I18n
{
    /// <summary>
    /// English strings.
    /// </summary>
    internal class I18nEnglish : I18nBase
    {
        internal override string CancelLabel => "Cancel";
        internal override string CloseLabel => "Close";
        internal override string DismissLabel => "Dismiss";

        // Convert Avatar for Quest
        internal override string OverwriteWarningDialogButtonCancel => "Cancel";
        internal override string OverwriteWarningDialogButtonOK => "OK";
        internal override string OverwriteWarningDialogButtonUseAltDir(string altDir) => $"Use \"{altDir}\"";
        internal override string OverwriteWarningDialogMessage(string artifactsDir) => $"\"{artifactsDir}\" already exists. Do you want to overwrite?";
        internal override string OverwriteWarningDialogTitle => $"{VRCQuestTools.Name} Warning";
        internal override string AvatarLabel => "Avatar";
        internal override string GenerateQuestTexturesLabel => "Generate Textures for Quest";
        internal override string QuestTexturesDescription => "By generating new textures which applying material's parameters not only main textures, get closer to PC version of the avatar";
        internal override string SupportedShadersLabel => "Supported Shaders";
        internal override string SaveToLabel => "Folder to Save";
        internal override string SelectButtonLabel => "Select";
        internal override string ConvertButtonLabel => "Convert";
        internal override string UpdateTexturesLabel => "Update Only Quest Textures";
        internal override string AdvancedConverterSettingsLabel => "Advanced Converter Settings";
        internal override string AnimationOverrideLabel => "Animation Override";
        internal override string AnimationOverrideTooltip => "Convert Animator Controllers with Animator Override Controller's animations.";
        internal override string AnimationOverrideMaterialErrorMessage => "Animator Override Controllers contain animated materials which uses unsupported shaders for Quest.";
        internal override string ConvertingMaterialsDialogMessage => "Converting materials...";
        internal override string GeneratingTexturesDialogMessage => "Generating textures...";
        internal override string MaterialExceptionDialogMessage => "An error occured when converting materials. Aborted.";
        internal override string AnimationClipExceptionDialogMessage => "An error occured when converting Animation Clips. Aborted.";
        internal override string AnimatorControllerExceptionDialogMessage => "An error occured when converting Animator Controllers. Aborted.";
        internal override string WarningForPerformance => $"{VRCQuestTools.Name} never optimize performance rank such as polygon reduction. In most cases, the converted avatar's performance rank will be \"Very Poor\" for Quest. Quest users see your fallback avatar by default, and need to change \"Avatar Display\" setting.";
        internal override string WarningForAppearance => "Texture's transparency doesn't make any effects, so this will be an issue for facial expression. In this case, please take steps by yourself (for example, by editing animation clips or deleting problematic meshes).\n\n" +
            "You should check converted avatar's appearance on PC by uploading with another Blueprint ID or using Avatars 3.0 local testing.";
        internal override string WarningForUnsupportedShaders => $"Following materials are using unsupported shaders. Textures might not properly be generated.\nDisabling \"{GenerateQuestTexturesLabel}\" option changes only shader.";
        internal override string AlertForComponents => "Following unsupported components will be removed. Check avatar features after conversion.";
        internal override string AlertForMaterialAnimation => "There are Animation clips which change avatar's materials. Animator Controllers and Animation clips will be duplicated then converted for Quest.";
        internal override string AlertForDynamicBoneConversion => $"{VRCQuestTools.Name} doesn't convert Dynamic Bones to PhysBones. Please set up PhysBones before converting the avatar.";
        internal override string AlertForAvatarDynamicsPerformance => "Avatar Dynamics components exceed \"Poor\" limits (Very Poor). Please keep \"Poor\" rating by removing them.";
        internal override string TexturesSizeLimitLabel => "Textures Size Limit";
        internal override string MainTextureBrightnessLabel => "Main Texture Brightness";
        internal override string MainTextureBrightnessTooltip => "Tweak main texture color for Toon Lit.";
        internal override string CompletedDialogMessage(string originalName) => $"{originalName} has been converted for Quest.\nTest your avatar such as facial expression then upload it for Android platform by using same Blueprint ID as PC version.";

        // Remove Missing Components
        internal override string NoMissingComponentsMessage(string objectName) => $"There are no \"missing\" components in {objectName}.";
        internal override string MissingRemoverConfirmationMessage(string objectName) => $"Remove \"missing\" components from {objectName}.";
        internal override string UnpackPrefabMessage => "This also executes \"Unpack Prefab\" operation.";

        // BlendShapes Copy
        internal override string SourceMeshLabel => "Source Mesh";
        internal override string TargetMeshLabel => "Target Mesh";
        internal override string CopyButtonLabel => "Copy BlendShape Weights";
        internal override string SwitchButtonLabel => "Switch Source/Target";

        // Remove Unsupported Components
        internal override string NoUnsupportedComponentsMessage(string objectName) => $"There are no unsupported components in {objectName}.";
        internal override string UnsupportedRemoverConfirmationMessage(string objectName) => $"Remove following unsupported components from {objectName}.";

        // Remove PhysBones
        internal override string PhysBonesSDKRequired => "VRCSDK which supports Avatar Dynamics is required.";
        internal override string SelectComponentsToKeep => "Select components to keep.";
        internal override string PhysBonesListTooltip => "The list of components and their root transforms.";
        internal override string KeepAll => "Keep All";
        internal override string PhysBonesWillBeRemovedAtRunTime => "You can't upload this avatar for Quest. Please reduce PhysBone components.";
        internal override string PhysBoneCollidersWillBeRemovedAtRunTime => "All PhysBone colliders will be removed at runtime on Quest. Please reduce PhysBoneCollider components.";
        internal override string ContactsWillBeRemovedAtRunTime => "You can't upload this avatar for Quest. Please reduce VRCContact components.";
        internal override string PhysBonesOrderMustMatchWithPC => "To properly synchronize PhysBones, the order of PhysBones must match with the PC avatar. Please select PhysBones from the top of the list.";
        internal override string DeleteUnselectedComponents => "Delete Unselected Components";

        // Metallic Smoothness
        internal override string TextureLabel => "Texture";
        internal override string InvertLabel => "Invert";
        internal override string SaveFileDialogTitle(string thing) => $"Save {thing}";
        internal override string SaveFileDialogMessage => "Please enter a file name to save the texture to";
        internal override string GenerateButtonLabel => "Generate Metallic Smoothness";

        // Unity Settings
        internal override string CacheServerModeLabel => "Cache Server Mode";
        internal override string CacheServerHelp => "By enabling the local cache server, you can save time for such as texture compression from the next. In default preferences, the server takes 10 GB from C drive at maximum.";
        internal override string CacheServerButtonLabel => "Enable Local Cache Server";
        internal override string TextureCompressionLabel => "Android Texture Compression";
        internal override string TextureCompressionHelp => "ASTC improves Quest texture quality in exchange for long compression time";
        internal override string TextureCompressionButtonLabel => "Set texture compression to ASTC";
        internal override string ApplyAllButtonLabel => "Apply All Settings";
        internal override string ShowOnStartupLabel => "Show on startup";
        internal override string AllAppliedHelp => "OK, all recommended settings are applied.";

        // Check for Update
        internal override string CheckLater => "Check Later";
        internal override string GetUpdate => "Get Update";
        internal override string SkipThisVersion => "Skip This";
        internal override string NewVersionIsAvailable(string latestVersion) => $"New version {latestVersion} is available.";
        internal override string NewVersionHasBreakingChanges => $"This version might have breaking changes about compatibility.";
        internal override string ThereIsNoUpdate => "There is no update.";

        // Validations
        internal override string DeactivateAvatar => "Deactivate avatar";
        internal override string IncompatibleForQuest => "On Android build target, you can't upload Quest avatars because this avatar can't be uploaded for Quest. Please deactivate such avatars or switch platfrom back to PC.";
        internal override string MissingScripts => "You can't upload an avatar which has \"missing\" scripts. Please remove these components to build.";
        internal override string MissingDynamicBone => "Dynamic Bone is missing in the project. Please import Dynamic Bone or remove \"missing\" components to build.";
        internal override string RemoveMissing => "Remove missing components";
        internal override string ValidatorAlertsProhibitedShaders(string shaderName, string[] materialNames) =>
            $"Shader \"{shaderName}\" (Materials: {string.Join(", ", materialNames)}) is not allowed for Quest.";
        internal override string ValidatorAlertsUnsupportedComponents(string componentName, string objectName) =>
            $"Component \"{componentName}\" ({objectName}) is not allowed for Quest.";
        internal override string ValidatorAlertsVeryPoorPhysBones(int count) =>
            $"Too many PhysBones: {count} (Very Poor).";
        internal override string ValidatorAlertsVeryPoorPhysBoneColliders(int count) =>
            $"Too many PhysBoneColliders: {count} (Very Poor).";
        internal override string ValidatorAlertsVeryPoorContacts(int count) =>
            $"Too many ContactSenders and ContactReceivers: {count} (Very Poor).";
    }
}
