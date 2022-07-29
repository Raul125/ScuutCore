using ScuutCore.API;

namespace ScuutCore.Modules.AutoFFToggle
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}