namespace ScuutCore.Modules.RespawnTimer;

using System.Collections.Generic;
using System.ComponentModel;
using ScuutCore.API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("Whether debug messages shoul be shown in a server console.")]
    public bool Debug { get; private set; } = false;

    [Description("List of timer names that will be used:")]
    public List<string> Timers { get; private set; } = new()
    {
        "Template"
    };

    [Description("Whether the timer should be hidden for players in overwatch.")]
    public bool HideTimerForOverwatch { get; private set; } = true;
    public bool EnableSpectatorList { get; set; } = true;
}