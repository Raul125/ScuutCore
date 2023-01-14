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
            player.GameObject.AddComponent<AfkComponent>();
        }
    }
}