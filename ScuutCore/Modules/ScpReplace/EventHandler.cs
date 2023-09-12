namespace ScuutCore.Modules.ScpReplace
{
    using System.Linq;
    using MEC;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.ScpReplace.Models;

    public class EventHandler : InstanceBasedEventHandler<ScpReplaceModule>
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnJoined(Player player)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                var found = ScpReplaceModule.ReplaceInfos.FirstOrDefault(x => x.UserId == player.UserId);
                if (found != null)
                {
                    found.Apply(player);
                    ScpReplaceModule.ReplaceInfos.Remove(found);
                }
            });
        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        public void OnPlayerLeave(Player player)
        {
            if (!Module.Config.AllowedRoles.Contains(player.Role))
                return;
            if (Round.Duration.TotalSeconds > Module.Config.SecondsIntoRoundActive)
                return;
            if (player.Health / player.MaxHealth > Module.Config.MinScpHealthPercent)
                return;
            ScpReplaceModule.ReplaceInfos.Add(new ReplaceInfo(player));
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            ScpReplaceModule.ReplaceInfos.Clear();
        }
    }
}