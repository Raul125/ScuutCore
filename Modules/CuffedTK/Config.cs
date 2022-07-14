using Exiled.API.Enums;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Features;

namespace ScuutCore.Modules.CuffedTK
{
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
            Team.MTF,
            Team.RSC
        };

        [Description("What Team should not be allowed to damage an Cuffed Scientists! CDP = Class-D, CHI = Chaos, MTF = Nine-Tailed Fox, RSC = Scientists, TUT = Tutorial")]
        public HashSet<Team> DisallowDamagetoScientists { get; set; } = new HashSet<Team>
        {
            Team.CDP,
            Team.CHI
        };

        public HashSet<Team> DisallowDamagetoChaos { get; set; } = new HashSet<Team>
        {
            Team.MTF,
            Team.RSC
        };

        public HashSet<Team> DisallowDamagetoMTF { get; set; } = new HashSet<Team>
        {
            Team.CDP,
            Team.CHI
        };

        [Description("0 To disable")]
        public int DamageTypeTime { get; set; } = 3;

        [Description("What hint should be displayed when trying to damage a Cuffed D-Class with a Disallowed DamageType? %PLAYER% will be replaced with the Target Username and %DAMAGETYPE% will be replaced with the DamageType. Time, 0 to disable")]
        public string DamageTypesHint { get; set; } = "You cannot damage %PLAYER% with %DAMAGETYPE%!";

        [Description("What DamageType should not be allowed to damage a Cuffed D-Class or Cuffed Scientist? Check https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.DamageType.html for all DamageTypes")]
        public HashSet<DamageType> DisallowedDamageTypes { get; set; } = new HashSet<DamageType>
        {
            DamageType.Explosion,
            DamageType.FriendlyFireDetector,
            DamageType.Falldown
        };
    }
}