using Exiled.Events.EventArgs;
using MEC;

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
            if (ev.Killer != null && health.Config.HealthOnKill.ContainsKey(ev.Killer.Role))
            {

                if (ev.Killer.Health + health.Config.HealthOnKill[ev.Killer.Role] <= ev.Killer.MaxHealth)
                    ev.Killer.Health += health.Config.HealthOnKill[ev.Killer.Role];
                else
                    ev.Killer.Health = ev.Killer.MaxHealth;
            }
        }
    }
}