namespace ScuutCore.Patches;

using AFK;
using HarmonyLib;
using ScuutCore.Modules.AntiAFK;

[HarmonyPatch(typeof(AFKManager), nameof(AFKManager.ConfigReloaded))]
public static class DisableAfkPatch
{
    // there is a better way to do this, but im too lazy to do it in this config.
    public static bool Prefix() => !AntiAFK.Singleton?.Config.DisableBaseGameAfk ?? true;
}