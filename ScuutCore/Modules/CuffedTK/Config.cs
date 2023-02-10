namespace ScuutCore.Modules.CuffedTK
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;
    using PlayerRoles;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("0 To disable")]
        public int AttackerHintTime { get; set; } = 3;

        [Description("What hint should the Attacker be displayed when trying to damage a Cuffed D-Class? %PLAYER% will be replaced with the Target Username. Time, 0 to disable")]
        public string AttackerHint { get; set; } = "You cannot damage %PLAYER% because he is cuffed!";

        [Description("What Team should not be allowed to damage an Cuffed D-Class! CDP = Class-D, CHI = Chaos, MTF = Nine-Tailed Fox, RSC = Scientists, TUT = Tutorial")]
        public HashSet<Team> DisallowDamagetoClassD { get; set; } = new HashSet<Team>
        {
            Team.FoundationForces,
            Team.Scientists
        };

        [Description("What Team should not be allowed to damage an Cuffed Scientists! CDP = Class-D, CHI = Chaos, MTF = Nine-Tailed Fox, RSC = Scientists, TUT = Tutorial")]
        public HashSet<Team> DisallowDamagetoScientists { get; set; } = new HashSet<Team>
        {
            Team.ClassD,
            Team.ChaosInsurgency
        };

        public HashSet<Team> DisallowDamagetoChaos { get; set; } = new HashSet<Team>
        {
            Team.FoundationForces,
            Team.Scientists
        };

        public HashSet<Team> DisallowDamagetoMTF { get; set; } = new HashSet<Team>
        {
            Team.ClassD,
            Team.ChaosInsurgency
        };

        public bool OnlyAllowCufferToRemoveHandcuffs { get; set; } = true;
        public string YouCantUnCuffMessage { get; set; } = "You can't uncuff {player}";
    }
}