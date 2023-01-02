using ScuutCore.API;

namespace ScuutCore.Modules.AutoNuke
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public float AutoNukeStartTime { get; set; } = 1080f;
        public float AutoNukeWarn { get; set; } = 780f;

        // Nuke Warn
        public PluginAPI.Core.Broadcast AutoNukeWarnBroadcast { get; set; } = new PluginAPI.Core.Broadcast
        {
            Show = true,
            Duration = 10,
            Content = "<color=#FF0000><b>WARNING - Warhead will automatically start in 5 minutes. You cannot cancel this. </b></color>"
        };

        public CassieConfig AutoNukeCassieWarn { get; set; } = new CassieConfig
        {
            isSubtitles = true,
            Text = "automatic warhead will detonate in 5 minutes",
            customSubtitle = "<color=#FF0000><b>Head to the exit!</b></color>"
        };

        public HintConfig AutoNukeWarnHint { get; set; } = new HintConfig
        {
            Time = 15,
            Message = "<color=#FF0000><b>Head to the exit!</b></color>",
        };

        // No Disable
        public HintConfig CantDisableHint { get; set; } = new HintConfig
        {
            Time = 7,
            Message = "<color=#FF0000>You cannot disable the warhead! EVACUATE RIGHT NOW!</color>"
        };

        // Nuke Start
        public HintConfig AutoNukeStartHint { get; set; } = new HintConfig
        {
            Time = 15,
            Message = "<color=#FF0000><b>RUN</b></color>",
        };

        public CassieConfig AutoNukeCassieStart { get; set; } = new CassieConfig
        {
            isSubtitles = true,
            Text = "automatic warhead has been activated",
            customSubtitle = "AutoNuke has started"
        };

        public PluginAPI.Core.Broadcast AutoNukeStartBroadcast { get; set; } = new PluginAPI.Core.Broadcast
        {
            Show = true,
            Duration = 10,
            Content = "<color=#FF0000><b>WARNING - Automatic Warhead has been activated. You cannot cancel this. </b></color>"
        };
    }
}