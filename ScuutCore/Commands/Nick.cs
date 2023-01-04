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
    public class Nick : ICommand
    {
        public string Command { get; } = "nick";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (PermissionHandler.CheckPermission(player.UserId, "scuutcore.nick"))
            {
                string nombreMsg = string.Empty;
                foreach (string str in arguments)
                {
                    nombreMsg += str + " ";
                }

                response = "<b><color=#00FFAE>Nickname changed!</color></b>";
                player.DisplayNickname = nombreMsg.Trim();
            }
            else
            {
                response = "<b><color=#00FFAE>Donate!</color></b>";
            }

            return true;
        }
    }
}
