namespace ScuutCore.Patches;

using HarmonyLib;
using Mirror;
using PlayerRoles.PlayableScps.Scp106;
using ScuutCore.API.Extensions;
using System.Collections.Generic;
using System.Reflection;
using Utils.Networking;

[HarmonyPatch(typeof(Scp106PocketItemManager), nameof(Scp106PocketItemManager.Update))]
internal static class PocketItemUpdatePatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> newInstructions = instructions.BeginTranspiler();

        // This patch replaces a NetworkServer.SendToAll call with NetworkUtils.SendToAuthenticated.
        // Presumably to prevent some error.

        int index = newInstructions.FindIndex(i => i.operand is MethodInfo methodInfo && methodInfo.Name == nameof(NetworkServer.SendToAll));

        newInstructions[index] = CodeInstruction.Call(typeof(NetworkUtils), nameof(NetworkUtils.SendToAuthenticated), null, new[]
        {
            typeof(Scp106PocketItemManager.WarningMessage)
        });

        newInstructions.RemoveAt(index - 1);

        return newInstructions.FinishTranspiler();
    }
}
