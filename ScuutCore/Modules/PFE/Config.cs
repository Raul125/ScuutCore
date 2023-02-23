namespace ScuutCore.Modules.PFE
{
    using System.Collections.Generic;
    using API.Interfaces;
    using PlayerRoles;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public List<RoleTypeId> ExplodingRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Scp173
        };
    }
}