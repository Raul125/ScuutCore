namespace ScuutCore.Modules.ScpSpeech
{
    using System.Collections.Generic;
    using API.Interfaces;
    using PlayerRoles;
    public sealed class Config : IModuleConfig
    {

        public bool IsEnabled { get; set; } = true;

        public List<RoleTypeId> PermittedRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939
        };

    }
}