namespace ScuutCore.Modules.RainbowTags
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;
    using PluginAPI.Core;

    public sealed class Config : IModuleConfig
    {
        private float tagInterval = 0.5f;

        public bool IsEnabled { get; set; } = true;

        [Description("The time, in seconds, between switching to the next color in a sequence.")]
        public float TagInterval
        {
            get => tagInterval;
            set
            {
                if (value < 0.5f)
                {
                    value = 0.5f;
                    Log.Warning($"The {nameof(TagInterval)} config cannot be set below 0.5 and has been automatically clamped.");
                }

                tagInterval = value;
            }
        }

        [Description("A collection of group names with their respective color sequences.")]
        public Dictionary<string, string[]> Sequences { get; set; } = new Dictionary<string, string[]>
        {
            ["owner"] = new[]
            {
                "red",
                "orange",
                "yellow",
                "green",
                "blue_green",
                "magenta",
            },
            ["admin"] = new[]
            {
                "green",
                "silver",
                "crimson",
            },
        };
    }
}