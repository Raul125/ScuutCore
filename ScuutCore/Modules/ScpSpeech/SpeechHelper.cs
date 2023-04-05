namespace ScuutCore.Modules.ScpSpeech
{
    using System.Collections.Generic;
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

    }
}