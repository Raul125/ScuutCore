using ScuutCore.API;

namespace ScuutCore.Modules.AdminTools
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}