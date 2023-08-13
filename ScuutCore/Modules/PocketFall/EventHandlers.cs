namespace ScuutCore.Modules.PocketFall
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
        public bool OnPlayerDamage(Player target, Player attacker, DamageHandlerBase damageHandler)
        {
            if (Player.TryGet(target.ReferenceHub, out ScuutPlayer scuutPlayer))
                return !scuutPlayer.EnteringPocket;

            return true;
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public void ChangeItem(Player target, ushort prev, ushort next)
        {
            if (target.ReferenceHub.isLocalPlayer)
                return;

            if (Player.TryGet(target.ReferenceHub, out ScuutPlayer scuutPlayer) && scuutPlayer.EnteringPocket)
            {
                var inv = target.ReferenceHub.inventory;
                if (inv.CurInstance is not UsableItem { IsUsing: true })
                    inv.NetworkCurItem = ItemIdentifier.None;
            }
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool DropItem(Player target, ItemBase item)
        {
            if (Player.TryGet(target.ReferenceHub, out ScuutPlayer scuutPlayer))
                return !scuutPlayer.EnteringPocket;

            return true;
        }

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public bool ThrowItem(Player target, ItemBase item, Rigidbody rigidbody)
        {
            if (Player.TryGet(target.ReferenceHub, out ScuutPlayer scuutPlayer))
                return !scuutPlayer.EnteringPocket;

            return true;
        }
        
        [PluginEvent(ServerEventType.PlayerUseItem)]
        public bool UseItem(Player target, UsableItem item)
        {
            if (Player.TryGet(target.ReferenceHub, out ScuutPlayer scuutPlayer))
                return !scuutPlayer.EnteringPocket;

            return true;
        }

        public IEnumerator<float> SendToPocket(ScuutPlayer player)
        {
            float delay = Module.Config.Delay;
            var effects = player.EffectsManager;
            effects.EnableEffect<Ensnared>(delay + 0.2f);
            effects.DisableEffect<Poisoned>();
            yield return Timing.WaitForSeconds(delay);
            effects.DisableEffect<Poisoned>();
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
                var effect = effects.GetEffect<PocketCorroding>();
                effect.ServerSetState(1);
                effect.CapturePosition = pos;
            }

            yield return Timing.WaitForSeconds(0.1f);
            player.EffectsManager.DisableEffect<Poisoned>();
            player.EnteringPocket = false;
        }

        public static void Send(ReferenceHub hub)
        {
            if (!Player.TryGet(hub, out ScuutPlayer player) || PocketFall.Instance == null)
                return;
            Plugin.Coroutines.Add(Timing.RunCoroutine(PocketFall.Instance.EventHandlers.SendToPocket(player)));
        }
    }
}