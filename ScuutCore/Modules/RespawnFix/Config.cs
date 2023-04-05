namespace ScuutCore.Modules.RespawnFix
{
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}