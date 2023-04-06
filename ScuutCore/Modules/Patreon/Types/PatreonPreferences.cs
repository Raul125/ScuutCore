namespace ScuutCore.Modules.Patreon.Types
{
    using ScuutCore.Modules.Pets;
    using System;

    [Serializable]
    public class PatreonPreferences
    {

        public string CustomBadge { get; set; }

        public string CustomBadgeColor { get; set; }

        public int BadgeIndex { get; set; } = Badge.Cycle;

        public bool FlyingRagdollSelf { get; set; }

        public bool FlyingRagdollKills { get; set; }

        public PetData PetData { get; set; } = new PetData();

    }
}