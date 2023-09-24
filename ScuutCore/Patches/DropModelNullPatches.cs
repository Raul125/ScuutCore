namespace ScuutCore.Patches
{
    using System;
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Items;
    using InventorySystem.Items.Pickups;
    using PluginAPI.Core;
    using UnityEngine;

    [HarmonyPatch(typeof(ItemBase), nameof(ItemBase.ServerDropItem))]
    public static class DropItemPatch
    {
        public static bool Prefix(ItemBase __instance)
        {
            if (__instance.PickupDropModel == null)
            {
                var stackTrace = new System.Diagnostics.StackTrace();
                Log.Error($"PickupDropModel is null! type:{__instance.ItemTypeId} Stack trace: " + stackTrace);
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
            Action<ItemPickupBase> setupMethod)
        {
            if (item.PickupDropModel == null)
            {
                var stackTrace = new System.Diagnostics.StackTrace();
                Log.Error($"PickupDropModel is null! type:{item.ItemTypeId} Stack trace: " + stackTrace);
                return false;
            }

            return true;
        }
    }
}