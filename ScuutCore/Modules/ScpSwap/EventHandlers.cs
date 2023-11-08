namespace ScuutCore.Modules.ScpSwap;

using ScuutCore.API.Features;
using MEC;
using Models;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

public sealed class EventHandlers : InstanceBasedEventHandler<ScpSwap>
{
    [PluginEvent(ServerEventType.PlayerChangeRole)]
    public void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
    {
        var customSwap = ValidSwaps.GetCustom(player);
        if (player.Role.GetTeam() is Team.SCPs || customSwap != null)
            return;

        Plugin.Coroutines.Add(Timing.CallDelayed(0.1f, () =>
        {
            #warning scp3114 - remove after halloween
            if ((player.Role.GetTeam() is Team.SCPs || player.Role == RoleTypeId.Scp3114 || customSwap != null) &&
                Round.Duration.TotalSeconds < Module.Config.SwapTimeout)
                Module.Config.StartMessage.Show(player);
        }));
    }

    [PluginEvent(ServerEventType.RoundRestart)]
    public void OnRestartingRound()
    {
        Swap.Clear();
    }

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    public void OnWaitingForPlayers()
    {
        ValidSwaps.Refresh();
    }
}