namespace ScuutCore.Patches.ScpSpeech
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Modules.ScpSpeech;
    using NorthwoodLib.Pools;
    using PlayerRoles.FirstPersonControl;
    using PlayerRoles.FirstPersonControl.NetworkMessages;
    [HarmonyPatch(typeof(FpcNoclipToggleMessage), nameof(FpcNoclipToggleMessage.ProcessMessage))]
    internal static class ToggleNoclipPatch
    {

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int permittedCheck = list.FindIndex(i => i.operand is MethodInfo { Name: nameof(FpcNoclip.IsPermitted) });
            if (ScpSpeechModule.Instance.Config.IsEnabled)
                list.InsertRange(list.FindIndex(permittedCheck, i => i.opcode == OpCodes.Ret), new[]
                {
                    new CodeInstruction(OpCodes.Ldloc_0),
                    CodeInstruction.Call(typeof(SpeechHelper), nameof(SpeechHelper.ProcessAltToggle))
                });
            foreach (var codeInstruction in list)
                yield return codeInstruction;
            ListPool<CodeInstruction>.Shared.Return(list);
        }

    }

}