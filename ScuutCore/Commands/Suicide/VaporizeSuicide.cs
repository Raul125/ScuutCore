namespace ScuutCore.Commands.Suicide
{
	using System;
	using CommandSystem;
	using Footprinting;
	using InventorySystem;
	using InventorySystem.Items.Pickups;
	using InventorySystem.Items.ThrowableProjectiles;
	using Mirror;
	using PlayerStatsSystem;
	using PluginAPI.Core;
	using PermissionHandler = NWAPIPermissionSystem.PermissionHandler;

	[CommandHandler(typeof(ClientCommandHandler))]
	public class VaporizeSuicide
    {
        public string Command { get; } = "explosivesuicide";

		public string[] Aliases { get; } = new []{"vaporize"};

		public string Description { get; } = "Eat dust.";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Player player = Player.Get(sender);
			if (!PermissionHandler.CheckPermission(player.UserId, "scuutcore.vaporizesuicide"))
			{
				response = "<b><color=#00FFAE>Get permissions bozo!</color></b>";
				return false;
			}
			if (Plugin.Singleton.Config.SuicideDisabledRoles.Contains(player.Role))
			{
				response = "Disabled for this role";
				return false;
			}
			
			player.Damage(new DisruptorDamageHandler(new Footprint(ReferenceHub._hostHub), -1f));
			response = "Done";
			return true;
		}
    }
}