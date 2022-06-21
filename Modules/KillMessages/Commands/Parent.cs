﻿using System;
using System.Runtime.InteropServices;
using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.PermissionsManagement.Group;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mirror;

namespace ScuutCore.Modules.KillMessages
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Parent : ParentCommand
    {
        public Parent() => LoadGeneratedCommands();
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Set());
            RegisterCommand(new Delete());
            RegisterCommand(new Toggle());
            RegisterCommand(new SetColor());
        }
        
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("kmsg"))
            {
                response = KillMessages.Singleton.Config.NoPerms;
                return false;
            }

            Player p = Player.Get(sender);
            string current = p.GetMessage();
            string color = p.GetColor();
            response = KillMessages.Singleton.Config.HelpMessage.Replace($"$current", string.IsNullOrEmpty(current) ? KillMessages.Singleton.Config.MessageNotSet : current).Replace("$color", color);
            return true;
        }

        public override string Command { get; } = "killmessage";
        public override string[] Aliases { get; } = {"kmsg"};
        public override string Description { get; } = "Sets or deletes your kill message. Available commands: kmsg set, kmsg delete, kmsg toggle, kmsg color";
        
    }
}