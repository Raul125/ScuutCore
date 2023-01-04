using CommandSystem;
using PluginAPI.Core;
using MEC;
using System;
using System.Linq;
using Hints;
using NWAPIPermissionSystem;

namespace ScuutCore.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Suicide : ICommand
    {
        public string Command { get; } = "suicide";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player.CheckPermission("scuutcore.suicide"))
            {
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
