namespace ScuutCore.Patches
{
    using HarmonyLib;
    using ScuutCore.Modules.Teslas;

    [HarmonyPatch(typeof(TeslaGate), nameof(TeslaGate.PlayerInRange))]
    public static class TeslaInRangePatch
    {
        public static bool Prefix(TeslaGate __instance, ref bool __result, ReferenceHub player)
        {
            if(Teslas.Singleton == null || !Teslas.Singleton.Config.IsEnabled)
                return true;
            if (Teslas.Singleton.Config.Roles.Contains(player.roleManager.CurrentRole.RoleTypeId))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}