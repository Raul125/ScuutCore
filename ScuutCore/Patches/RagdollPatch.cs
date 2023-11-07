namespace ScuutCore.Patches;

using Commands.Suicide;
using HarmonyLib;
using Modules.Patreon;
using Modules.Patreon.Commands;
using NWAPIPermissionSystem;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using ScuutCore.API.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

[HarmonyPatch(typeof(RagdollManager), nameof(RagdollManager.ServerSpawnRagdoll))]
public static class RagdollPatch
{
    private static void ProcessRagdoll(ReferenceHub player, DamageHandlerBase handler)
    {
        if (PatreonPerksModule.Singleton is null || !player.queryProcessor._sender.CheckPermission(FlyingRagdollSelf.RagdollPermissions))
            return;

        if (!player.TryGetComponent(out PatreonData data) || !data.Prefs.FlyingRagdollSelf)
            return;

        if (handler is not CustomReasonDamageHandler standard || standard._deathReason != ExplosiveSuicide.DeathReason)
            return;

        var random = Random.onUnitSphere;
        random.y = Mathf.Abs(random.y);

        standard.StartVelocity = random * PatreonPerksModule.Singleton.Config.RagdollFlyMultiplier * 5;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> newInstructions = instructions.BeginTranspiler();

        int index = newInstructions.FindIndex(i => i.operand is MethodInfo methodInfo && methodInfo.Name == nameof(Object.Instantiate)) - 3;

        // RagdollPatch.ProcessRagdoll(owner, handler);
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldarg_1),
            new(OpCodes.Call, AccessTools.Method(typeof(RagdollPatch), nameof(ProcessRagdoll)))
        });

        return newInstructions.FinishTranspiler();
    }
}
