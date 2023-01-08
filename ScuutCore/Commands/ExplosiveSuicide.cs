using CommandSystem;
using PluginAPI.Core;
using System;
using NWAPIPermissionSystem;

namespace ScuutCore.Commands
{
    using Footprinting;
    using InventorySystem;
    using InventorySystem.Items.Pickups;
    using InventorySystem.Items.ThrowableProjectiles;
    using Mirror;
    using PluginAPI.Core.Items;
    using UnityEngine;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class ExplosiveSuicide : ICommand
    {
        public string Command { get; } = "explosivesuicide";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (PermissionHandler.CheckPermission(player.UserId, "scuutcore.explosivesuicide"))
            {
                if (Plugin.Singleton.Config.SuicideDisabledRoles.Contains(player.Role))
                {
                    response = "Disabled for this role";
                    return false;
                }
                
                ThrowableItem g = (ThrowableItem)ReferenceHub._localHub.inventory.ServerAddItem(ItemType.GrenadeHE);
                ThrownProjectile thrownProjectile = Object.Instantiate(g.Projectile, player.Position, Quaternion.identity);
                ((ExplosionGrenade)thrownProjectile)._fuseTime = 0.1f;
                PickupSyncInfo pickupSyncInfo = new (g.ItemTypeId, player.Position, Quaternion.identity, g.Weight, g.ItemSerial) { Locked = true };
                PickupSyncInfo pickupSyncInfo2 = pickupSyncInfo;
                thrownProjectile.NetworkInfo = pickupSyncInfo2;
                thrownProjectile.PreviousOwner = new Footprint(g.Owner);
                (thrownProjectile as ExplosionGrenade)._maxRadius = 0f;
                (thrownProjectile as ExplosionGrenade)._scpDamageMultiplier = 0f;
                NetworkServer.Spawn(thrownProjectile.gameObject);
                ItemPickupBase itemPickupBase = thrownProjectile;
                pickupSyncInfo = default;
                itemPickupBase.InfoReceived(pickupSyncInfo, pickupSyncInfo2);
                thrownProjectile.ServerActivate();
                ReferenceHub._localHub.inventory.ServerRemoveItem(g.ItemSerial, null);
                
                player.Kill();

                response = "Done";
                return true;
            }
            else
            {
                response = "<b><color=#00FFAE>Get permissions bozo!</color></b>";
                return false;
            }
        }
    }
}