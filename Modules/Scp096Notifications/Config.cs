using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Scp096Notifications
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Whether people who view Scp096's face will be notified.")]
        public bool Enable096SeenMessage { get; set; } = true;

        [Description("The message to show people who become a target of Scp096.")]
        public string Scp096SeenMessage { get; set; } = "You are a target of SCP-096!";

        [Description("Whether Scp096 will be notified when someone views their face.")]
        public bool Enable096NewTargetMessage { get; set; } = true;

        [Description("The message to show Scp096 when they gain a target.")]
        public string Scp096NewTargetMessage { get; set; } = "<b>$name</b> has viewed your face! They are a <b>$class</b>.";

        [Description("Change the display strings of each class (applies to SCP-096's notification).")]
        public Dictionary<RoleType, string> RoleStrings { get; set; } = new Dictionary<RoleType, string>
        {
            [RoleType.ClassD] = "Class-D Personnel",
            [RoleType.Scientist] = "Scientist",
            [RoleType.FacilityGuard] = "Facility Guard",
            [RoleType.NtfPrivate] = "NTF Private",
            [RoleType.NtfSergeant] = "NTF Sergeant",
            [RoleType.NtfSpecialist] = "NTF Specialist",
            [RoleType.NtfCaptain] = "NTF Captain",
            [RoleType.ChaosRifleman] = "Chaos Rifleman",
            [RoleType.ChaosConscript] = "Chaos Conscript",
            [RoleType.ChaosRepressor] = "Chaos Repressor",
            [RoleType.ChaosMarauder] = "Chaos Marauder",
            [RoleType.Tutorial] = "Tutorial",
        };
    }
}