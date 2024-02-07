namespace ScuutCore.Modules.Scp008;

using API.Features;
using CustomPlayerEffects;
using InventorySystem.Items;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Rewards;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using ScuutCore.API.Extensions;
using UnityEngine;

public sealed class EventHandlers : InstanceBasedEventHandler<Scp008>
{
    [PluginEvent(ServerEventType.PlayerDamage)]
    public void OnHurt(PlayerDamageEvent e)
    {
        if (e.Player == null)
            return;
        if (Module.Config.BlacklistedRoles.Contains(e.Player.Role))
            return;

        var target = e.Target;
        if (!target.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect))
            return;

        if (poisonedEffect.IsEnabled || e.DamageHandler is not AttackerDamageHandler { Attacker.Role: RoleTypeId.Scp0492 })
            return;

        if (Random.Range(0, 100) > Module.Config.InfectionChance)
            return;

        target.EffectsManager.EnableEffect<Poisoned>();
        Module.Config.InfectedHint.Show(target);
    }

    [PluginEvent(ServerEventType.PlayerUsedItem)]
    public bool OnHealed(Player player, ItemBase item)
    {
        if (player.EffectsManager.TryGetEffect(out Scp207 scp027)
            && scp027.IsEnabled
            && player.EffectsManager.TryGetEffect(out Scp1853 scp1853)
            && scp1853.IsEnabled)
            return true;

        if (player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) && !poisonedEffect.IsEnabled)
            return true;

        if (!Module.Config.CureChance.TryGetValue(item.ItemTypeId, out int chance) || Random.Range(0, 100) > chance)
            return true;

        poisonedEffect.DisableEffect();
        Module.Config.CuredHint.Show(player);
        return true;
    }

    [PluginEvent(ServerEventType.PlayerDying)]
    public bool OnDying(PlayerDyingEvent e)
    {
        var player = e.Player;
        if (!player.EffectsManager.TryGetEffect<Poisoned>(out var poisonedEffect) || !poisonedEffect.IsEnabled)
            return true;

        var currRoom = RoomIdUtils.RoomAtPositionRaycasts(player.Position, false);
        if (currRoom != null)
        {
            var name = currRoom.gameObject.name.RemoveBracketsOnEndOfName();
            if (name == "PocketWorld")
                return true;
        }

        if (e.DamageHandler is UniversalDamageHandler universalDamage 
            && universalDamage.TranslationId == DeathTranslations.Crushed.Id)
            return true;

        if (Module.Config.DropInventory)
            player.DropEverything();

        TerminationRewards.OnHumanTerminated(e.Player.ReferenceHub, e.DamageHandler);
        player.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Revived, RoleSpawnFlags.None);
        return false;
    }
}