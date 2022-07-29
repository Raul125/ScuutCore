using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.CustomEscape
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public Dictionary<RoleType, RoleType> CuffedRoleConversions { get; set; } = new Dictionary<RoleType, RoleType>
        {
            { RoleType.FacilityGuard, RoleType.ChaosRifleman },
            { RoleType.ChaosRifleman, RoleType.NtfPrivate },
            { RoleType.ChaosRepressor, RoleType.NtfPrivate },
            { RoleType.ChaosMarauder, RoleType.NtfPrivate },
            { RoleType.ChaosConscript, RoleType.NtfPrivate }
        };

        public bool Debug { get; set; } = false;
    }
}