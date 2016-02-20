namespace PrefabHook
{
    public delegate void PrefabEventHandler<in T>(T prefab) where T : PrefabInfo;
}
