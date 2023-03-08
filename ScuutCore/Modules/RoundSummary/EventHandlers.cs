namespace ScuutCore.Modules.RoundSummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ScuutCore.API.Features;
    using MEC;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public sealed class EventHandlers : InstanceBasedEventHandler<RoundSummary>
    {
        private readonly Dictionary<Player, int> playerKills = new();
        private string firstEscaped = string.Empty;
        private uint escapeTime;
        private string firstScpKiller = string.Empty;
        private string escapedRole = string.Empty;
        private string killedScp = string.Empty;
        private string killerRole = string.Empty;

        public static bool PreventHints;

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

        [PluginEvent(ServerEventType.PlayerDying)]
        public void OnDying(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker != null && attacker is { IsSCP: false } && player is { IsSCP: true } && firstScpKiller == string.Empty)
            {
                firstScpKiller = attacker.Nickname;
                killerRole = attacker.Role.ToString();
                killedScp = player.Role.ToString();
            }

            if (damageHandler is UniversalDamageHandler universal
                && universal.TranslationId == DeathTranslations.PocketDecay.Id)
            {
                foreach (var ply in Player.GetPlayers().Where(x => x.Role is RoleTypeId.Scp106))
                {
                    if (!playerKills.ContainsKey(ply))
                        playerKills.Add(ply, 1);
                    else
                        playerKills[ply]++;
                }
            }

            if (attacker is null)
                return;

            if (!playerKills.ContainsKey(attacker))
                playerKills.Add(attacker, 1);
            else
                playerKills[attacker]++;
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public void OnPlayerEscaping(Player player, RoleTypeId newRole)
        {
            if (firstEscaped != string.Empty)
                return;
            escapedRole = player.Role.ToString();
            firstEscaped = player.Nickname;
            escapeTime = (uint)Round.Duration.TotalSeconds;
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(global::RoundSummary.LeadingTeam leadingTeam)
        {
            Timing.CallDelayed(Module.Config.DisplayDelay, () =>
            {
                string message = string.Empty;

                if (Module.Config.ShowKills && playerKills.Count > 0)
                {
                    var bestPlayer = playerKills.OrderByDescending(x => x.Value).ElementAt(0);
                    message += Module.Config.KillsMessage.Replace("{player}", bestPlayer.Key.Nickname)
                        .Replace("{kills}", bestPlayer.Value.ToString()) + "\n";
                }
                else
                    message += Module.Config.NoKillsMessage + "\n";

                if (Module.Config.ShowEscapee && firstEscaped != string.Empty)
                {
                    Enum.TryParse(escapedRole, out RoleTypeId eRole);
                    message += Module.Config.EscapeeMessage.Replace("{player}", firstEscaped)
                        .Replace("{time}", $"{escapeTime / 60} : {escapeTime % 60}").Replace("{role}", escapedRole)
                        .Replace("{roleColor}", GetColor(eRole)) + "\n";
                }
                else
                    message += Module.Config.NoEscapeeMessage + "\n";

                if (Module.Config.ShowScpFirstKill && firstScpKiller != string.Empty)
                {
                    Enum.TryParse(killerRole, out RoleTypeId kRole);
                    message += Module.Config.ScpFirstKillMessage.Replace("{player}", firstScpKiller)
                        .Replace("{killerRole}", killerRole).Replace("{killedScp}", killedScp)
                        .Replace("{killerColor}", GetColor(kRole)) + "\n";
                }
                else
                    message += Module.Config.NoScpKillMessage + "\n";

                for (int i = 0; i < 30; i++)
                    message += "\n";

                foreach (var ply in Player.GetPlayers())
                    ply.ReceiveHint(message, 30);

                Plugin.Coroutines.Add(Timing.CallDelayed(0.25f, () => PreventHints = true));
            });
        }
        private static string GetColor(RoleTypeId role) => PlayerRoleLoader.TryGetRoleTemplate(role, out PlayerRoleBase roleBase) ? roleBase.RoleColor.ToHex() : "#FFF";
    }
}