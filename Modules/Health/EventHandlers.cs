using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
using PlayerStatsSystem;

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
                    if (ev.NewRole.GetSide() == Exiled.API.Enums.Side.Scp)
                    {
                        switch (ev.NewRole)
                        {
                            case RoleType.Scp173:
                                {
                                    var script = ev.Player.Role.As<Exiled.API.Features.Roles.Scp173Role>().Script;
                                    script.Shield.SustainTime = ahp.Sustain;
                                    script.Shield.Limit = ahp.Limit;
                                    script.Shield.Efficacy = ahp.Efficacy;
                                    script.Shield.DecayRate = ahp.Decay;
                                    script.Shield.CurrentAmount = ahp.Amount;
                                    return;
                                }
                            case RoleType.Scp096:
                                {
                                    var script = ev.Player.Role.As<Exiled.API.Features.Roles.Scp096Role>().Script;
                                    script.Shield.SustainTime = ahp.Sustain;
                                    script.Shield.Limit = ahp.Limit;
                                    script.Shield.Efficacy = ahp.Efficacy;
                                    script.Shield.DecayRate = ahp.Decay;
                                    script.Shield.CurrentAmount = ahp.Amount;
                                    return;
                                }
                            case RoleType.Scp93989:
                            case RoleType.Scp93953:
                                {
                                    var script = ev.Player.Role.As<Exiled.API.Features.Roles.Scp939Role>().Script;
                                    script.Shield.SustainTime = ahp.Sustain;
                                    script.Shield.Limit = ahp.Limit;
                                    script.Shield.Efficacy = ahp.Efficacy;
                                    script.Shield.DecayRate = ahp.Decay;
                                    script.Shield.CurrentAmount = ahp.Amount;
                                    return;
                                }
                        }
                    }
                }));

                Plugin.Coroutines.Add(Timing.CallDelayed(3f, () =>
                {
                    ev.Player.AddAhp(ahp.Amount, ahp.Limit, ahp.Decay, ahp.Efficacy, ahp.Sustain, ahp.Persistent);
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
                if (ev.Killer.ArtificialHealth + health.Config.AhpOnKill[ev.Killer.Role] <= ev.Killer.MaxArtificialHealth)
                    ev.Killer.ArtificialHealth += health.Config.AhpOnKill[ev.Killer.Role];
                else
                    ev.Killer.ArtificialHealth = ev.Killer.MaxArtificialHealth;
            }
        }
    }
}