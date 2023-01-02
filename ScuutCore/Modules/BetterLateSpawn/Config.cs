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

        [Description("Chances as what class you spawn:")]
        public float FacilityGuardChance { get; set; } = 10f;
        public float ScientistChance { get; set; } = 20f;
        public float ClassDChance { get; set; } = 70f;

        [Description("What should it broadcast when you late spawn.")]
        public PluginAPI.Core.Broadcast Broadcast { get; set; } = new PluginAPI.Core.Broadcast
        {
            Show = true,
            Content = "You joined late and have been spawned",
            Duration = 10,
            Type = global::Broadcast.BroadcastFlags.Normal
        };
    }
}