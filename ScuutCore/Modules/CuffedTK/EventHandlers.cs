using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.CuffedTK
{
    public class EventHandlers
    {
        private CuffedTK cuffedTK;
        public EventHandlers(CuffedTK btc)
        {
            cuffedTK = btc;
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnHurting(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!target.IsDisarmed || target is null || attacker is null || target.DisarmedBy == attacker || target.Role is RoleTypeId.Tutorial) 
                return true;

            if (attacker.Role.GetFaction() != Faction.SCP)
            {
                if (cuffedTK.Config.AttackerHintTime > 0)
                    attacker.ReceiveHint(cuffedTK.Config.AttackerHint.Replace("%PLAYER%", target.Nickname), cuffedTK.Config.AttackerHintTime);

                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerRemoveHandcuffs)]
        public bool OnPlayerRemoveHandcuffs(Player player, Player target)
        {
            if (!cuffedTK.Config.OnlyAllowCufferToRemoveHandcuffs || player.Role.GetFaction() == target.Role.GetFaction())
                return true;

            if (target.DisarmedBy != player)
            {
                player.ReceiveHint(cuffedTK.Config.YouCantUnCuffMessage.Replace("{player}", player.Nickname), 5);
                return false;
            }
                

            return true;
        }
    }
}