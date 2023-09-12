namespace ScuutCore.Modules.Patreon
{
    using System.Collections.Generic;
    using System.Text;
    using NWAPIPermissionSystem;
    using PlayerRoles.Spectating;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using Types;
    public static class PatreonExtensions
    {

        public static bool TryGetRankFromUser(ReferenceHub hub, out PatreonRank rank)
        {
            var cfg = PatreonPerksModule.Singleton.Config;
            string id = null;
            var sender = hub.queryProcessor._sender;
            foreach (var pair in cfg.PatreonPermissionIds)
            {
                if (sender.CheckPermission(pair.Key))
                {
                    id = pair.Value;
                    break;
                }
            }

            if (id == null)
            {
                rank = default;
                return false;
            }

            foreach (var r in cfg.RankList)
            {
                if (r.Id == id)
                {
                    rank = r;
                    return true;
                }
            }

            rank = default;
            return false;
        }

        public static void SetRank(ReferenceHub hub, string content, string color)
        {
            var roles = hub.serverRoles;
            roles.Network_myText = content.Replace("\\u", "");
            if (color != null)
                roles.SetColor(color);
        }

        public static float GetRagdollVelocityMultiplier(DamageHandlerBase handler, ReferenceHub owner)
        {
            if (owner == null || handler == null || PatreonPerksModule.Singleton?.Config == null)
                return 1f;
            float value = PatreonPerksModule.Singleton.Config.RagdollFlyMultiplier;
            if (owner.TryGetComponent(out PatreonData data) && data.Prefs.FlyingRagdollSelf)
                return value;
            if (handler is not AttackerDamageHandler { Attacker: { Hub: var attacker } } || attacker == null)
                return 1f;
            if (attacker.TryGetComponent(out PatreonData attackerData) && attackerData.Prefs.FlyingRagdollKills)
                return value;
            return 1f;
        }

        public static List<uint> SpectatorListPlayers = new();
        public static bool ShouldShowSpectatorList(this Player ply) => SpectatorListPlayers.Contains(ply.NetworkId);

        public static void ShowSpectators(this Player ply)
        {
            StringBuilder sb = new();
            List<ReferenceHub> hubs = new();
            foreach (var hub in ReferenceHub.AllHubs)
            {
                if (hub == ReferenceHub.HostHub)
                    continue;
                if (!ply.ReferenceHub.IsSpectatedBy(hub))
                    continue;
                hubs.Add(hub);
            }

            if (PatreonPerksModule.Singleton == null)
                return;
            var config = PatreonPerksModule.Singleton.Config;
            if (hubs.Count == 0 && !config.ShowEmptySpectatorList)
                return;
            sb.Append(config.SpectatorListPrefix);
            sb.Append(config.SpectatorListTitle.Replace("%count%", hubs.Count.ToString()));
            sb.AppendLine(config.SpectatorListSuffix);
            foreach (var hub in hubs)
            {
                sb.Append(config.SpectatorListPrefix);
                sb.Append(config.SpectatorListElement.Replace("%name%", hub.nicknameSync.MyNick));
                sb.AppendLine(config.SpectatorListSuffix);
            }
            ply.ReceiveHint(sb.ToString(), 1.25f);
        }

        public static int GetPriorityPatreon(this Player ply) =>
            ply.GameObject.TryGetComponent(out PatreonData data) ? data.Rank.Priority : 0;
    }
}