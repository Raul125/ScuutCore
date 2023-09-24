namespace ScuutCore.Patches;

using System;
using Footprinting;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables.Scp1576;
using Mirror;
using PluginAPI.Core;
using UnityEngine;
using Object = UnityEngine.Object;

[HarmonyPatch(typeof(ItemBase), nameof(ItemBase.ServerDropItem))]
public static class DropItemPatch
{
    public static bool Prefix(ItemBase __instance, ref ItemPickupBase __result)
    {
        if (__instance.PickupDropModel == null)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            Log.Error($"PickupDropModel is null! type:{__instance.ItemTypeId} Stack trace: " + stackTrace);
            __instance.Owner.inventory.UserInventory.Items.Remove(__instance.ItemSerial);
            Object.Destroy(__instance.gameObject);
            __result = DropModelNullHelper.CreateCoinPickup(__instance.Owner);
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerCreatePickup), 
    typeof(ItemBase), typeof(PickupSyncInfo), typeof(Vector3), typeof(bool), typeof(Action<ItemPickupBase>))]
public static class ServerCreatePickupPatch
{
    public static bool Prefix(ItemBase item, PickupSyncInfo psi, Vector3 position, bool spawn,
        Action<ItemPickupBase> setupMethod, ref ItemPickupBase __result)
    {
        if (item.PickupDropModel == null)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            Log.Error($"PickupDropModel is null! type:{item.ItemTypeId} Stack trace: " + stackTrace);
            __result = DropModelNullHelper.CreateCoinPickup(item.Owner, position, spawn);
            return false;
        }

        return true;
    }
}

internal static class DropModelNullHelper
{
    internal static ItemPickupBase CreateCoinPickup(ReferenceHub? owner = null, Vector3? position = null, bool spawn = false)
    {
        owner ??= ReferenceHub.HostHub;
        var @base = InventoryItemLoader.AvailableItems[ItemType.Coin];
        var item = Object.Instantiate(@base.PickupDropModel);

        PickupSyncInfo psi = new()
        {
            ItemId = ItemType.Coin,
            Serial = ItemSerialGenerator.GenerateNext(),
            WeightKg = @base.Weight
        };

        item.Info = psi;
        item.PreviousOwner = new Footprint(owner);
        if (position.HasValue)
            item.Position = position.Value;
        if (spawn)
            NetworkServer.Spawn(item.gameObject);
        return item;
    }
}