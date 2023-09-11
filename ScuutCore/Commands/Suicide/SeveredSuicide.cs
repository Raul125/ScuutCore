namespace ScuutCore.Commands.Suicide
{
    using System;
    using CommandSystem;
    using CustomPlayerEffects;
    using PluginAPI.Core;
    using ScuutCore.API.Helpers;
    using PermissionHandler = NWAPIPermissionSystem.PermissionHandler;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class SeveredSuicide : ICommand
    {
        public const string DeathReason = "Hands fell off";

        public string Command { get; } = "severedsuicide";

        public string[] Aliases { get; } = new[]
        {
            "sever"
        };

        public string Description { get; } = "Sever your hands";

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

            player.EffectsManager.EnableEffect<SeveredHands>();

            response = "Done";
            return true;
        }
    }
}