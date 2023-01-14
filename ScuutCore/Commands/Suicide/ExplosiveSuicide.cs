namespace ScuutCore.Commands.Suicide
{
	using System;
	using CommandSystem;
	using Footprinting;
	using InventorySystem;
	using InventorySystem.Items.Pickups;
	using InventorySystem.Items.ThrowableProjectiles;
	using Mirror;
	using PluginAPI.Core;
	using UnityEngine;
	using PermissionHandler = NWAPIPermissionSystem.PermissionHandler;

	[CommandHandler(typeof(ClientCommandHandler))]
	public class ExplosiveSuicide : ICommand
    {
        public string Command { get; } = "explosivesuicide";

		public string[] Aliases { get; } = new []{"explode"};

		public string Description { get; } = "Explode";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!PermissionHandler.CheckPermission(player.UserId, "scuutcore.explosivesuicide"))
			{
				response = "<b><color=#00FFAE>Get permissions bozo!</color></b>";
				return false;
			}
			if (Plugin.Singleton.Config.SuicideDisabledRoles.Contains(player.Role))
			{
				response = "Disabled for this role";
				return false;
			}
			ThrowableItem throwableItem = (ThrowableItem)ReferenceHub._hostHub.inventory.ServerAddItem(ItemType.GrenadeHE, 0, null);
			ThrownProjectile thrownProjectile = UnityEngine.Object.Instantiate<ThrownProjectile>(throwableItem.Projectile, player.Position, Quaternion.identity);
			((ExplosionGrenade)thrownProjectile)._fuseTime = 0.1f;
			PickupSyncInfo pickupSyncInfo = new PickupSyncInfo(throwableItem.ItemTypeId, player.Position, Quaternion.identity, throwableItem.Weight, throwableItem.ItemSerial)
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
			player.Kill();
			response = "Done";
			return true;
		}
    }
}