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
	using PermissionHandler = NWAPIPermissionSystem.PermissionHandler;

	public class ExplosiveSuicide
    {
        public string Command { get; } = "explosivesuicide";

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000716C File Offset: 0x0000536C
		public string[] Aliases { get; } = new string[0];

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00007174 File Offset: 0x00005374
		public string Description { get; } = "";

		// Token: 0x06000253 RID: 595 RVA: 0x0000717C File Offset: 0x0000537C
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
			NetworkServer.Spawn(thrownProjectile.gameObject, null);
			ItemPickupBase itemPickupBase = thrownProjectile;
			pickupSyncInfo2 = default(PickupSyncInfo);
			ItemPickupBase itemPickupBase2 = itemPickupBase;
			pickupSyncInfo = default(PickupSyncInfo);
			itemPickupBase2.InfoReceived(pickupSyncInfo, pickupSyncInfo2);
			thrownProjectile.ServerActivate();
			ReferenceHub._hostHub.inventory.ServerRemoveItem(throwableItem.ItemSerial, null);
			player.Kill();
			response = "Done";
			return true;
		}
    }
}