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
	using ScuutCore.API.Helpers;
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

			if (!Round.IsRoundStarted)
			{
				response = "You gotta wait for the round to start!";
				return false;
			}

			if (Plugin.Singleton.Config.SuicideDisabledRoles.Contains(player.Role))
			{
				response = "Disabled for this role";
				return false;
			}

			PlayerDeathEffects.PlayExplosionEffect(player);

			player.Kill("Ate taco bell");

			response = "Done";
			return true;
		}
    }
}