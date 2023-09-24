namespace ScuutCore.Modules.PFE;

using ScuutCore.API.Features;
using API.Helpers;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

public sealed class EventHandlers : InstanceBasedEventHandler<PFE>
{
    [PluginEvent(ServerEventType.PlayerDeath)]
    public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
    {
        if (player is null || !Module.Config.ExplodingRoles.Contains(player.Role))
            return;

        PlayerDeathEffects.PlayExplosionEffect(player);
    }
}