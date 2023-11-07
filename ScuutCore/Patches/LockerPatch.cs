namespace ScuutCore.Patches;

using HarmonyLib;
using MapGeneration.Distributors;
using ScuutCore.API.Extensions;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using static HarmonyLib.AccessTools;

[HarmonyPatch(typeof(Locker), nameof(Locker.Start))]
public static class LockerPatch
{
    private static bool _alreadySpawned1576;

    static LockerPatch()
    {
        CharacterClassManager.OnRoundStarted += ResetChance;
    }

    public static void ResetChance()
    {
        _alreadySpawned1576 = Random.Range(0, 100) > Plugin.Singleton.Config.Scp1576SpawnChance;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = instructions.BeginTranspiler();

        // LockerPatch.OnLockerStart(this);
        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new(OpCodes.Ldarg_0),
            new(OpCodes.Call, Method(typeof(LockerPatch), nameof(OnLockerStart))),
        });

        return newInstructions.FinishTranspiler();
    }

    private static void OnLockerStart(Locker locker)
    {
        if (_alreadySpawned1576 || locker.StructureType != StructureType.ScpPedestal)
            return;

        foreach (var loot in locker.Loot)
        {
            if (loot.TargetItem != ItemType.SCP500)
                continue;

            loot.TargetItem = ItemType.SCP1576;
            _alreadySpawned1576 = true;
            return;
        }
    }
}
