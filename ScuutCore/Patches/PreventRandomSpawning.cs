namespace ScuutCore.Patches;

using HarmonyLib;
using PlayerRoles.RoleAssign;
using ScuutCore.API.Extensions;
using ScuutCore.Modules.ChooseRole;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

[HarmonyPatch(typeof(RoleAssigner), nameof(RoleAssigner.OnRoundStarted))] 
public static class PreventRandomSpawning 
{ 
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.BeginTranspiler();

        Label allowLabel = generator.DefineLabel();

        newInstructions[0].labels.Add(allowLabel);

        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            // if (ChooseRole.Singleton is null)
            //     goto allow;
            new(OpCodes.Call, PropertyGetter(typeof(ChooseRole), nameof(ChooseRole.Singleton))),
            new(OpCodes.Brfalse_S, allowLabel),

            // if (ChooseRole.Singleton.Config.IsEnabled)
            //     return;
            new(OpCodes.Call, PropertyGetter(typeof(ChooseRole), nameof(ChooseRole.Singleton))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(ChooseRole), nameof(ChooseRole.Config))),
            new(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.IsEnabled))),
            new(OpCodes.Brfalse_S, allowLabel),
            new(OpCodes.Ret),
        });

        return newInstructions.FinishTranspiler();
    }
}