using System;

namespace PrefabHook.ReflectiveApi
{
    public static class BuildingInfoHookReflective
    {
        public static void RegisterPreInitializationHook(Action<BuildingInfo> action)
        {
            BuildingInfoHook.OnPreInitialization += action.Invoke;
        }

        public static void RegisterPostInitializationHook(Action<BuildingInfo> action)
        {
            BuildingInfoHook.OnPostInitialization += action.Invoke;
        }

        public static void Deploy()
        {
            BuildingInfoHook.Deploy();
        }

        public static void Revert()
        {
            BuildingInfoHook.Revert();
        }
    }
}