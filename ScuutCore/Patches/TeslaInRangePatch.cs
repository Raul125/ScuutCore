namespace ScuutCore.Patches;

using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Modules.Teslas;
using ScuutCore.API.Extensions;

[HarmonyPatch(typeof(TeslaGate), nameof(TeslaGate.PlayerInRange))]
public static class TeslaInRangePatch
{
    public static bool DisableTriggering(ReferenceHub hub) => Teslas.Singleton?.Config?.Roles.Contains(hub.roleManager.CurrentRole.RoleTypeId) ?? false;

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructiosns = instructions.BeginTranspiler();

        Label allowLabel = generator.DefineLabel();

        newInstructiosns[0].labels.Add(allowLabel);

        newInstructiosns.InsertRange(0, new[]
        {
            // if (DisableTriggering(player))
            //     return false;
            new(OpCodes.Ldarg_1),
            CodeInstruction.Call(typeof(TeslaInRangePatch), nameof(DisableTriggering)),
            new(OpCodes.Brfalse, allowLabel),
            new(OpCodes.Ldc_I4_0),
            new(OpCodes.Ret)
        });

        return newInstructiosns.FinishTranspiler();
    }
}