namespace ScuutCore.Modules.NukeRadiation
{
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.API.Features;
    using ScuutCore.Modules.NukeRadiation.Components;

    public sealed class EventHandlers : InstanceBasedEventHandler<NukeRadiation>
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerJoin(Player player)
        {
            player.GameObject.AddComponent<NukeRadiationComponent>();
        }
    }
}