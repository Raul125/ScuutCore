namespace ScuutCore.Patches.ScpSpeech
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Modules.ScpSpeech;
    using NorthwoodLib.Pools;
    //[HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    internal static class TransmitPatch
    {

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var isProximity = generator.DeclareLocal(typeof(bool));
            var setChannelLabel = generator.DefineLabel();

            int checkIndex = list.FindLastIndex(i => i.opcode == OpCodes.Brfalse_S);
            int setIndex = checkIndex + 2;
            list[setIndex].labels.Add(setChannelLabel);
            list.InsertRange(setIndex, new[]
            {
                new CodeInstruction(OpCodes.Ldloc, isProximity.LocalIndex),
                new CodeInstruction(OpCodes.Brfalse, setChannelLabel),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Stloc, 6)
            });

            list.InsertRange(checkIndex, new[]
            {
                new CodeInstruction(new CodeInstruction(OpCodes.Ldloc_0)),
                new CodeInstruction(new CodeInstruction(OpCodes.Ldloc, 5)),
                CodeInstruction.Call(typeof(SpeechHelper), nameof(SpeechHelper.SendCheck)),
                new CodeInstruction(OpCodes.Stloc, isProximity.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc, isProximity.LocalIndex)
            });

            foreach (var instruction in list)
                yield return instruction;

            ListPool<CodeInstruction>.Shared.Return(list);
        }
    }
}