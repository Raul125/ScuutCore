namespace ScuutCore.Modules.Restart;

using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;
}