namespace ScuutCore.Modules.Patreon.Types
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;

    [Serializable]
    public struct PatreonRank
    {
        public string Id { get; set; }

        public List<PatreonPerk> Perks { get; set; }

        public List<Badge> BadgeOptions { get; set; }

        [YamlIgnore]
        public string CustomBadge { get; set; }

        [YamlIgnore]
        public string CustomColor { get; set; }

        [YamlIgnore]
        public int ChosenBadge { get; set; }

    }
}