namespace ScuutCore.Modules.Scp008
{
    using API.Features;
    using CustomPlayerEffects;
    using InventorySystem.Items;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<Scp008>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnHurt(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!target.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
                return;
            if (poisonedEffect.IsEnabled || damageHandler is not AttackerDamageHandler { Attacker: { Role: RoleTypeId.Scp0492 } })
                return;

            if (Random.Range(0, 100) > Module.Config.InfectionChance)
                return;
            target.EffectsManager.EnableEffect<Poisoned>();
            Module.Config.InfectedHint.Show(target);
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void OnHealed(Player player, ItemBase item)
        {
            if (player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) && !poisonedEffect.IsEnabled)
                return;

            if (!Module.Config.CureChance.TryGetValue(item.ItemTypeId, out int chance) || Random.Range(0, 100) > chance)
                return;
            poisonedEffect.DisableEffect();
            Module.Config.CuredHint.Show(player);
        }

        [PluginEvent(ServerEventType.PlayerDying)]
        public bool OnDying(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) || !poisonedEffect.IsEnabled)
                return true;

            if (Module.Config.DropInventory)
                player.DropEverything();

            player.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Revived, RoleSpawnFlags.None);
            return false;
        }
    }
}