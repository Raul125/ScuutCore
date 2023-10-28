namespace ScuutCore.Modules.AdminTools.Commands.Tags;

using System;
using API.Extensions;
using CommandSystem;
using PluginAPI.Core;

public class Hide : ICommand
{
    public string Command { get; } = "hide";

    public string[] Aliases { get; } = new string[] { };

    public string Description { get; } = "Hides staff tags on the server";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 0)
        {
            response = "Usage: tags hide";
            return false;
        }

        foreach (Player player in Player.GetPlayers())
            if (player.ReferenceHub.serverRoles.RemoteAdmin)
                player.ReferenceHub.serverRoles.HideTag();

        response = "All staff tags are hidden now";
        return true;
    }
}
