using System;
using System.ComponentModel;
using CommandSystem;
using Exiled.API.Features;

namespace TpsLogger.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CurrentCommand : ICommand
    {
        public string Command { get; set; } = "tps";

        public string[] Aliases { get; set; } = null;

        public string Description { get; set; } = "Gets the current tps";

        [Description("The response to send to the player.")]
        public string Response { get; set; } = "Current TPS: {0}";

        [Description("The response to send to a player when they lack permission to execute the command.")]
        public string InsufficientPermission { get; set; } = "You do not have permission to use this command.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Response, Server.Tps);
            return true;
        }
    }
}