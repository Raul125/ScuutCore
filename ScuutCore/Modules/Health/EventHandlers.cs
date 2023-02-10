namespace ScuutCore.Modules.Health
{
    using API.Features;
    using MEC;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public sealed class EventHandlers : InstanceBasedEventHandler<Health>
    {
        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (Module.Config.HealthValues.ContainsKey(newRole))
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(2.5f, () =>
                {
                    player.Health = Module.Config.HealthValues[newRole];
                }));
            }
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null)
                return;

            if (!Module.Config.HealthOnKill.ContainsKey(attacker.Role))
                return;
            if (attacker.Health + Module.Config.HealthOnKill[attacker.Role] <= attacker.MaxHealth)
                attacker.Health += Module.Config.HealthOnKill[attacker.Role];
            else
                attacker.Health = attacker.MaxHealth;
        }
    }
}