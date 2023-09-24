namespace ScuutCore.Patches;

using HarmonyLib; 
using PlayerRoles.RoleAssign;
using ScuutCore.Modules.ChooseRole;

[HarmonyPatch(typeof(RoleAssigner), nameof(RoleAssigner.OnRoundStarted))] 
public static class PreventRandomSpawning 
{ 
    private static bool Prefix() => !(ChooseRole.Singleton?.Config.IsEnabled ?? false); 
}