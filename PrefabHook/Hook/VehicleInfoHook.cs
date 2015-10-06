using System;
using System.Reflection;
using UnityEngine;

namespace PrefabHook
{
    public class VehicleInfoHook : VehicleInfo
    {
        private static bool deployed = false;

        private static RedirectCallsState _InitializePrefab_state;
        private static MethodInfo _InitializePrefab_original;
        private static MethodInfo _InitializePrefab_detour;

        public static event PrefabEventHandler<VehicleInfo> OnPreInitialization;
        public static event PrefabEventHandler<VehicleInfo> OnPostInitialization;

        public static void Deploy()
        {
            if (!deployed && (OnPreInitialization != null || OnPostInitialization != null))
            {
                _InitializePrefab_original = typeof(VehicleInfo).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_detour = typeof(VehicleInfoHook).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_state = RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);

                deployed = true;

                Debug.LogFormat("PrefabHook: {0} Methods detoured!", "VehicleInfo");
            }
        }

        public static void Revert()
        {
            if (deployed)
            {
                RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
                _InitializePrefab_original = null;
                _InitializePrefab_detour = null;

                OnPreInitialization = null;
                OnPostInitialization = null;

                deployed = false;

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "VehicleInfo");
            }
        }

        public new virtual void InitializePrefab()
        {
            if (OnPreInitialization != null)
            {
                try
                {
                    OnPreInitialization(base.GetComponent<VehicleInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
            base.InitializePrefab();
            RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);

            if (OnPostInitialization != null)
            {
                try
                {
                    OnPostInitialization(base.GetComponent<VehicleInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
