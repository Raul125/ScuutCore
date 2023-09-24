namespace ScuutCore.Modules.LastPerson;

using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool PrintDebug { get; set; } = false;
    public string Message { get; set; } = "You are the last person alive on your team!";
    public float Delay { get; set; } = 60f;
    public float Duration { get; set; } = 5f;
}