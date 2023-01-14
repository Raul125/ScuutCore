namespace ScuutCore.Commands.Suicide
{
    using System;
    using CommandSystem;
    using NWAPIPermissionSystem;
    using PluginAPI.Core;
    using Plugin = ScuutCore.Plugin;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Suicide : ICommand
    {
        public string Command { get; } = "suicide";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (PermissionHandler.CheckPermission(player.UserId, "scuutcore.suicide"))
            {
                if (Plugin.Singleton.Config.SuicideDisabledRoles.Contains(player.Role))
                {
                    response = "Disabled for this role";
                    return false;
                }

                player.Kill();
                response = "Done";
                return true;
            }
            else
            {
                response = "<b><color=#00FFAE>Donate!</color></b>";
                return false;
            }
        }
    }
}
