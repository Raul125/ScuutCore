﻿namespace ScuutCore.Modules.Patreon
{
    using NWAPIPermissionSystem;
    using PlayerStatsSystem;
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

        public static sbyte Multiply(sbyte value, float multiplier) => (sbyte)(value * multiplier);

    }
}