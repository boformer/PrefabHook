using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace PrefabHook
{
    public class TransportInfoHook
    {
        private static bool deployed = false;

        private static MethodInfo OriginalMethod => typeof(TransportInfo).GetMethod(
            nameof(TransportInfo.InitializePrefab),
            BindingFlags.Instance | BindingFlags.Public);

        private static MethodInfo Prefix => typeof(TransportInfoHook).GetMethod(nameof(PreInitializePrefab),
            BindingFlags.Static | BindingFlags.NonPublic);

        private static MethodInfo Postfix => typeof(TransportInfoHook).GetMethod(nameof(PostInitializePrefab),
            BindingFlags.Static | BindingFlags.NonPublic);


        public static event PrefabEventHandler<TransportInfo> OnPreInitialization;
        public static event PrefabEventHandler<TransportInfo> OnPostInitialization;

        public static void Deploy()
        {
            if (!deployed && (OnPreInitialization != null || OnPostInitialization != null))
            {
                PrefabHookMod.Harmony.Patch(OriginalMethod, new HarmonyMethod(Prefix), new HarmonyMethod(Postfix),
                    null);

                deployed = true;

                Debug.LogFormat("PrefabHook: {0} Methods patched with Harmony!", "TransportInfo");
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

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "TransportInfo");
            }
        }

        private static bool PreInitializePrefab(TransportInfo __instance)
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

        private static void PostInitializePrefab(TransportInfo __instance)
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