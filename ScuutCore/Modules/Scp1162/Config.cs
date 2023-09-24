namespace ScuutCore.Modules.Scp1162;

using System.Collections.Generic;
using System.ComponentModel;
using API.Interfaces;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("Use Hints instead of Broadcast?")]
    public bool UseHints { get; set; } = true;

    [Description("What message will be displayed when using SCP-1162?")]
    public string ItemDropMessage { get; set; } = "<b>You dropped a <color=green>{dropitem}</color> through <color=yellow>SCP-1162</color>, and received a <color=red>{giveitem}</color></b>";

    [Description("From what distance can SCP-1162 be used?")]
    public float SCP1162Distance { get; set; } = 2f;

    [Description("Will the hands be cut off if the item is not in the hands?")]
    public bool CuttingHands { get; set; } = true;

    [Description("What is the chance that the hands will be cut off if the item is not in the hands")]
    public int ChanceCutting { get; set; } = 40;

    [Description("If this item is enabled, the hands will not be cut off only when the player threw item")]
    public bool OnlyThrow { get; set; } = false;

    [Description("The list of item chances.")]
    public Dictionary<ItemType, int> Chances { get; set; } = new Dictionary<ItemType, int>
    {
        {
            ItemType.GrenadeFlash, 20
        },
        {
            ItemType.ArmorLight, 10
        },
        {
            ItemType.MicroHID, 10
        },
        {
            ItemType.KeycardO5, 10
        },
        {
            ItemType.Flashlight, 20
        },
        {
            ItemType.Coin, 30
        }
    };
}