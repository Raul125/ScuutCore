using Exiled.API.Enums;
using Exiled.Events.EventArgs;

namespace ScuutCore.Modules.CuffedTK
{
    public class EventHandlers
    {
        private CuffedTK cuffedTK;
        public EventHandlers(CuffedTK btc)
        {
            cuffedTK = btc;
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.Target.IsCuffed || ev.Target == null || ev.Attacker == null || ev.Handler.Type == DamageType.Unknown || ev.Target.Cuffer == ev.Attacker) 
                return;

            if (cuffedTK.Config.DisallowedDamageTypes.Contains(ev.Handler.Type) && (ev.Target.Role.Team == Team.CDP || ev.Target.Role.Team == Team.RSC))
            {
                if (cuffedTK.Config.DamageTypeTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.DamageTypesHint.Replace("%PLAYER%", ev.Target.Nickname).Replace("%DAMAGETYPE%", ev.Handler.Type.ToString()), cuffedTK.Config.DamageTypeTime);

                ev.IsAllowed = false;
            }
            else if ((ev.Target.Role.Team == Team.CDP && cuffedTK.Config.DisallowDamagetoClassD.Contains(ev.Attacker.Role.Team)) || (ev.Target.Role.Team == Team.RSC && cuffedTK.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Role.Team)))
            {
                if (cuffedTK.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.AttackerHint.Replace("%PLAYER%", ev.Target.Nickname), cuffedTK.Config.AttackerHintTime);

                ev.IsAllowed = false;
            }
        }
    }
}