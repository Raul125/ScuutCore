namespace ScuutCore.Modules.RoundSummary
{
    using System.ComponentModel;
    using API.Interfaces;

    public sealed class Config : IModuleConfig
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
        
        public bool ShowFirstDeath { get; set; } = true;
        public string FirstDeathMessage { get; set; } = "<size=30><color=yellow><b>{player}</color> was the first to die {time} into the round!</b></size>";
        public string NoFirstDeathMessage { get; set; } = "No one died!";

        public bool ShowMostDamage { get; set; } = true;
        public bool ExcludeScpsFromMostDamage { get; set; } = true;
        public int DamageLimit { get; set; } = -1;
        public string MostDamageMessage { get; set; } = "<size=30><color=yellow><b>{player}</color> did the most damage, they did <color=red>{damage} damage</b></color></size>";
        public string NoMostDamageMessage { get; set; } = "No one did any damage!";

        public float DisplayDelay { get; set; } = 1f;
    }
}