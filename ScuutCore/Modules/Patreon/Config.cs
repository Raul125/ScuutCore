namespace ScuutCore.Modules.Patreon
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;
    using Commands;
    using Types;
    public sealed class Config : IModuleConfig
    {

        public bool IsEnabled { get; set; } = true;

        [Description("Amount of time in seconds before swapping to another Patreon badge")]
        public float AutoSwitchBadge { get; set; } = 10f;

        public List<string> BlacklistedBadgePhrases { get; set; } = new List<string>
        {
            "admin",
            "moderator"
        };

        public List<PatreonRank> RankList { get; set; } = new List<PatreonRank>
        {
            new PatreonRank
            {
                Id = "ExampleRank",
                BadgeOptions = new List<Badge>
                {
                    new Badge("Example Badge", "lime"),
                    new Badge("Other Badge", "red", new[]
                    {
                        "pink",
                        "silver"
                    })
                },
                DefaultCustomColor = "tomato"
            }
        };

        [Description("Permissions that specify the exact Patreon rank to use")]
        public Dictionary<string, string> PatreonPermissionIds { get; set; } = new Dictionary<string, string>
        {
            {
                "example.permission", "ExampleRank"
            }
        };

        [Description("The multiplier for ragdoll velocity when flying ragdoll is enabled")]
        public float RagdollFlyMultiplier { get; set; } = 3f;

        [Description("Whether to add the suffix:" + SetCustomBadge.CustomPatreon)]
        public bool CustomPatreonSuffix { get; set; } = true;

        [Description("NEEDS RESPAWNTIMER")]
        public string SpectatorListTitle { get; set; } = "<b>👥 Spectators (%count%):</b>";
        public string SpectatorListElement { get; set; } = "%name%";
        public string SpectatorListPrefix { get; set; } = "<voffset=1em>";
        public string SpectatorListSuffix { get; set; } = '\n'.ToString();
        public bool ShowEmptySpectatorList { get; set; } = false;
    }
}