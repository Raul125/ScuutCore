namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using System.Collections.Generic;
    using System.Linq;
    using API.Features;
    using Hints;
    using MEC;
    using Models;
    using NorthwoodLib.Pools;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using RemoteKeycard;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<WhoAreMyTeammates>
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.CallDelayed(Module.Config.DelayTime, () =>
            {
                foreach (var broadcast in Module.Config.WamtBroadcasts)
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

            if (players.Count is 1)
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(broadcast.Delay, () => DisplayBroadcast(players[0], broadcast.AloneContents, broadcast.Time, broadcast.Type)));
                return;
            }

            string contentsFormatted = broadcast.Contents.Replace("%list%", GeneratePlayerList(players, broadcast));
            contentsFormatted = contentsFormatted.Replace("%count%", players.Count.ToString());
            foreach (Player player in players)
                Plugin.Coroutines.Add(Timing.CallDelayed(broadcast.Delay, () => DisplayBroadcast(player, contentsFormatted, broadcast.Time, broadcast.Type)));
        }

        private void DisplayBroadcast(Player player, string content, ushort duration, DisplayType displayType)
        {
            switch (displayType)
            {
                case DisplayType.Broadcast:
                    player.SendBroadcast(content, duration);
                    return;
                case DisplayType.Hint:
                    var curve = HintEffectPresets.CreateTrailingBumpCurve(0.5f, 1, count: 3, 0.5f, duration);
                    curve.postWrapMode = WrapMode.Clamp;
                    player.ReceiveHint(new string('\n', Module.Config.HintLowering) + content, new HintEffect[]
                    {
                        HintEffectPresets.FadeIn(0.05f),
                        HintEffectPresets.FadeOut(0.05f, 0.95f)
                    }, duration);
                    return;
                case DisplayType.ConsoleMessage:
                    player.SendConsoleMessage(content, "cyan");
                    return;
            }
        }

        private static string GeneratePlayerList(IList<Player> players, WamtBroadcast broadcast)
        {
            var stringBuilder = StringBuilderPool.Shared.Rent();
            if (!broadcast.Contents.Contains("%list%"))
                return string.Empty;

            int cutOff = players.Count - 1;
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];

                stringBuilder.Append(' ').Append(player.Nickname);
                if (player.IsSCP())
                    stringBuilder.Append(' ').Append(player.ReferenceHub.roleManager._curRole.RoleName);

                if (i != cutOff)
                    stringBuilder.Append(", ");
            }

            stringBuilder.Append('.');
            return StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimStart();
        }
    }
}