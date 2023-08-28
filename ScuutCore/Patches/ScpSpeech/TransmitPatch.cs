namespace ScuutCore.Patches.ScpSpeech
{
    using System;
    using Modules.ScpSpeech;
    using HarmonyLib;
    using PlayerRoles.Spectating;
    using PlayerRoles.Voice;
    using PlayerRoles;
    using VoiceChat;
    using VoiceChat.Networking;
    using PluginAPI.Core;
    using UnityEngine;

    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public class VoicePatch
    {
        public static bool Prefix(Mirror.NetworkConnection conn, VoiceMessage msg)
        {
            if (msg.SpeakerNull || msg.Speaker.netId != conn.identity.netId)
                return false;

            if (msg.Speaker.roleManager.CurrentRole is not IVoiceRole voiceRole)
                return false;

            if (!voiceRole.VoiceModule.CheckRateLimit())
                return false;

            VcMuteFlags flags = VoiceChatMutes.GetFlags(msg.Speaker);
            if (flags == VcMuteFlags.GlobalRegular || flags == VcMuteFlags.LocalRegular)
                return false;

            VoiceChatChannel voiceChatChannel = voiceRole.VoiceModule.ValidateSend(msg.Channel);
            if (voiceChatChannel == VoiceChatChannel.None)
                return false;

            voiceRole.VoiceModule.CurrentChannel = voiceChatChannel;
            RoleTypeId role = msg.Speaker.roleManager.CurrentRole.RoleTypeId;

            try
            {
                // Lobby && finished round chat
                if (RoundSummary._singletonSet && Round.IsRoundEnded || (!(Round.IsRoundEnded || Round.IsRoundStarted)))
                {
                    foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
                    {
                        if (referenceHub == msg.Speaker)
                            continue;

                        msg.Channel = VoiceChatChannel.RoundSummary;
                        referenceHub.connectionToClient.Send(msg, 0);
                    }

                    return false;
                }

                // Scp chat
                if (voiceChatChannel is VoiceChatChannel.ScpChat)
                {
                    if ((ScpSpeechModule.Instance?.Config.PermittedRoles.Contains(role) ?? false) &&
                        EventHandlers.ScpsToggled.Contains(msg.Speaker))
                    {
                        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
                        {
                            if (referenceHub == msg.Speaker || referenceHub.roleManager.CurrentRole.Team == Team.SCPs)
                                continue;

                            bool allowSpect = referenceHub.roleManager.CurrentRole.Team == Team.Dead &&
                                              msg.Speaker.IsSpectatedBy(referenceHub) &&
                                              ScpSpeechModule.Instance.Config.SpectatorCanHear;

                            if (!allowSpect &&
                                Vector3.Distance(msg.Speaker.transform.position, referenceHub.transform.position) >
                                ScpSpeechModule.Instance.Config.ProximityChatRange)
                                continue;

                            msg.Channel = VoiceChatChannel.Proximity;
                            referenceHub.connectionToClient.Send(msg);
                        }

                        return false;
                    }
                }

                // Normal chat
                foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
                {
                    if (referenceHub.roleManager.CurrentRole is IVoiceRole voiceRole1)
                    {
                        VoiceChatChannel voiceChatChannel1 =
                            voiceRole1.VoiceModule.ValidateReceive(msg.Speaker, voiceChatChannel);

                        if (voiceChatChannel1 != VoiceChatChannel.None)
                        {
                            msg.Channel = voiceChatChannel1;
                            referenceHub.connectionToClient.Send(msg, 0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return true;
            }

            return false;
        }
    }
}