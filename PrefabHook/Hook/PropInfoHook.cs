using System;
using System.Reflection;
using UnityEngine;

namespace PrefabHook
{
    public class PropInfoHook : PropInfo
    {
        private static bool deployed = false;

        private static RedirectCallsState _InitializePrefab_state;
        private static MethodInfo _InitializePrefab_original;
        private static MethodInfo _InitializePrefab_detour;

        public static event PrefabEventHandler<PropInfo> OnPreInitialization;
        public static event PrefabEventHandler<PropInfo> OnPostInitialization;

        public static void Deploy()
        {
            if (!deployed && (OnPreInitialization != null || OnPostInitialization != null))
            {
                _InitializePrefab_original = typeof(PropInfo).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_detour = typeof(PropInfoHook).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_state = RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);

                deployed = true;

                Debug.LogFormat("PrefabHook: {0} Methods detoured!", "PropInfo");
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

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "PropInfo");
            }
        }

        public new virtual void InitializePrefab()
        {
            if (OnPreInitialization != null)
            {
                try
                {
                    OnPreInitialization(base.GetComponent<PropInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
            try
            {
                base.InitializePrefab();
            }
            finally
            {
                RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);
            }

            if (OnPostInitialization != null)
            {
                try
                {
                    OnPostInitialization(base.GetComponent<PropInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
