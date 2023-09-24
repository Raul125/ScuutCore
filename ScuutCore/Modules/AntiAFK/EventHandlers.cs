namespace ScuutCore.Modules.AntiAFK;

using ScuutCore.API.Features;
using Components;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

public sealed class EventHandlers : IEventHandler
{

    [PluginEvent(ServerEventType.PlayerJoined)]
    public void OnVerified(Player player)
    {
        if (!PermissionsHandler.IsPermitted(player.ReferenceHub.serverRoles.Permissions, PlayerPermissions.AFKImmunity))
            player.GameObject.AddComponent<AfkComponent>();
    }
}