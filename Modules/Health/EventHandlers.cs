using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
using PlayableScps.Interfaces;
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

            if (health.Config.AhpValues.TryGetValue(ev.NewRole, out var ahp))
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(3f, () =>
                {
                    var module = ev.Player.ReferenceHub.playerStats.GetModule<AhpStat>();
                    if (ev.Player.CurrentScp is IShielded shielded)
                    {
                        var shield = shielded.Shield;
                        shield.Limit = ahp.Limit;
                        shield.CurrentAmount = ahp.Amount;
                        shield.DecayRate = ahp.Decay;
                        shield.Efficacy = ahp.Efficacy;
                        shield.SustainTime = ahp.SustainTime;
                        shield.GetType().GetField("Persistant", BindingFlags.Instance | BindingFlags.Public).SetValue(shield, ahp.Persistent);
                    }
                    else
                    {
                        foreach (var process in ev.Player.ActiveArtificialHealthProcesses)
                        {
                            module.ServerKillProcess(process.KillCode);
                        }

                        ev.Player.AddAhp(ahp.Amount, ahp.Limit, ahp.Decay, ahp.Efficacy, ahp.SustainTime, ahp.Persistent);
                    }
                }));
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Killer == null)
                return;

            if (health.Config.HealthOnKill.ContainsKey(ev.Killer.Role))
            {
                if (ev.Killer.Health + health.Config.HealthOnKill[ev.Killer.Role] <= ev.Killer.MaxHealth)
                    ev.Killer.Health += health.Config.HealthOnKill[ev.Killer.Role];
                else
                    ev.Killer.Health = ev.Killer.MaxHealth;
            }

            if (health.Config.AhpOnKill.ContainsKey(ev.Killer.Role))
            {
                if (ev.Killer.CurrentScp is IShielded shielded)
                {
                    shielded.Shield.CurrentAmount += health.Config.AhpOnKill[ev.Killer.Role.Type];
                }
                else
                {
                    ev.Killer.ArtificialHealth += health.Config.AhpOnKill[ev.Killer.Role.Type];
                }
            }
        }
    }
}