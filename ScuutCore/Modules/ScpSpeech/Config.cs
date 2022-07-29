using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.ScpSpeech
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("A collection of roles that can speak regardless of their permissions.")]
        public List<RoleType> GlobalTalking { get; set; } = new List<RoleType>
        {
            RoleType.Scp049,
            RoleType.Scp93953,
            RoleType.Scp93989,
        };
    }
}