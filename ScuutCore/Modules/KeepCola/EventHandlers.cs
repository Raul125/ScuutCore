namespace ScuutCore.Modules.KeepCola
{
    using CustomPlayerEffects;
    using MEC;
    using PlayerRoles;
    using ScuutCore.API.Features;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    public sealed class EventHandlers : InstanceBasedEventHandler<KeepCola>
    {
        [PluginEvent(ServerEventType.PlayerEscape)]
        public void OnEscaping(Player player, RoleTypeId newRole)
        {
            if (player is null)
                return;

            if (player.EffectsManager.TryGetEffect(out Scp207 effect))
            {
                var strength = effect.Intensity;
                Timing.CallDelayed(2f, () =>
                {
                    player.EffectsManager.EnableEffect<Scp207>(strength);
                });
            }
        }
    }
}