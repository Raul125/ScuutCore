namespace ScuutCore.Modules.KillMessages;

using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    public string Message { get; set; } = "Killed <color=red>{name}</color>";
}