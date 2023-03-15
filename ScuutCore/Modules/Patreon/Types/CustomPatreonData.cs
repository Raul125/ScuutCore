namespace ScuutCore.Modules.Patreon.Types
{
    using System;
    [Serializable]
    public sealed class CustomPatreonData
    {

        public string CustomBadge { get; set; }

        public string CustomBadgeColor { get; set; }

        public int BadgeIndex { get; set; }

    }
}