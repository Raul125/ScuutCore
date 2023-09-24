namespace ScuutCore.Modules.Teslas;

using System.Collections.Generic;
using System.ComponentModel;
using API.Interfaces;
using PlayerRoles;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("Roles with disabled teslas")]
    public List<RoleTypeId> Roles { get; set; } = new List<RoleTypeId>
    {
        RoleTypeId.FacilityGuard
    };
}