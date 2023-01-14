using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.Health
{
    public class EventHandlers
    {
        private Health health;
        public EventHandlers(Health h)
        {
            health = h;
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (health.Config.HealthValues.ContainsKey(newRole))
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(2.5f, () =>
                {
                    player.Health = health.Config.HealthValues[newRole];
                }));
            }
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null)
                return;

            if (health.Config.HealthOnKill.ContainsKey(attacker.Role))
            {
                if (attacker.Health + health.Config.HealthOnKill[attacker.Role] <= attacker.MaxHealth)
                    attacker.Health += health.Config.HealthOnKill[attacker.Role];
                else
                    attacker.Health = attacker.MaxHealth;
            }
        }
    }
}