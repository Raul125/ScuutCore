using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.AntiAFK
{
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