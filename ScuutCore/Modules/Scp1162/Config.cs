using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;

namespace ScuutCore.Modules.Scp1162
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Use Hints instead of Broadcast?")]
        public bool UseHints { get; set; } = true;

        [Description("Change the message that displays when you drop an item through SCP-1162.")]
        public string ItemDropMessage { get; set; } = "<i>You try to drop the item through <color=yellow>SCP-1162</color> to get another...</i>";
        public ushort ItemDropMessageDuration { get; set; } = 5;

        [Description("The list of item chances.")]
        public List<ItemType> Chances { get; set; } = new List<ItemType>
        {
            ItemType.GrenadeFlash,
            ItemType.ArmorLight,
            ItemType.MicroHID,
            ItemType.KeycardO5,
            ItemType.Flashlight,
            ItemType.Coin
        };
    }
}