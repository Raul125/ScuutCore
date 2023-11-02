namespace ScuutCore.Modules.ScpReplace;

using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using ScuutCore.API.Features;
using ScuutCore.Modules.ScpReplace.Models;

public class EventHandler : InstanceBasedEventHandler<ScpReplaceModule>
{
    [PluginEvent(ServerEventType.PlayerChangeRole)]
    public void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
    {
        if (changeReason != RoleChangeReason.Destroyed)
            return;
        if (!Module.Config.AllowedRoles.Contains(player.Role))
            return;

        Module.Debug($"Player {player.Nickname} left the server, role {player.Role} is allowed");
        if (Round.Duration.TotalSeconds > Module.Config.SecondsIntoRoundActive)
            return;

        Module.Debug("Passed time check");
        ScpReplaceModule.ReplaceInfos.Add(new ReplaceInfo(player));
        Module.Debug($"Added {player.Nickname} to replace list");
    }

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    public void OnWaitingForPlayers()
    {
        ScpReplaceModule.ReplaceInfos.Clear();
        Module.Debug("Cleared replace list");
    }
}