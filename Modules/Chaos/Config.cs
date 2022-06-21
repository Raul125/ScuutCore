using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.Chaos
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Chaos cassie when spawning")]
        public string ChaosCassie = ".g7 .g7 Chaos Insurgency .g7 .g7";
    }
}