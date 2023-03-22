namespace ScuutCore.Modules.KeepCola
{
    using ScuutCore.API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}