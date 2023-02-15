namespace ScuutCore.Modules.Scp008
{
    using ScuutCore.API.Features;
    using PluginAPI.Core;
    using PlayerRoles;
    using UnityEngine;
    using InventorySystem.Items;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using CustomPlayerEffects;
    using PlayerStatsSystem;

    public sealed class EventHandlers : InstanceBasedEventHandler<Scp008>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnHurt(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (attacker is null)
                return;

            if (!target.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
                return;

            if (attacker.Role is not RoleTypeId.Scp0492 || poisonedEffect.IsEnabled)
                return;

            if (Random.Range(0, 100) > Module.Config.InfectionChance)
                return;
            target.EffectsManager.EnableEffect<Poisoned>();
            Module.Config.InfectedHint.Show(target);
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void OnHealed(Player player, ItemBase item)
        {
            if (player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) && poisonedEffect.IsEnabled is false)
                return;

            if (!Module.Config.CureChance.TryGetValue(item.ItemTypeId, out int chance))
                return;

            if (Random.Range(0, 100) > chance)
                return;
            poisonedEffect.DisableEffect();
            Module.Config.CuredHint.Show(player);
        }

        [PluginEvent(ServerEventType.PlayerDying)]
        public bool OnDying(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
                return true;

            if (poisonedEffect.IsEnabled is false)
                return true;

            if (Module.Config.DropInventory)
                player.DropEverything();

            player.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Revived, RoleSpawnFlags.None);
            return false;
        }
    }
}