﻿// <copyright file="AvatarConverterMenu.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using KRT.VRCQuestTools.Utils;
using KRT.VRCQuestTools.Views;
using UnityEditor;

#if VRC_SDK_VRCSDK2 || VRC_SDK_VRCSDK3
using VRC_AvatarDescriptor = VRC.SDKBase.VRC_AvatarDescriptor;
#else
using VRC_AvatarDescriptor = KRT.VRCQuestTools.Mocks.Mock_VRC_AvatarDescriptor;
#endif

namespace KRT.VRCQuestTools.Menus
{
    /// <summary>
    /// Menu for avatar converter.
    /// </summary>
    internal static class AvatarConverterMenu
    {
        [MenuItem(VRCQuestToolsMenus.MenuPaths.ConvertAvatarForQuest, false, (int)VRCQuestToolsMenus.MenuPriorities.ConvertAvatarForQuest)]
        private static void InitFromMenu()
        {
            var target = Selection.activeGameObject;
            if (target != null && VRCSDKUtility.IsAvatarRoot(target))
            {
                var avatar = target.GetComponent<VRC_AvatarDescriptor>();
                AvatarConverterWindow.ShowWindow(avatar);
            }
            else
            {
                AvatarConverterWindow.ShowWindow();
            }
        }

        [MenuItem(VRCQuestToolsMenus.GameObjectMenuPaths.ConvertAvatarForQuest, false, (int)VRCQuestToolsMenus.GameObjectMenuPriorities.GameObjectConvertAvatarForQuest)]
        private static void InitFromContextForGameObject()
        {
            InitFromMenu();
        }

        [MenuItem(VRCQuestToolsMenus.GameObjectMenuPaths.ConvertAvatarForQuest, true)]
        private static bool ValidateContextForGameObject()
        {
            var obj = Selection.activeGameObject;
            return VRCSDKUtility.IsAvatarRoot(obj);
        }
    }
}
