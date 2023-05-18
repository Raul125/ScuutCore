namespace ScuutCore.Modules.NukeRadiation
{
    using CustomPlayerEffects;
    using MEC;
    using PlayerStatsSystem;
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

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            Timing.CallDelayed(1f, () =>
            {
                if (player.EffectsManager.GetEffect<Decontaminating>())
                    player.EffectsManager.DisableEffect<Decontaminating>();
            });
        }
    }
}