namespace ScuutCore.Modules.Scp008;

using ScuutCore.API.Features;
using ScuutCore.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

public class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("Percent chance to create infection.")]
    public int InfectionChance { get; set; } = 100;

    [Description("Percent chance to successfully cure.")]
    public Dictionary<ItemType, int> CureChance { get; set; } = new()
    {
        { ItemType.SCP500, 100 },
        { ItemType.Medkit, 60 },
        { ItemType.Painkillers, 30 }
    };

    public HintConfig InfectedHint { get; set; } = new()
    {
        Message = "You've been infected!\nUse SCP-500, medkit or painkillers to be cured!",
        Time = 5
    };

    public HintConfig CuredHint { get; set; } = new()
    {
        Message = "You've been cured!",
        Time = 5
    };

    public bool DropInventory { get; set; } = true;

    public List<RoleTypeId> BlacklistedRoles { get; set; } = new()
    {
    };
}