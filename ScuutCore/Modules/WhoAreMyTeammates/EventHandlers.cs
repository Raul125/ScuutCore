namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using MEC;
    using NorthwoodLib.Pools;
    using PlayerRoles;
    using PluginAPI.Core;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EventHandlers
    {
        private WhoAreMyTeammates whoAreMyTeammates;
        public EventHandlers(WhoAreMyTeammates wamt)
        {
            whoAreMyTeammates = wamt;
        }

        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.CallDelayed(whoAreMyTeammates.Config.DelayTime, () =>
            {
                foreach (WamtBroadcast broadcast in whoAreMyTeammates.Config.WamtBroadcasts)
                    RunBroadcast(broadcast);
            }));
        }

        private void RunBroadcast(WamtBroadcast broadcast)
        {
            if (!broadcast.IsEnabled)
                return;

            List<Player> players = Player.GetPlayers().Where(x => x.Role.GetTeam() == broadcast.Team).ToList();
            if (broadcast.MaxPlayers > -1 && players.Count >= broadcast.MaxPlayers)
                return;

            if (players.Count == 1)
            {
                Timing.CallDelayed(broadcast.Delay, () => DisplayBroadcast(players[0], broadcast.AloneContents, broadcast.Time, broadcast.Type));
                return;
            }

            string contentsFormatted = broadcast.Contents.Replace("%list%", GeneratePlayerList(players, broadcast));
            contentsFormatted = contentsFormatted.Replace("%count%", players.Count.ToString());
            foreach (Player player in players)
                Timing.CallDelayed(broadcast.Delay, () => DisplayBroadcast(player, contentsFormatted, broadcast.Time, broadcast.Type));
        }

        private void DisplayBroadcast(Player player, string content, ushort duration, DisplayType displayType)
        {
            switch (displayType)
            {
                case DisplayType.Broadcast:
                    player.SendBroadcast(content, duration);
                    return;
                case DisplayType.Hint:
                    player.ReceiveHint(content, duration);
                    return;
                case DisplayType.ConsoleMessage:
                    player.SendConsoleMessage(content, "cyan");
                    return;
            }
        }

        private string GeneratePlayerList(IList<Player> players, WamtBroadcast broadcast)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            if (!broadcast.Contents.Contains("%list%"))
                return string.Empty;

            int cutOff = players.Count - 1;
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];

                stringBuilder.Append(' ').Append(player.Nickname);
                if (player.IsScp)
                    stringBuilder.Append(' ').Append(player.ReferenceHub.roleManager._curRole.RoleName);

                if (i != cutOff)
                    stringBuilder.Append(", ");
            }

            stringBuilder.Append('.');
            return StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimStart();
        }
    }
}