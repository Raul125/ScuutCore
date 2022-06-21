using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.Chaos
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Chaos cassie when spawning")]
        public string ChaosCassie { get; set; } = ".g7 .g7 Chaos Insurgency .g7 .g7";
        public float CassieDelay { get; set; } = 0.5f;
    }
}