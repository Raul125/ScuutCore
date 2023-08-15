namespace ScuutCore.Patches.ScpSpeech
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using Mirror;
    using Modules.ScpSpeech;
    using NorthwoodLib.Pools;
    using PlayerRoles.Voice;
    using PluginAPI.Core;
    using VoiceChat;
    using VoiceChat.Networking;

    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    internal static class TransmitPatch
    {
        public static bool Prefix(NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.SpeakerNull || (int) msg.Speaker.netId != (int) conn.identity.netId || !(msg.Speaker.roleManager.CurrentRole is IVoiceRole currentRole1) || !currentRole1.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker))
                return false;
            VoiceChatChannel channel = currentRole1.VoiceModule.ValidateSend(msg.Channel);
            if (channel == VoiceChatChannel.None)
                return false;
            currentRole1.VoiceModule.CurrentChannel = channel;
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.roleManager.CurrentRole is IVoiceRole currentRole2)
                {
                    VoiceChatChannel voiceChatChannel = currentRole2.VoiceModule.ValidateReceive(msg.Speaker, channel);
                    if (SpeechHelper.SendCheck(voiceChatChannel, currentRole1, currentRole2))
                    {
                        if (!Round.IsRoundStarted)
                            voiceChatChannel = VoiceChatChannel.RoundSummary;
                        msg.Channel = voiceChatChannel;
                        allHub.connectionToClient.Send(msg);
                    }
                }
            }

            return false;
        }

        /*private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
            if (ScpSpeechModule.Instance is { Config: { IsEnabled: true } })
            {
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
            }

            foreach (var instruction in list)
                yield return instruction;

            ListPool<CodeInstruction>.Shared.Return(list);
        }*/
    }
}