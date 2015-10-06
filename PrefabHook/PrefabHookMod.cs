using ICities;

namespace PrefabHook
{
    public class PrefabHookMod : IUserMod
    {
        public string Name
        {
            get { return "Prefab Hook"; }
        }
        public string Description
        {
            get { return "Utility mod, required by a few other mods"; }
        }
    }
}
