namespace ScuutCore.API.Helpers
{
    using Footprinting;
    using InventorySystem;
    using InventorySystem.Items.Pickups;
    using InventorySystem.Items.ThrowableProjectiles;
    using Mirror;
    using PluginAPI.Core;
    using UnityEngine;

    public static class PlayerDeathEffects
    {
        public static void PlayExplosionEffect(Player ply)
        {
            ThrowableItem throwableItem = (ThrowableItem)ReferenceHub._hostHub.inventory.ServerAddItem(ItemType.GrenadeHE, 0, null);
            ThrownProjectile thrownProjectile = UnityEngine.Object.Instantiate<ThrownProjectile>(throwableItem.Projectile, ply.Position, Quaternion.identity);
            ((ExplosionGrenade)thrownProjectile)._fuseTime = 0.1f;
            PickupSyncInfo pickupSyncInfo = new PickupSyncInfo(throwableItem.ItemTypeId, ply.Position, Quaternion.identity, throwableItem.Weight, throwableItem.ItemSerial)
            {
                Locked = true
            };
            PickupSyncInfo pickupSyncInfo2 = pickupSyncInfo;
            thrownProjectile.NetworkInfo = pickupSyncInfo2;
            thrownProjectile.PreviousOwner = new Footprint(throwableItem.Owner);
            (thrownProjectile as ExplosionGrenade)._maxRadius = 0f;
            (thrownProjectile as ExplosionGrenade)._scpDamageMultiplier = 0f;
            NetworkServer.Spawn(thrownProjectile.gameObject);
            ItemPickupBase itemPickupBase = thrownProjectile;
            pickupSyncInfo2 = default;
            ItemPickupBase itemPickupBase2 = itemPickupBase;
            pickupSyncInfo = default;
            itemPickupBase2.InfoReceived(pickupSyncInfo, pickupSyncInfo2);
            thrownProjectile.ServerActivate();
            ReferenceHub._hostHub.inventory.ServerRemoveItem(throwableItem.ItemSerial, null);
        }
    }
}