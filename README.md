# Prefab Hook
Mod for Cities: Skylines

Used by different mods to hook into the asset loading process of the game.

This mod provides hooks for ``BuildingInfo``, ``NetInfo``, ``PropInfo``, ``TreeInfo`` and ``VehicleInfo``.

Usage:
------

Register an event handler in the ``OnCreated`` method of your mod. **After that**, deploy the hook. Revert the hook in your ``OnReleased`` method.

Always check if the Prefab Hook mod is installed. Display a warning if it is not installed.

You can use this mod to safely modify existing prefabs (e.g. replace textures, change service classification), or to clone prefabs.

The hooks are called for default and custom prefabs.

Example:
--------

```c#
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using PrefabHook;
using UnityEngine;

public class ModLoader : LoadingExtensionBase
{
    public override void OnCreated(ILoading loading)
    {
        base.OnCreated(loading);

        // cancel if Prefab Hook is not installed
        if (!IsHooked()) return;

        // register event handlers
        BuildingInfoHook.OnPreInitialization += OnPreBuildingInit;
        BuildingInfoHook.OnPostInitialization += OnPostBuildingInit;
        
        // deploy (after event handler registration!)
        BuildingInfoHook.Deploy();
    }
    
    public override void OnLevelLoaded(LoadMode mode)
    {
        base.OnLevelLoaded(mode);

        // display warning when level is loaded if Prefab Hook is not installed
        if (!IsHooked())
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                "Missing dependency", 
                "<YOUR MOD NAME> requires the 'Prefab Hook' mod to work properly. Please subscribe to the mod and restart the game!", 
                false);
            return;
        }
        
        // other stuff...
    }
    
    public override void OnReleased()
    {
        base.OnReleased();

        if (!IsHooked()) return;
        
        // revert on release
        BuildingInfoHook.Revert();
    }
    
    
    // This event handler is called before building initialization
    public void OnPreBuildingInit(BuildingInfo info) 
    { 
        // your code here
        Debug.Log("Game is now initializing BuildingInfo " + info.Name);
    }
    
    // This event handler is called after building initialization
    public void OnPostBuildingInit(BuildingInfo info) 
    { 
        // your code here
        Debug.Log("Game has initialized BuildingInfo " + info.Name);
    }

    
    // checks if the player subscribed to the Prefab Hook mod
    private bool IsHooked()
    {
        foreach (PluginManager.PluginInfo current in PluginManager.instance.GetPluginsInfo())
        {
            if (current.publishedFileID.AsUInt64 == 530771650uL) return true;
        }
        return false;
    }
}
```

---------

Using the [C# detours](https://github.com/sschoener/cities-skylines-detour) code created by Sebastian Schöner, released under the MIT license.
