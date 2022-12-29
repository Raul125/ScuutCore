﻿using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
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
        private string escapedRole = string.Empty;
        private string killedScp = string.Empty;
        private string killerRole = string.Empty;

        public static bool PreventHints = false;

        public void OnWaitingForPlayers()
        {
            playerKills.Clear();
            firstEscaped = string.Empty;
            firstScpKiller = string.Empty;
            escapedRole = string.Empty;
            killedScp = string.Empty;
            killerRole = string.Empty;
            PreventHints = false;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            if (ev.Attacker != null && ev.Player != null && ev.Player.IsScp && firstScpKiller == string.Empty)
            {
                firstScpKiller = ev.Attacker.Nickname;
                killerRole = ev.Attacker.Role.Type.ToString();
                killedScp = ev.Player.Role.Type.ToString();
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.PocketDimension || ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Scp106)
            {
                foreach (var ply in Player.Get(RoleTypeId.Scp106))
                {
                    if (!playerKills.ContainsKey(ply))
                    {
                        playerKills.Add(ply, 1);
                    }
                    else
                    {
                        playerKills[ply]++;
                    }
                }
            }

            if (ev.Attacker == null)
                return;

            if (!playerKills.ContainsKey(ev.Attacker))
            {
                playerKills.Add(ev.Attacker, 1);
            }
            else
            {
                playerKills[ev.Attacker]++;
            }
        }

        public void OnPlayerEscaping(EscapingEventArgs ev)
        {
            if (firstEscaped == string.Empty)
            {
                escapedRole = ev.Player.Role.Type.ToString();
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
            {
                Enum.TryParse<RoleTypeId>(escapedRole, out RoleTypeId eRole);
                message += roundSummary.Config.EscapeeMessage.Replace("{player}", firstEscaped).Replace("{time}", $"{escapeTime / 60} : {escapeTime % 60}").Replace("{role}", escapedRole).Replace("{roleColor}", eRole.GetColor().ToHex()) + "\n";
            }
            else
                message += roundSummary.Config.NoEscapeeMessage + "\n";

            if (roundSummary.Config.ShowScpFirstKill && firstScpKiller != string.Empty)
            {
                Enum.TryParse<RoleTypeId>(killerRole, out RoleTypeId kRole);
                message += roundSummary.Config.ScpFirstKillMessage.Replace("{player}", firstScpKiller).Replace("{killerRole}", killerRole).Replace("{killedScp}", killedScp).Replace("{killerColor}", kRole.GetColor().ToHex()) + "\n";
            }
            else
                message += roundSummary.Config.NoScpKillMessage + "\n";

            for (int i = 0; i < 30; i++)
            {
                message += "\n";
            }

            Map.ShowHint(message, ev.TimeToRestart);
            Timing.CallDelayed(0.25f, () => PreventHints = true);
        }
    }
}