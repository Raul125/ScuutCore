namespace ScuutCore.Modules.Patreon
{
    using NWAPIPermissionSystem;
    using Types;
    public static class PatreonExtensions
    {

        public static bool TryGetRankFromUser(ReferenceHub hub, out PatreonRank rank)
        {
            var cfg = PatreonPerksModule.Singleton.Config;
            string id = null;
            foreach (var pair in cfg.PatreonPermissionIds)
            {
                if (hub.queryProcessor._sender.CheckPermission(pair.Key))
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
            roles.Network_myText = content;
            roles.Network_myColor = color;
        }

    }
}