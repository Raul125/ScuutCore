using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.AutoNuke
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public float AutoNukeStartTime { get; set; } = 1080f;
        public float AutoNukeWarn { get; set; } = 780f;

        // Nuke Warn
        public Exiled.API.Features.Broadcast AutoNukeWarnBroadcast { get; set; } = new Exiled.API.Features.Broadcast
        {
            Show = true,
            Duration = 10,
            Content = "<color=#FF0000><b>WARNING - Warhead will automatically start in 5 minutes. You cannot cancel this. </b></color>"
        };

        public string AutoNukeCassieWarn { get; set; } = "automatic warhead will detonate in 5 minutes";

        public string AutoNukeWarnHint { get; set; } = "<color=#FF0000><b>Head to the exit!</b></color>";
        public float AutoNukeWarnHintDuration { get; set; } = 15f;

        // No Disable
        public string CantDisableHint { get; set; } = "<color=#FF0000>You cannot disable the warhead! EVACUATE RIGHT NOW!</color>";
        public float CantDisableHintTime { get; set; } = 7f;

        // Nuke Start
        public float AutoNukeStartHintDuration { get; set; } = 15f;
        public string AutoNukeStartHint { get; set; } = "<color=#FF0000><b>RUN</b></color>";

        public string AutoNukeCassieStart {get; set;} = "automatic warhead has been activated";

        public Exiled.API.Features.Broadcast AutoNukeStartBroadcast { get; set; } = new Exiled.API.Features.Broadcast
        {
            Show = true,
            Duration = 10,
            Content = "<color=#FF0000><b>WARNING - Automatic Warhead has been activated. You cannot cancel this. </b></color>"
        };
    }
}