namespace ScuutCore.Patches
{
    using System.Diagnostics;
    using HarmonyLib;
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
            Debug.Log($"drop item null pickup: {__instance.ItemTypeId}, serial: {__instance.ItemSerial}, owner: {(owner == null ? "<null>" : owner.nicknameSync.MyNick)}");
            Debug.Log(new StackTrace().ToString());
            return true;
        }
    }
}