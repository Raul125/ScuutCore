namespace ScuutCore.Patches
{
    using AFK;
    using HarmonyLib;
    using ScuutCore.Modules.AntiAFK;

    [HarmonyPatch(typeof(AFKManager), nameof(AFKManager.ConfigReloaded))]
    public static class DisableAfkPatch
    {
        public static bool Prefix() => !AntiAFK.Singleton?.Config.DisableBaseGameAfk ?? true;
    }
}