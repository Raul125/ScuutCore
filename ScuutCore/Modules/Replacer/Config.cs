namespace ScuutCore.Modules.Replacer;

using System.Collections.Generic;
using API.Features;
using API.Interfaces;
using PlayerRoles;

public sealed class Config : IModuleConfig
{
    public bool IsEnabled { get; set; } = true;

    public BroadcastConfig BroadCast { get; set; } = new BroadcastConfig
    {
        Duration = 10,
        Text = "<i>You have replaced a disconnected player</i>",
        AbleToShow = true,
        BroadcastFlags = Broadcast.BroadcastFlags.Normal,
        ClearPrevious = false,
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