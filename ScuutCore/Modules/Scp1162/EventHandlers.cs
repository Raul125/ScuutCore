﻿namespace ScuutCore.Modules.Scp1162;

using ScuutCore.API.Features;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using UnityEngine;
using CustomPlayerEffects;
using InventorySystem.Items;
using PluginAPI.Core.Items;
using PluginAPI.Core.Zones;
using System.Linq;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;

public sealed class EventHandlers : InstanceBasedEventHandler<Scp1162>
{
    private Vector3 SCP1162Position;

    [PluginEvent(ServerEventType.MapGenerated)]
    public void OnGenerationMap()
    {
        var Room = LightZone.Rooms.FirstOrDefault(x => x.GameObject.name == "LCZ_173");
        var scp1162 = new SimplifiedToy(PrimitiveType.Cylinder, new Vector3(17f, 13f, 3.59f), new Vector3(90f, 0f, 0f),
            new Vector3(1.3f, 0.1f, 1.3f), Color.black, Room.Transform, 0.95f).Spawn();

        SCP1162Position = scp1162.transform.position;
    }

    [PluginEvent(ServerEventType.PlayerDropItem)]
    public bool OnPlayerDroppedItem(Player player, ItemBase item)
    {
        if (!Round.IsRoundStarted) 
            return true;
        if (player.Role == RoleTypeId.Scp3114)
            return true;

        if (Vector3.Distance(SCP1162Position, player.Position) <= Module.Config.SCP1162Distance)
        {
            if (Module.Config.CuttingHands)
            {
                if (Module.Config.OnlyThrow)
                {
                    player.EffectsManager.EnableEffect<SeveredHands>(1000);
                    return true;
                }

                if (player.CurrentItem != item)
                {
                    if (Module.Config.ChanceCutting >= Random.Range(0, 101))
                    {
                        player.EffectsManager.EnableEffect<SeveredHands>(1000);
                        return true;
                    }
                }
            }

            OnUseSCP1162(player, item);
            return false;
        }

        return true;
    }

    [PluginEvent(ServerEventType.PlayerThrowItem)]
    public bool OnThrowItem(Player player, ItemBase item, Rigidbody rb)
    {
        if (!Round.IsRoundStarted) 
            return true;

        if (Vector3.Distance(SCP1162Position, player.Position) <= Module.Config.SCP1162Distance)
        {
            OnUseSCP1162(player, item);
            return false;
        }

        return true;
    }

    private void OnUseSCP1162(Player player, ItemBase item)
    {
        if (item.ItemTypeId is ItemType.SCP330 || item is ICandy)
            return;

        var newItemType = ItemType.None;

        getItem:
        foreach (var pair in Module.Config.Chances)
        {
            if (Plugin.Random.Next(0, 100) > pair.Value)
                continue;

            newItemType = pair.Key;
            break;
        }

        if (newItemType == ItemType.None)
            goto getItem;

        var message = Module.Config.ItemDropMessage.Replace("{dropitem}", item.ItemTypeId.ToString())
                .Replace("{giveitem}", newItemType.ToString());
        if (Module.Config.UseHints)
            player.ReceiveHint(message, 4);
        else
            player.SendBroadcast(message, 4);

        player.RemoveItem(new Item(item));
        player.AddItem(newItemType);
    }
}