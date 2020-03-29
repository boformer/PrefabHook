﻿using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace PrefabHook
{
    public class CitizenInfoHook
    {
        private static bool deployed = false;

        private static MethodInfo OriginalMethod => typeof(CitizenInfo).GetMethod(nameof(CitizenInfo.InitializePrefab),
            BindingFlags.Instance | BindingFlags.Public);

        private static MethodInfo Prefix => typeof(CitizenInfoHook).GetMethod(nameof(PreInitializePrefab),
            BindingFlags.Static | BindingFlags.NonPublic);

        private static MethodInfo Postfix => typeof(CitizenInfoHook).GetMethod(nameof(PostInitializePrefab),
            BindingFlags.Static | BindingFlags.NonPublic);


        public static event PrefabEventHandler<CitizenInfo> OnPreInitialization;
        public static event PrefabEventHandler<CitizenInfo> OnPostInitialization;

        public static void Deploy()
        {
            if (!deployed && (OnPreInitialization != null || OnPostInitialization != null))
            {
                PrefabHookMod.Harmony.Patch(OriginalMethod, new HarmonyMethod(Prefix), new HarmonyMethod(Postfix),
                    null);

                deployed = true;

                Debug.LogFormat("PrefabHook: {0} Methods patched with Harmony!", "CitizenInfo");
            }
        }

        public static void Revert()
        {
            if (deployed)
            {
                PrefabHookMod.Harmony.Unpatch(OriginalMethod, HarmonyPatchType.All);

                OnPreInitialization = null;
                OnPostInitialization = null;

                deployed = false;

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "CitizenInfo");
            }
        }

        private static bool PreInitializePrefab(CitizenInfo __instance)
        {
            if (OnPreInitialization != null)
            {
                try
                {
                    OnPreInitialization(__instance);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return true;
        }

        private static void PostInitializePrefab(CitizenInfo __instance)
        {
            if (OnPostInitialization != null)
            {
                try
                {
                    OnPostInitialization(__instance);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}