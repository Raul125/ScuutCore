using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerStatsSystem;
using System.Linq;
using System.Reflection;

namespace ScuutCore.Modules.Health
{
    public class EventHandlers
    {
        private Health health;
        public EventHandlers(Health h)
        {
            health = h;
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (health.Config.HealthValues.ContainsKey(ev.NewRole))
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(2.5f, () =>
                {
                    ev.Player.Health = health.Config.HealthValues[ev.NewRole];
                    ev.Player.MaxHealth = health.Config.HealthValues[ev.NewRole];
                }));
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Attacker == null)
                return;

            if (health.Config.HealthOnKill.ContainsKey(ev.Attacker.Role))
            {
                if (ev.Attacker.Health + health.Config.HealthOnKill[ev.Attacker.Role] <= ev.Attacker.MaxHealth)
                    ev.Attacker.Health += health.Config.HealthOnKill[ev.Attacker.Role];
                else
                    ev.Attacker.Health = ev.Attacker.MaxHealth;
            }
        }
    }
}