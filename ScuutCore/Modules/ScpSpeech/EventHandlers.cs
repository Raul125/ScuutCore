namespace ScuutCore.Modules.ScpSpeech
{
    using API.Features;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    public sealed class EventHandlers : InstanceBasedEventHandler<ScpSpeechModule>
    {

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnRoleChanged(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason reason)
        {
            SpeechHelper.RemoveProximityChat(player.ReferenceHub);
        }

    }
}