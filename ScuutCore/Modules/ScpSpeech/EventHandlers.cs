namespace ScuutCore.Modules.ScpSpeech;

using API.Features;
using PlayerRoles;
using PlayerRoles.PlayableScps;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System.Collections.Generic;

public sealed class EventHandlers : InstanceBasedEventHandler<ScpSpeechModule>
{
    public static List<ReferenceHub> ScpsToggled = new();

    [PluginEvent(ServerEventType.PlayerChangeRole)]
    public void OnRoleChanged(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason reason)
    {
        if (oldRole is FpcStandardScp)
            ScpsToggled.Remove(player.ReferenceHub);
    }

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    public void OnWaitingForPlayers()
    {
        ScpsToggled.Clear();
    }
}