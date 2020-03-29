using Harmony;
using ICities;

namespace PrefabHook
{
    public class PrefabHookMod : IUserMod
    {
        
        public static readonly HarmonyInstance Harmony = HarmonyInstance.Create("github.com/boformer/PrefabHook");
        
        public string Name => "Prefab Hook";
        public string Description => "Utility mod, required by a few other mods";
    }
}
