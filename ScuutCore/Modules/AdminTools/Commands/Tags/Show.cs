namespace ScuutCore.Modules.AdminTools.Commands.Tags;

using System;
using CommandSystem;
using PluginAPI.Core;

public class Show : ICommand
{
    public string Command { get; } = "show";

    public string[] Aliases { get; } = new string[] { };

    public string Description { get; } = "Shows staff tags on the server";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 0)
        {
            response = "Usage: tags show";
            return false;
        }

        foreach (Player player in Player.GetPlayers())
            if (player.ReferenceHub.serverRoles.RemoteAdmin && !player.ReferenceHub.authManager.RemoteAdminGlobalAccess)
                player.ReferenceHub.serverRoles.RefreshLocalTag();

        response = "All staff tags are now visible";
        return true;
    }
}
