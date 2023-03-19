namespace ScuutCore.Patches
{
    using System.Diagnostics;
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Items;
    using Debug = UnityEngine.Debug;

    [HarmonyPatch(typeof(ItemBase), nameof(ItemBase.ServerDropItem))]
    public static class DropItemPatch
    {
        public static bool Prefix(ItemBase __instance)
        {
            if (__instance.PickupDropModel != null)
                return true;

            var owner = __instance.Owner;
            var msg = $"drop item null pickup: {__instance.ItemTypeId}, serial: {__instance.ItemSerial}, owner: {(owner == null ? "<null>" : owner.nicknameSync.MyNick)}";
            Modules.ErrorLogs.WebhookSender.AddMessage(msg);
            Debug.Log(msg);
            if (ItemCreationCache.Cache.ContainsKey(__instance))
            {
                Modules.ErrorLogs.WebhookSender.AddMessage("Cache contains key");
                Debug.Log("Cache contains key");
                Debug.Log(ItemCreationCache.Cache[__instance]);
            }

            Debug.Log(new StackTrace().ToString());
            __instance.OwnerInventory.ServerRemoveItem(__instance.ItemSerial, null);
            return false;
        }
    }
}