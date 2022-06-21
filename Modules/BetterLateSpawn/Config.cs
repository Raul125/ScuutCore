using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.BetterLateSpawn
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("How long after the round started should you still be able to spawn (seconds)")]
        public double SpawnTime { get; set; } = 60;

        [Description("Random role when joining")]
        public List<RoleType> Roles { get; set; } = new List<RoleType>
        {
            RoleType.FacilityGuard,
            RoleType.ClassD,
            RoleType.Scientist
        };

        [Description("What should it broadcast when you late spawn.")]
        public string Broadcast { get; set; } = "You joined late and have been spawned";
    }
}