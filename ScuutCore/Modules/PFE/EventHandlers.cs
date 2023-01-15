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
        private PFE pfe;
        public EventHandlers(PFE plugin) => pfe = plugin;
        
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (player is null || !pfe.Config.ExplodingRoles.Contains(player.Role))
                return;

            PlayerDeathEffects.PlayExplosionEffect(player);
        }
    }
}