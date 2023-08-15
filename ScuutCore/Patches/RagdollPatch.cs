namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Commands.Suicide;
    using HarmonyLib;
    using Modules.Patreon;
    using Modules.Patreon.Commands;
    using NorthwoodLib.Pools;
    using NWAPIPermissionSystem;
    using PlayerRoles.Ragdolls;
    using PlayerStatsSystem;
    using UnityEngine;

    [HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
    public static class RagdollPatch
    {
        public static void ProcessRagdoll(ReferenceHub player, DamageHandlerBase handler)
        {
            if (player == null || string.IsNullOrWhiteSpace(player.characterClassManager.UserId) || !player.queryProcessor._sender.CheckPermission(FlyingRagdollSelf.RagdollPermissions))
                return;
            if (!player.TryGetComponent(out PatreonData data) || !data.Prefs.FlyingRagdollSelf)
                return;
            if (handler is not CustomReasonDamageHandler { _deathReason: ExplosiveSuicide.DeathReason } standard)
                return;
            var random = Random.onUnitSphere;
            random.y = Mathf.Abs(random.y);
            var velocity = random * PatreonPerksModule.Singleton.Config.RagdollFlyMultiplier * 5;
            standard.StartVelocity = velocity;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = list.FindIndex(i => i.operand is MethodInfo { Name: nameof(Object.Instantiate) }) - 3;
            list.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(list[index]),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RagdollPatch), nameof(ProcessRagdoll)))
            });
            foreach (var codeInstruction in list)
                yield return codeInstruction;
            ListPool<CodeInstruction>.Shared.Return(list);
        }

    }

}