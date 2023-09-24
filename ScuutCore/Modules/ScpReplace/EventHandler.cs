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
        if (!Module.Config.AllowedRoles.Contains(player.Role))
            return;
        if (Round.Duration.TotalSeconds > Module.Config.SecondsIntoRoundActive)
            return;
        ScpReplaceModule.ReplaceInfos.Add(new ReplaceInfo(player));
    }

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    public void OnWaitingForPlayers()
    {
        ScpReplaceModule.ReplaceInfos.Clear();
    }
}