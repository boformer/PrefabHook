namespace PrefabHook
{
    public delegate void PrefabEventHandler<P>(P prefab) where P : PrefabInfo;
}
