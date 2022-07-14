using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Health
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("A list of roles and what their default starting health should be.")]
        public Dictionary<RoleType, int> HealthValues { get; set; } = new Dictionary<RoleType, int>()
        {
            {
                RoleType.Scp173,
                3200
            },
            {
                RoleType.NtfCaptain,
                150
            }
        };

        public Dictionary<RoleType, AhpRole> AhpValues { get; set; } = new Dictionary<RoleType, AhpRole>()
        {
            {
                RoleType.Scp173,
                new AhpRole
                {
                    Sustain = 1f,
                    Amount = 760,
                    Decay = 0,
                    Efficacy = 0.75f,
                    Limit = 760,
                    Persistent = true,
                }
            },
        };

        public Dictionary<RoleType, float> AhpOnKill { get; set; } = new Dictionary<RoleType, float>()
        {
             {
                RoleType.Scp173,
                5
            },
        };

        [Description("A list of roles and how much health they should be given when they kill someone.")]
        public Dictionary<RoleType, float> HealthOnKill { get; set; } = new Dictionary<RoleType, float>()
        {
            {
                RoleType.Scp173,
                0
            },
            {
                RoleType.Scp93953,
                10
            },
            {
                RoleType.Scp93989,
                20
            }
        };
    }
}