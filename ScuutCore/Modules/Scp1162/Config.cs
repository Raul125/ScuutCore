﻿namespace ScuutCore.Modules.Scp1162
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using API.Interfaces;

    public sealed class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Use Hints instead of Broadcast?")]
        public bool UseHints { get; set; } = true;

        [Description("Change the message that displays when you drop an item through SCP-1162.")]
        public string ItemDropMessage { get; set; } = "<i>You try to drop the item through <color=yellow>SCP-1162</color> to get another...</i>";
        public ushort ItemDropMessageDuration { get; set; } = 5;

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
}