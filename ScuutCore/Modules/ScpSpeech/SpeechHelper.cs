namespace ScuutCore.Modules.ScpSpeech
{
    using System.Collections.Generic;
    using Hints;
    using PlayerRoles;
    public static class SpeechHelper
    {
        private static Config Cfg => ScpSpeechModule.Instance.Config;

        private static readonly HashSet<uint> ProximityChatNetIDs = new HashSet<uint>();

        public static bool CanSwitchVoiceChannels(ReferenceHub hub) => Cfg.PermittedRoles.Contains(hub.GetRoleId());

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
            return true;
        }

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

    }
}