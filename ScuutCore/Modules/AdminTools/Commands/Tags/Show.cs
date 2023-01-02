using System;
using CommandSystem;
using PluginAPI.Core;
using Exiled.Permissions.Extensions;

namespace ScuutCore.Modules.AdminTools
{
    public class Show : ICommand
    {
        public string Command { get; } = "show";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Shows staff tags on the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ScuutCore.tp"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: tags show";
                return false;
            }

            foreach (Player player in Player.List)
                if (player.ReferenceHub.serverRoles.RemoteAdmin && !player.ReferenceHub.serverRoles.RaEverywhere)
                    player.BadgeHidden = false;

            response = "All staff tags are now visible";
            return true;
        }
    }
}
