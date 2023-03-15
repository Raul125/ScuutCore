namespace ScuutCore.Modules.Patreon
{
    using Types;
    using Utils.NonAllocLINQ;
    public static class PatreonExtensions
    {

        public static bool TryGetRank(string id, out PatreonRank rank)
        {
            rank = PatreonPerksModule.Singleton.Config.RankList.FirstOrDefault(e => e.Id == id, default);
            return rank.IsValid;
        }

        public static bool TryGetRankFromUser(string userId, out PatreonRank rank)
        {
            var association = PatreonPerksModule.Singleton.Config.UserRanks.FirstOrDefault(e => e.UserId == userId, default);
            if (association.RankId != null)
                return TryGetRank(association.RankId, out rank);
            rank = default;
            return false;

        }

        public static bool TryGetRankFromUser(ReferenceHub hub, out PatreonRank rank)
        {
            if (!hub.TryGetComponent(out PatreonData data))
            {
                rank = default;
                return false;
            }

            rank = data.Rank;
            return rank.IsValid;
        }

        public static bool IsSupporter(ReferenceHub hub) => TryGetRankFromUser(hub, out _);

        public static void SetRank(ReferenceHub hub, string content, string color)
        {
            var roles = hub.serverRoles;
            roles.Network_myText = content;
            roles.Network_myColor = color;
        }

    }
}