﻿namespace ScuutCore.Modules.PocketFall
{
    using System.Collections.Generic;
    using API.Features;
    using CustomPlayerEffects;
    using InventorySystem.Items;
    using InventorySystem.Items.Usables;
    using MEC;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using RelativePositioning;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<PocketFall>
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnPlayerDamage(ScuutPlayer target, Player attacker, DamageHandlerBase damageHandler) => !target.EnteringPocket;

        [PluginEvent(ServerEventType.Scp106TeleportPlayer)]
        public bool OnPlayerEnterPocketDimension(ScuutPlayer scp106, ScuutPlayer target)
        {
            if (target.EnteringPocket || !target.ReferenceHub.playerStats.DealDamage(new ScpDamageHandler(scp106.ReferenceHub, 40, DeathTranslations.PocketDecay)))
                return false;
            target.EnteringPocket = true;
            Plugin.Coroutines.Add(Timing.RunCoroutine(SendToPocket(target)));
            return false;
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public void ChangeItem(ScuutPlayer target, ushort prev, ushort next)
        {
            if (!target.EnteringPocket)
                return;
            var inv = target.ReferenceHub.inventory;
            if (inv.CurInstance is not UsableItem { IsUsing: true })
                inv.NetworkCurItem = ItemIdentifier.None;
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool DropItem(ScuutPlayer target, ItemBase item) => !target.EnteringPocket;

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public bool ThrowItem(ScuutPlayer target, ItemBase item, Rigidbody rigidbody) => !target.EnteringPocket;

        [PluginEvent(ServerEventType.PlayerUseItem)]
        public bool UseItem(ScuutPlayer target, UsableItem item) => !target.EnteringPocket;

        private IEnumerator<float> SendToPocket(ScuutPlayer player)
        {
            float delay = Module.Config.Delay;
            player.EffectsManager.EnableEffect<Ensnared>(delay + 0.2f);
            player.EffectsManager.DisableEffect<Poisoned>();
            yield return Timing.WaitForSeconds(delay);
            var pos = new RelativePosition(player.Position);

            for (int i = 0; i < Module.Config.Ticks; i++)
            {
                player.Position += Vector3.down;
                yield return Timing.WaitForOneFrame;
            }

            if (Warhead.IsDetonated)
                player.Kill();
            else
            {
                player.SendToPocketDimension();
                var effect = player.EffectsManager.GetEffect<Corroding>();
                effect.ServerSetState(1);
                effect.CapturePosition = pos;
            }

            yield return Timing.WaitForSeconds(0.1f);
            player.EnteringPocket = false;
        }
    }
}