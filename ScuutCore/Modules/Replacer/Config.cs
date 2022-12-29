using Exiled.API.Enums;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Features;
using PlayerRoles;

namespace ScuutCore.Modules.Replacer
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public Exiled.API.Features.Broadcast BroadCast { get; set; } = new Exiled.API.Features.Broadcast
        {
            Duration = 10,
            Content = "<i>You have replaced a disconnected player</i>",
            Show = true,
            Type = Broadcast.BroadcastFlags.Normal
        };
    }
}