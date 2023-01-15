namespace ScuutCore.Modules.PFE
{
    using System.Collections.Generic;
    using PlayerRoles;
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public List<RoleTypeId> ExplodingRoles { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.Scp173
        };
    }
}