namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Modules.PocketFall;
    using NorthwoodLib.Pools;
    using PlayerRoles.PlayableScps.Scp106;
    [HarmonyPatch(typeof(Scp106Attack), nameof(Scp106Attack.ServerShoot))]
    internal static class Scp106Teleport
    {

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
            if (PocketFall.Instance.Config.IsEnabled)
            {
                int index = list.Count - 5;
                list.RemoveRange(index, 3);
                list.InsertRange(index, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(Scp106Attack), nameof(Scp106Attack._targetHub)),
                    CodeInstruction.Call(typeof(EventHandlers), nameof(EventHandlers.Send)),
                });
            }
            foreach (var codeInstruction in list)
                yield return codeInstruction;
            ListPool<CodeInstruction>.Shared.Return(list);
        }

    }

}