﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace ScuutCore.Modules.AdminTools
{
    public class Hide : ICommand
    {
        public string Command { get; } = "hide";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Hides staff tags on the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ScuutCore.tp"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: tags hide";
                return false;
            }

            foreach (Player player in Player.List)
                if (player.ReferenceHub.serverRoles.RemoteAdmin)
                    player.BadgeHidden = true;

            response = "All staff tags are hidden now";
            return true;
        }
    }
}
