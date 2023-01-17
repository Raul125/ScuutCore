namespace ScuutCore.Modules.Subclasses
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}