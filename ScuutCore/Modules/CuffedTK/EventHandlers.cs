namespace ScuutCore.Modules.CuffedTK
{
    using ScuutCore.API.Features;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    public sealed class EventHandlers : InstanceBasedEventHandler<CuffedTK>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnHurting(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!target.IsDisarmed || target is null || attacker is null || target.DisarmedBy.ReferenceHub == attacker.ReferenceHub || attacker.Role is RoleTypeId.Tutorial)
                return true;
            if (attacker.Role.GetFaction() == Faction.SCP)
                return true;
            if (Module.Config.AttackerHintTime > 0)
                attacker.ReceiveHint(Module.Config.AttackerHint.Replace("%PLAYER%", target.Nickname), Module.Config.AttackerHintTime);

            return false;

        }

        [PluginEvent(ServerEventType.PlayerRemoveHandcuffs)]
        public bool OnPlayerRemoveHandcuffs(Player player, Player target)
        {
            int num = 0;
            if (target == null)
                num ++;
            if (player == null)
                num += 2;
            if (target.DisarmedBy == null)
                num += 4;
            if (num > 0)
            {
                player.ReceiveHint("You are allowed to uncuff this player since something was null, report plz. Code: " + num, 5);
                Log.Debug("CuffedTK null: " + num);
                return true;
            }
            if (Module.Config.OnlyAllowCufferToRemoveHandcuffs && target.DisarmedBy!.PlayerId != player!.PlayerId && target.DisarmedBy.Team != Team.Dead && player.Role.GetFaction() != target.Role.GetFaction())
            {
                player.ReceiveHint(Module.Config.YouCantUnCuffMessage.Replace("{player}", player.Nickname), 5);
                return false;
            }

            return true;
        }
    }
}