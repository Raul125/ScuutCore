namespace ScuutCore.Modules.ScpSpeech
{
    using System.Collections.Generic;
    using API.Features;
    using Hints;
    using PlayerRoles;
    using PlayerRoles.PlayableScps;
    using PlayerRoles.PlayableScps.Scp079;
    using PlayerRoles.Voice;
    using PluginAPI.Core;
    using VoiceChat;
    public static class SpeechHelper
    {
        public static Config? Cfg => ScpSpeechModule.Instance?.Config;

        private static readonly HashSet<uint> ProximityChatNetIDs = new HashSet<uint>();

        public static bool CanSwitchVoiceChannels(ReferenceHub hub) => Cfg?.PermittedRoles.Contains(hub.GetRoleId()) ?? false;

        public static bool IsUsingProximityChat(ReferenceHub hub) => ProximityChatNetIDs.Contains(hub.characterClassManager.netId);

        public static bool ToggleProximityChat(ReferenceHub hub)
        {
            uint id = hub.characterClassManager.netId;
            if (ProximityChatNetIDs.Contains(id))
            {
                ProximityChatNetIDs.Remove(id);
                return false;
            }
            ProximityChatNetIDs.Add(id);
            if (Player.TryGet(hub, out ScuutPlayer p))
                p.SpeechUpdateTime = -2.5f;
            return true;
        }

        public static void RemoveProximityChat(ReferenceHub hub) => ProximityChatNetIDs.Remove(hub.characterClassManager.netId);

        public static void ProcessAltToggle(ReferenceHub hub)
        {
            if (!CanSwitchVoiceChannels(hub))
                return;
            bool result = ToggleProximityChat(hub);
            string text = result ? "\n\n\nProximity chat <mark=#00ff0055>enabled</mark>" : "\n\n\nProximity chat <mark=#ff000055>disabled</mark>";
            hub.hints.Show(new TextHint(text, new HintParameter[]
            {
                new StringHintParameter(text)
            }));
        }

        public static bool SendCheck(VoiceChatChannel channel, IVoiceRole sender, IVoiceRole receiver)
        {
            if (sender == receiver)
                return false;
            if (sender.VoiceModule is not (StandardScpVoiceModule module and not Scp079VoiceModule) || !IsUsingProximityChat(module.Owner))
                return channel != VoiceChatChannel.None;
            float distanceSqr = (receiver.VoiceModule.Owner.PlayerCameraReference.position - module.Owner.PlayerCameraReference.position).sqrMagnitude;
            float range = Cfg.ProximityChatRange;
            return distanceSqr <= range * range;
        }
    }
}