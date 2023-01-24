using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.AntiAFK
{
    using ScuutCore.Modules.AntiAFK.Components;

    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnVerified(Player player)
        {
            if(!PermissionsHandler.IsPermitted(player.ReferenceHub.serverRoles.Permissions, PlayerPermissions.AFKImmunity))
                player.GameObject.AddComponent<AfkComponent>();
        }
    }
}