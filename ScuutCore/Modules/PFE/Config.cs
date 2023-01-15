namespace ScuutCore.Modules.PFE
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}