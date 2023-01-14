using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;
using PluginAPI.Core;
using PlayerRoles;

namespace ScuutCore.Modules.CuffedTK
{
    using ScuutCore.API.Interfaces;

    public class Config : IModuleConfig
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
    }
}