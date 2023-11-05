namespace ScuutCore.Modules.ScpReplace;

using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using ScuutCore.API.Features;
using ScuutCore.Modules.ScpReplace.Models;

public class EventHandler : InstanceBasedEventHandler<ScpReplaceModule>
{
    [PluginEvent(ServerEventType.PlayerLeft)]
    public void OnPlayerLeave(Player player)
    {
        if (Module.Config.ExtraDebug)
            Log.Debug($"Left role: {player.Role}");
        if (!Module.Config.AllowedRoles.Contains(player.Role))
        {
            if (Module.Config.ExtraDebug)
                Log.Debug($"Role {player.Role} is not allowed");
            return;
        }

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