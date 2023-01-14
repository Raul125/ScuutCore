using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.RoundSummary
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Show most kills:")]
        public bool ShowKills { get; set; } = true;

        [Description("Most kills messeage:")]
        public string KillsMessage { get; set; } = "<size=30><color=yellow><b>{player}</color> had the most kills, they had <color=red>{kills} kills</b></color></size>";

        public string NoKillsMessage { get; set; } = "No one got any kills.";

        [Description("Show first escapee:")]
        public bool ShowEscapee { get; set; } = true;

        [Description("First escapee message:")]
        public string EscapeeMessage { get; set; } = "<size=30><color=yellow><b>{player}</color> was the first to escape at <color=red>{time}</color> as <color={roleColor}>{role}</color></b></size>";

        public string NoEscapeeMessage { get; set; } = "No one escaped!";

        [Description("Show first one who killed a SCP:")]
        public bool ShowScpFirstKill { get; set; } = true;

        [Description("First SCP killer message:")]
        public string ScpFirstKillMessage { get; set; } = "<size=30><color={killerColor}><b>{player}</color> as {killerRole} was the first person to contain a {killedScp}!</b></size>";

        public string NoScpKillMessage { get; set; } = "No one killed a SCP!";
    }
}