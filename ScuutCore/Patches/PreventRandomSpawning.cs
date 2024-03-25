namespace ScuutCore.Patches;

using HarmonyLib;
using PlayerRoles.RoleAssign;
using ScuutCore.Modules.ChooseRole;

[HarmonyPatch(typeof(RoleAssigner), nameof(RoleAssigner.OnRoundStarted))]
public static class PreventRandomSpawning
{
    private static bool Prefix()
    {
        if (ChooseRole.Singleton is null)
            return true;

        RoleAssigner._spawned = true;
        RoleAssigner.LateJoinTimer.Restart();
        return false;
    }
}