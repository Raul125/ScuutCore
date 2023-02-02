using ScuutCore.API;

namespace ScuutCore.Modules.AutoNuke
{
    using ScuutCore.API.Features;
    using ScuutCore.API.Interfaces;
    using YamlDotNet.Serialization;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public float AutoNukeStartTimeMinutes { get; set; } = 18f;
        public float AutoNukeEndTimeSeconds { get; set; } = 0f;
        public float AutoNukeWarnMinutes { get; set; } = 13f;
        public float AutoNukeWarnSeconds { get; set; } = 0f;

        [YamlIgnore]
        public float AutoNukeStartTime => AutoNukeStartTimeMinutes * 60f + AutoNukeEndTimeSeconds;
        [YamlIgnore]
        public float AutoNukeWarn => AutoNukeWarnMinutes * 60f + AutoNukeWarnSeconds;

        // Nuke Warn
        public BroadcastConfig AutoNukeWarnBroadcast { get; set; } = new BroadcastConfig
        {
            AbleToShow = true,
            Duration = 10,
            Text = "<color=#FF0000><b>WARNING - Warhead will automatically start in 5 minutes. You cannot cancel this. </b></color>"
        };

        public CassieConfig AutoNukeCassieWarn { get; set; } = new CassieConfig
        {
            isSubtitles = true,
            Text = "automatic warhead will detonate in 5 minutes",
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
        };

        public BroadcastConfig AutoNukeStartBroadcast { get; set; } = new BroadcastConfig
        {
            AbleToShow = true,
            Duration = 10,
            Text = "<color=#FF0000><b>WARNING - Automatic Warhead has been activated. You cannot cancel this. </b></color>"
        };
    }
}