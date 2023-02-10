﻿namespace ScuutCore.Modules.RespawnTimer
{
    using ScuutCore.API.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages shoul be shown in a server console.")]
        public bool Debug { get; private set; } = false;

        [Description("List of timer names that will be used:")]
        public List<string> Timers { get; private set; } = new()
        {
            "Template"
        };

        [Description("Whether the timer should be reloaded each round. Useful if you have many different timers designed.")]
        public bool ReloadTimerEachRound { get; private set; } = true;

        [Description("Whether the timer should be hidden for players in overwatch.")]
        public bool HideTimerForOverwatch { get; private set; } = true;
    }
}