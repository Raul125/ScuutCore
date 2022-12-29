using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
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
                    if (ev.Player.Role.Base is IShielded shielded)
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
            if (ev.Attacker == null)
                return;

            if (health.Config.HealthOnKill.ContainsKey(ev.Attacker.Role))
            {
                if (ev.Attacker.Health + health.Config.HealthOnKill[ev.Attacker.Role] <= ev.Attacker.MaxHealth)
                    ev.Attacker.Health += health.Config.HealthOnKill[ev.Attacker.Role];
                else
                    ev.Attacker.Health = ev.Attacker.MaxHealth;
            }

            if (health.Config.AhpOnKill.ContainsKey(ev.Attacker.Role))
            {
                if (ev.Attacker.Role.Base is IShielded shielded)
                {
                    shielded.Shield.CurrentAmount += health.Config.AhpOnKill[ev.Attacker.Role.Type];
                }
                else
                {
                    ev.Attacker.ArtificialHealth += health.Config.AhpOnKill[ev.Attacker.Role.Type];
                }
            }
        }
    }
}