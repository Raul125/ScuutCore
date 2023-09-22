namespace ScuutCore.Modules.ScpReplace
{
    using System.Collections.Generic;
    using PlayerRoles;
    using ScuutCore.API.Interfaces;

    public class ScpReplaceConfig : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int SecondsIntoRoundActive { get; set; } = 120;
        public int SecondsClaimPeriod { get; set; } = 15;

        public List<RoleTypeId> AllowedRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
        };
    }
}