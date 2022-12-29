using PlayerRoles;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Teslas
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Roles with disabled teslas")]
        public List<RoleTypeId> Roles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.FacilityGuard
        };
    }
}