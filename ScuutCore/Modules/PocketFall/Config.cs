namespace ScuutCore.Modules.PocketFall;

using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;
    public float Delay { get; set; } = 1f;
    public int Ticks { get; set; } = 25;
}