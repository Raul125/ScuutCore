using Exiled.API.Enums;
using ScuutCore.API;
using System.Collections.Generic;
using System.ComponentModel;
using PluginAPI.Core;
using PlayerRoles;

namespace ScuutCore.Modules.Replacer
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        public PluginAPI.Core.Broadcast BroadCast { get; set; } = new PluginAPI.Core.Broadcast
        {
            Duration = 10,
            Content = "<i>You have replaced a disconnected player</i>",
            Show = true,
            Type = Broadcast.BroadcastFlags.Normal
        };

        public int DontReplaceTime { get; set; } = 360;

        public List<RoleTypeId> DisallowedRolesToReplace { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.Tutorial,
            RoleTypeId.Overwatch,
            RoleTypeId.Spectator,
            RoleTypeId.None,
        };
    }
}