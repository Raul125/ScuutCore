using PlayerRoles;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.ScpSpeech
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("A collection of roles that can speak regardless of their permissions.")]
        public List<RoleTypeId> GlobalTalking { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp939,
        };
    }
}