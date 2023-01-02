﻿using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace ScuutCore.Modules.AdminTools
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Tags : ParentCommand
    {
        public Tags() => LoadGeneratedCommands();

        public override string Command { get; } = "tags";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Hides staff tags in the server";

        public override void LoadGeneratedCommands() 
        {
            RegisterCommand(new Hide());
            RegisterCommand(new Show());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ScuutCore.tp"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            response = "Invalid subcommand. Available ones: hide, show";
            return false;
        }
    }
}