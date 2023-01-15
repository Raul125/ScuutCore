namespace ScuutCore.Modules.PFE
{
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.API.Helpers;

    public class EventHandlers
    {
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (player is null || player.Role != RoleTypeId.Scp173)
                return;

            PlayerDeathEffects.PlayExplosionEffect(player);
        }
    }
}