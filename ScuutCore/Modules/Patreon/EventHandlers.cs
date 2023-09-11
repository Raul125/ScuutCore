namespace ScuutCore.Modules.Patreon
{
    using API.Features;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using RoundSummary = global::RoundSummary;
    public sealed class EventHandlers : IEventHandler
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        private void OnPlayerJoined(Player player)
        {
            if (!PatreonExtensions.TryGetRankFromUser(player.ReferenceHub, out var rank))
                return;
            MEC.Timing.CallDelayed(1, () => PatreonData.Get(player.ReferenceHub).Rank = rank);
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        private void OnRoundEnd(RoundSummary.LeadingTeam team)
        {
            PatreonData.WriteAll();
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        private void OnWaitingForPlayers()
        {
            PatreonData.ReadAll();
            PatreonExtensions.SpectatorListPlayers.Clear();
        }
    }
}