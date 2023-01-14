using PluginAPI.Core;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PlayerStatsSystem;
using static RoundSummary;

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

        [PluginEvent(ServerEventType.WaitingForPlayers)]
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

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDying(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker != null && player != null && player.Role.GetTeam() is Team.SCPs && firstScpKiller == string.Empty)
            {
                firstScpKiller = attacker.Nickname;
                killerRole = attacker.Role.ToString();
                killedScp = player.Role.ToString();
            }

            if (damageHandler is UniversalDamageHandler universal)
            {
                if (universal.TranslationId == DeathTranslations.PocketDecay.Id)
                {
                    foreach (var ply in Player.GetPlayers().Where(x => x.Role is RoleTypeId.Scp106))
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
            }
            

            if (attacker == null)
                return;

            if (!playerKills.ContainsKey(attacker))
            {
                playerKills.Add(attacker, 1);
            }
            else
            {
                playerKills[attacker]++;
            }
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public void OnPlayerEscaping(Player player, RoleTypeId newRole)
        {
            if (firstEscaped == string.Empty)
            {
                escapedRole = player.Role.ToString();
                firstEscaped = player.Nickname;
                escapeTime = (uint)Round.Duration.TotalSeconds;
            }
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(LeadingTeam leadingTeam)
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
                message += roundSummary.Config.EscapeeMessage.Replace("{player}", firstEscaped).Replace("{time}", $"{escapeTime / 60} : {escapeTime % 60}").Replace("{role}", escapedRole).Replace("{roleColor}", ColorsDict[eRole]) + "\n";
            }
            else
                message += roundSummary.Config.NoEscapeeMessage + "\n";

            if (roundSummary.Config.ShowScpFirstKill && firstScpKiller != string.Empty)
            {
                Enum.TryParse<RoleTypeId>(killerRole, out RoleTypeId kRole);
                message += roundSummary.Config.ScpFirstKillMessage.Replace("{player}", firstScpKiller).Replace("{killerRole}", killerRole).Replace("{killedScp}", killedScp).Replace("{killerColor}", ColorsDict[kRole]) + "\n";
            }
            else
                message += roundSummary.Config.NoScpKillMessage + "\n";

            for (int i = 0; i < 30; i++)
            {
                message += "\n";
            }

            foreach (var ply in Player.GetPlayers())
                ply.ReceiveHint(message, 30);

            Timing.CallDelayed(0.25f, () => PreventHints = true);
        }

        public static Dictionary<RoleTypeId, string> ColorsDict = new Dictionary<RoleTypeId, string>
        {
            {RoleTypeId.Scp173, "#FF0000FF"},
            {RoleTypeId.ClassD, "#FF8000FF"},
            {RoleTypeId.Spectator, "#FFFFFFFF"},
            {RoleTypeId.Scp106, "#FF0000FF"},
            {RoleTypeId.NtfSpecialist, "#0096FFFF"},
            {RoleTypeId.Scp049, "#FF0000FF"},
            {RoleTypeId.Scientist, "#FFFF7CFF"},
            {RoleTypeId.Scp079, "#FF0000FF"},
            {RoleTypeId.ChaosConscript, "#008F1CFF"},
            {RoleTypeId.Scp096, "#FF0000FF"},
            {RoleTypeId.Scp0492, "#FF0000FF"},
            {RoleTypeId.NtfSergeant, "#0096FFFF"},
            {RoleTypeId.NtfCaptain, "#003DCAFF"},
            {RoleTypeId.NtfPrivate, "#70C3FFFF"},
            {RoleTypeId.Tutorial, "#FF00B0FF"},
            {RoleTypeId.FacilityGuard, "#556278FF"},
            {RoleTypeId.Scp939, "#FF0000FF"},
            {RoleTypeId.CustomRole, "#FFFFFFFF"},
            {RoleTypeId.ChaosRifleman, "#008F1CFF"},
            {RoleTypeId.ChaosRepressor, "#008F1CFF"},
            {RoleTypeId.ChaosMarauder, "#008F1CFF"},
            {RoleTypeId.Overwatch, "#00FFFFFF"},
            {RoleTypeId.None, "#FFFFFFFF"}
        };
    }
}