using ScuutCore.API;
using System.ComponentModel;

namespace ScuutCore.Modules.RoundSummary
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Show most kills:")]
        public bool ShowKills { get; private set; } = true;

        [Description("Most kills messeage:")]
        public string KillsMessage { get; private set; } = "<size=30><color=yellow><b>{player}</color> had the most kills, they had <color=red>{kills} kills</b></color></size>";

        public string NoKillsMessage { get; private set; } = "No one got any kills.";

        [Description("Show first escapee:")]
        public bool ShowEscapee { get; private set; } = true;

        [Description("First escapee message:")]
        public string EscapeeMessage { get; private set; } = "<size=30><color=yellow><b>{player}</color> was the first to escape at <color=red>{time}</b></color></size>";

        public string NoEscapeeMessage { get; private set; } = "No one escaped!";

        [Description("Show first one who killed a SCP:")]
        public bool ShowScpFirstKill { get; private set; } = true;

        [Description("First SCP killer message:")]
        public string ScpFirstKillMessage { get; private set; } = "<size=30><color=yellow><b>{player}</color> was the first person to contain a scp!</b></size>";

        public string NoScpKillMessage { get; private set; } = "No one killed a SCP!";
    }
}