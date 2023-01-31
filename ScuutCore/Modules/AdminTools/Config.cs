namespace ScuutCore.Modules.AdminTools
{
    using ScuutCore.API.Interfaces;
    using ScuutCore.API;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}