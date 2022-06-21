using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Teslas
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Roles with disabled teslas")]
        public List<RoleType> Roles = new List<RoleType>
        {
            RoleType.FacilityGuard
        };
    }
}