namespace ScuutCore.Modules.Patreon.Types
{
    using System;

    [Serializable]
    public class PatreonPreferences
    {

        public string CustomBadge { get; set; }

        public string CustomBadgeColor { get; set; }

        public int BadgeIndex { get; set; } = Badge.Cycle;

    }
}