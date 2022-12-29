using PlayerRoles;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.CustomEscape
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public Dictionary<RoleTypeId, RoleTypeId> CuffedRoleConversions { get; set; } = new Dictionary<RoleTypeId, RoleTypeId>
        {
            { RoleTypeId.FacilityGuard, RoleTypeId.ChaosRifleman },
            { RoleTypeId.ChaosRifleman, RoleTypeId.NtfPrivate },
            { RoleTypeId.ChaosRepressor, RoleTypeId.NtfPrivate },
            { RoleTypeId.ChaosMarauder, RoleTypeId.NtfPrivate },
            { RoleTypeId.ChaosConscript, RoleTypeId.NtfPrivate }
        };

        public bool Debug { get; set; } = false;
    }
}