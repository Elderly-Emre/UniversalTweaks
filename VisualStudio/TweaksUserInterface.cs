﻿using Il2CppTLD.OptionalContent;
using UniversalTweaks.Properties;
using static Il2Cpp.Panel_MainMenu.MainMenuItem;

namespace UniversalTweaks;

internal class TweaksUserInterface
{
    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.UpdateCrosshair))]
    internal static class AlwaysShowCrosshair
    {
        private static bool Prefix(HUDManager __instance)
        {
            if (Settings.Instance.AlwaysShowCrosshair)
            {
                __instance.m_CrosshairAlpha = 1f;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.GetShouldGreyOut))]
    private static class GreyOutSprayPaintRadial
    {
        private static bool Prefix(Panel_ActionsRadial.RadialType radialType, ref bool __result)
        {
            if (radialType == Panel_ActionsRadial.RadialType.SprayPaint)
            {
                if (GameManager.GetInventoryComponent().GetBestGearItemWithName("GEAR_SprayPaintCan") != null)
                {
                    __result = false;
                }
                else
                {
                    __result = true;
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.Initialize))]
    private static class FireSpriteFix
    {
        private static void Postfix(Panel_FeedFire __instance)
        {
            if (__instance.m_Sprite_FireFill != null)
            {
                __instance.m_Sprite_FireFill.gameObject.transform.localPosition = new Vector3(159.1f, -31.6f, 0);
                __instance.m_Sprite_FireFill.gameObject.transform.localScale = new Vector3(1.7f, 1.7f, 1);
            }
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.Initialize))]
    private class RemoveOptionalContentMenus
    {
        private static void Postfix(Panel_MainMenu __instance)
        {
            OptionalContentManager contentManager = OptionalContentManager.Instance;
            bool hasWintermute = contentManager.IsContentOwned(__instance.m_WintermuteConfig);

            RemoveMainMenuItem(MainMenuItemType.TFTFTUpsell, __instance);

            if (!hasWintermute)
            {
                RemoveMainMenuItem(MainMenuItemType.Story, __instance);
            }
        }

        private static void RemoveMainMenuItem(MainMenuItemType removeType, Panel_MainMenu __instance)
        {
            for (int i = __instance.m_MenuItems.Count - 1; i >= 0; i--)
            {
                if (__instance.m_MenuItems[i].m_Type == removeType)
                {
                    __instance.m_MenuItems.RemoveAt(i);
                }
            }

            __instance.ConfigureMenu();
            __instance.m_BasicMenu.Refresh();
        }
    }
}