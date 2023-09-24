namespace ScuutCore.Modules.AutoFFToggle;

using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;
}