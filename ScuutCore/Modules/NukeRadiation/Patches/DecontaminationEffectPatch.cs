namespace ScuutCore.Modules.NukeRadiation.Patches
{
    using CustomPlayerEffects;
    using HarmonyLib;
    using ScuutCore.Modules.NukeRadiation.Components;

    [HarmonyPatch(typeof(Decontaminating), nameof(Decontaminating.OnTick))]
    public static class DecontaminationEffectPatch
    {
        public static bool Prefix(Decontaminating __instance)
        {
            if (__instance.Hub.gameObject.TryGetComponent<NukeRadiationComponent>(out var comp) && comp.IsInNukeZone)
                return false;
            return true;
        }
    }
}