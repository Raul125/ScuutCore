using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.Better106Attack
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("The delay (in seconds) between 106 hiting player and player starting falling into pocket.")]
        public float Delay { get; set; } = 1f;

        [Description("The number of \"ticks\" before teleporting player to the pocket dimension.")]
        public uint Ticks { get; set; } = 125;
    }
}