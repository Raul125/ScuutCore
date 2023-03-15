namespace ScuutCore.Modules.Patreon
{
    using API.Features;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    public sealed class EventHandlers : IEventHandler
    {

        [PluginEvent(ServerEventType.PlayerJoined)]
        private void OnPlayerJoined(Player player)
        {
            if (PatreonExtensions.TryGetRankFromUser(player.UserId, out var rank))
                PatreonData.Get(player.ReferenceHub).Rank = rank;
        }

    }
}