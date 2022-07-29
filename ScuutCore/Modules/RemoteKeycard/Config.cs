using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.RemoteKeycard
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether  Amnesia affects the usage of keycards.")]
        public bool AmnesiaMatters { get; set; } = true;

        [Description("Whether this plugin works on generators.")]
        public bool AffectGenerators { get; set; } = true;

        [Description("Whether this plugin works on Warhead's panel.")]
        public bool AffectWarheadPanel { get; set; } = true;

        [Description("Whether this plugin works on SCP lockers.")]
        public bool AffectScpLockers { get; set; } = true;

        [Description("Whether this plugin works on doors.")]
        public bool AffectDoors { get; set; } = true;
    }
}