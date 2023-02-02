namespace ScuutCore.Modules.ScpSwap
{
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public class EventHandlers
    {
        private ScpSwap scpSwap;
        public EventHandlers(ScpSwap sc)
        {
            scpSwap = sc;
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (player.Role.GetTeam() is Team.SCPs || ValidSwaps.GetCustom(player) != null)
                return;

            Plugin.Coroutines.Add(Timing.CallDelayed(0.1f, () =>
            {
                if ((player.Role.GetTeam() is Team.SCPs || ValidSwaps.GetCustom(player) != null) &&
                    Round.Duration.TotalSeconds < scpSwap.Config.SwapTimeout)
                    scpSwap.Config.StartMessage.Show(player);
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
}