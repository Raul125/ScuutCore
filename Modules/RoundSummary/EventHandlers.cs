using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using System.Linq;

namespace ScuutCore.Modules.RoundSummary
{
    public class EventHandlers
    {
        private RoundSummary roundSummary;
        public EventHandlers(RoundSummary rs)
        {
            roundSummary = rs;
        }

        private Dictionary<Player, int> playerKills = new Dictionary<Player, int>();
        private string firstEscaped = string.Empty;
        private uint escapeTime = 0;
        private string firstScpKiller = string.Empty;

        public void OnWaitingForPlayers()
        {
            playerKills.Clear();
            firstEscaped = string.Empty;
            firstScpKiller = string.Empty;
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Handler.Type == Exiled.API.Enums.DamageType.PocketDimension || ev.Handler.Type == Exiled.API.Enums.DamageType.Scp106)
            {
                foreach (var ply in Player.Get(RoleType.Scp106))
                    playerKills[ply]++;
            }

            if (ev.Killer == null)
                return;

            playerKills[ev.Killer]++;

            if (ev.Target != null && ev.Target.IsScp && firstScpKiller == string.Empty)
                firstScpKiller = ev.Killer.Nickname;
        }

        public void OnPlayerEscaping(EscapingEventArgs ev)
        {
            if (firstEscaped == string.Empty)
            {
                firstEscaped = ev.Player.Nickname;
                escapeTime = (uint)Round.ElapsedTime.TotalSeconds;
            }
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            string message = string.Empty;

            if (roundSummary.Config.ShowKills && playerKills.Count > 0)
            {
                var bestPlayer = playerKills.OrderByDescending(x => x.Value).ElementAt(0);
                message += roundSummary.Config.KillsMessage.Replace("{player}", bestPlayer.Key.Nickname).Replace("{kills}", bestPlayer.Value.ToString()) + "\n";
            }
            else
                message += roundSummary.Config.NoKillsMessage + "\n";

            if (roundSummary.Config.ShowEscapee && firstEscaped != string.Empty)
                message += roundSummary.Config.EscapeeMessage.Replace("{player}", firstEscaped).Replace("{time}", $"{escapeTime / 60} : {escapeTime % 60}") + "\n";
            else
                message += roundSummary.Config.NoEscapeeMessage + "\n";

            if (roundSummary.Config.ShowScpFirstKill && firstScpKiller != string.Empty)
                message += roundSummary.Config.ScpFirstKillMessage.Replace("{player}", firstScpKiller) + "\n";
            else
                message += roundSummary.Config.NoScpKillMessage + "\n";

            for (int i = 0; i < 30; i++)
            {
                message += "\n";
            }

            Map.ShowHint(message, ev.TimeToRestart);
        }
    }
}