namespace ScuutCore.Modules.Spawn3114;

using ScuutCore.API.Interfaces;

public class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;
    public float Chance { get; set; } = 0.5f;
    public int MaxScp3114 { get; set; } = 2;
}