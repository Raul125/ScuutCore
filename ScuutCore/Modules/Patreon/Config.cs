namespace ScuutCore.Modules.Patreon
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;
    using Types;
    public sealed class Config : IModuleConfig
    {

        public bool IsEnabled { get; set; } = true;

        [Description("Amount of time in seconds before swapping to another Patreon badge")]
        public float AutoSwitchBadge { get; set; } = 10f;

        public List<string> BlacklistedBadgePhrases { get; set; } = new List<string>();

        public List<PatreonRank> RankList { get; set; } = new List<PatreonRank>
        {
            new PatreonRank
            {
                Id = "ExampleRank",
                Perks = new List<PatreonPerk>
                {
                    PatreonPerk.CustomBadge,
                    PatreonPerk.CustomColor,
                    PatreonPerk.FlyingRagdollOthers,
                    PatreonPerk.FlyingRagdollSelf
                },
                BadgeOptions = new List<Badge>
                {
                    new Badge("Example Badge", "lime"),
                    new Badge("Other Badge", "red")
                }
            }
        };

        public List<UserRankAssociation> UserRanks = new List<UserRankAssociation>
        {
            new UserRankAssociation
            {
                UserId = "ExampleUser@steam",
                RankId = "ExampleRank"
            }
        };
    }
}