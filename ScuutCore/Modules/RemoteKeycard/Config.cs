namespace ScuutCore.Modules.RemoteKeycard
{
    using ScuutCore.API.Interfaces;
    using PlayerRoles;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("RolesType that are in this list cannot use RemoteKeycard")]
        public List<RoleTypeId> BlackListRole { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.None
        };


        [Description("RemoteKeycard will not work with doors that are in this list")]
        public List<string> BlacklistedDoors { get; set; } = new List<string>()
        {
        };

        [Description("RemoteKeycard will not work with lockers that are on this list")]
        public List<string> BlacklistedLockers { get; set; } = new List<string>()
        {
        };
    }
}