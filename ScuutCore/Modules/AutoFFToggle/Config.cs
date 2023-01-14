using ScuutCore.API;

namespace ScuutCore.Modules.AutoFFToggle
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}