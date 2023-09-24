namespace ScuutCore.Modules.Scp096Notifications;

using System.Collections.Generic;
using System.ComponentModel;
using API.Interfaces;
using PlayerRoles;

public sealed class Config : IModuleConfig
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

    public float HintDuration { get; set; } = 10f;

    [Description("Change the display strings of each class (applies to SCP-096's notification).")]
    public Dictionary<RoleTypeId, string> RoleStrings { get; set; } = new Dictionary<RoleTypeId, string>
    {
        [RoleTypeId.ClassD] = "Class-D Personnel",
        [RoleTypeId.Scientist] = "Scientist",
        [RoleTypeId.FacilityGuard] = "Facility Guard",
        [RoleTypeId.NtfPrivate] = "NTF Private",
        [RoleTypeId.NtfSergeant] = "NTF Sergeant",
        [RoleTypeId.NtfSpecialist] = "NTF Specialist",
        [RoleTypeId.NtfCaptain] = "NTF Captain",
        [RoleTypeId.ChaosRifleman] = "Chaos Rifleman",
        [RoleTypeId.ChaosConscript] = "Chaos Conscript",
        [RoleTypeId.ChaosRepressor] = "Chaos Repressor",
        [RoleTypeId.ChaosMarauder] = "Chaos Marauder",
        [RoleTypeId.Tutorial] = "Tutorial",
    };
}