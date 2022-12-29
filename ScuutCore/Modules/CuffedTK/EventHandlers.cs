using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

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
            if (!ev.Player.IsCuffed || ev.Player is null || ev.Attacker is null || ev.Player.Cuffer == ev.Attacker || ev.Player.Role.Type == RoleTypeId.Tutorial) 
                return;

            /*if (cuffedTK.Config.DisallowedDamageTypes.Contains(ev.DamageHandler.Type) && (ev.Player.Role.Team == Team.ClassD || ev.Player.Role.Team == Team.Scientists || ev.Player.Role.Team == Team.FoundationForces || ev.Player.Role.Team == Team.ChaosInsurgency))
            {
                if (cuffedTK.Config.DamageTypeTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.DamageTypesHint.Replace("%PLAYER%", ev.Player.Nickname).Replace("%DAMAGETYPE%", ev.DamageHandler.Type.ToString()), cuffedTK.Config.DamageTypeTime);

                ev.IsAllowed = false;
            }
            else if ((ev.Player.Role.Team == Team.ClassD && cuffedTK.Config.DisallowDamagetoClassD.Contains(ev.Attacker.Role.Team)) || (ev.Player.Role.Team == Team.Scientists && cuffedTK.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Role.Team)) || (ev.Player.Role.Team == Team.FoundationForces && cuffedTK.Config.DisallowDamagetoMTF.Contains(ev.Attacker.Role.Team)) || (ev.Player.Role.Team == Team.ChaosInsurgency && cuffedTK.Config.DisallowDamagetoChaos.Contains(ev.Attacker.Role.Team)))
            {
                if (cuffedTK.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.AttackerHint.Replace("%PLAYER%", ev.Player.Nickname), cuffedTK.Config.AttackerHintTime);

                ev.IsAllowed = false;
            }*/

            if (cuffedTK.Config.DisallowedDamageTypes.Contains(ev.DamageHandler.Type))
            {
                if (cuffedTK.Config.DamageTypeTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.DamageTypesHint.Replace("%PLAYER%", ev.Player.Nickname).Replace("%DAMAGETYPE%", ev.DamageHandler.Type.ToString()), cuffedTK.Config.DamageTypeTime);

                ev.IsAllowed = false;
            }
            else if (ev.Attacker.Role.Side != Side.Scp)
            {
                if (cuffedTK.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(cuffedTK.Config.AttackerHint.Replace("%PLAYER%", ev.Player.Nickname), cuffedTK.Config.AttackerHintTime);

                ev.IsAllowed = false;
            }
        }
    }
}