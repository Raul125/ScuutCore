namespace ScuutCore.Modules.Health
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;
    using Modules;
    using PlayerRoles;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("A list of roles and what their default starting health should be.")]
        public Dictionary<RoleTypeId, int> HealthValues { get; set; } = new Dictionary<RoleTypeId, int>
        {
            {
                RoleTypeId.Scp173, 3200
            },
            {
                RoleTypeId.NtfCaptain, 150
            }
        };

        public Dictionary<RoleTypeId, AhpRole> AhpValues { get; set; } = new Dictionary<RoleTypeId, AhpRole>
        {
            {
                RoleTypeId.Scp173, new AhpRole
                {
                    Amount = 760,
                    Decay = 0,
                    Efficacy = 0.75f,
                    Limit = 760,
                    Persistent = true,
                    SustainTime = 10f,
                }
            },
        };

        public Dictionary<RoleTypeId, float> AhpOnKill { get; set; } = new Dictionary<RoleTypeId, float>
        {
            {
                RoleTypeId.Scp173, 5
            },
        };

        [Description("A list of roles and how much health they should be given when they kill someone.")]
        public Dictionary<RoleTypeId, float> HealthOnKill { get; set; } = new Dictionary<RoleTypeId, float>
        {
            {
                RoleTypeId.Scp173, 0
            },
            {
                RoleTypeId.Scp939, 10
            }
        };
    }
}