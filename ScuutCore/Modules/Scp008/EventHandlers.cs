namespace ScuutCore.Modules.Scp008
{
    using PluginAPI.Core;
    using PlayerRoles;
    using UnityEngine;
    using InventorySystem.Items;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using CustomPlayerEffects;
    using PlayerStatsSystem;

    public class EventHandlers
    {
        private readonly Scp008 instance;
        public EventHandlers(Scp008 instance) => this.instance = instance;

        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnHurt(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null)
                return;

            if (!target.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
                return;

            if (attacker.Role is not RoleTypeId.Scp0492 || poisonedEffect.IsEnabled)
                return;

            if (Random.Range(0, 100) <= instance.Config.InfectionChance)
            {
                target.EffectsManager.EnableEffect<Poisoned>();
                instance.Config.InfectedHint.Show(target);
            }
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void OnHealed(Player player, ItemBase item)
        {
            if (player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) && poisonedEffect.IsEnabled is false)
                return;

            if (!instance.Config.CureChance.TryGetValue(item.ItemTypeId, out int chance))
                return;

            if (Random.Range(0, 100) <= chance)
            {
                poisonedEffect.DisableEffect();
                instance.Config.CuredHint.Show(player);
            }
        }

        [PluginEvent(ServerEventType.PlayerDying)]
        public bool OnDying(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
                return true;

            if (poisonedEffect.IsEnabled is false)
                return true;

            if (instance.Config.DropInventory)
                player.DropEverything();

            player.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Revived, RoleSpawnFlags.None);
            return false;
        }
    }
}