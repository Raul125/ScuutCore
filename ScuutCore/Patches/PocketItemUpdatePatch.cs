namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using System.Reflection;
    using HarmonyLib;
    using Mirror;
    using PlayerRoles.PlayableScps.Scp106;
    using Utils.Networking;
    [HarmonyPatch(typeof(Scp106PocketItemManager), nameof(Scp106PocketItemManager.Update))]
    internal static class PocketItemUpdatePatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = new List<CodeInstruction>(instructions);
            int index = list.FindIndex(i => i.operand is MethodInfo { Name: nameof(NetworkServer.SendToAll) });
            list[index] = CodeInstruction.Call(typeof(NetworkUtils), nameof(NetworkUtils.SendToAuthenticated), null, new[]
            {
                typeof(Scp106PocketItemManager.WarningMessage)
            });
            list.RemoveAt(index - 1);
            return list;
        }

    }

}