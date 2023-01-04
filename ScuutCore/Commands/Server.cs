using CommandSystem;
using PluginAPI.Core;
using MEC;
using System;
using System.Linq;
using Hints;
using NWAPIPermissionSystem;

namespace ScuutCore.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Server : ICommand
    {
        public string Command { get; } = "server";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (sender.CheckPermission("scuutcore.server"))
            {
                if (!Plugin.Singleton.Config.ServerCommand.TryGetValue(arguments.At(0), out ushort port))
                {
                    response = "Wrong server";
                    return false;
                }

                player.RedirectToServer(port);
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
