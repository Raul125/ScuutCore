namespace ScuutCore.Modules.ScpReplace.Commands;

using System;
using System.Linq;
using CommandSystem;
using PluginAPI.Core;
using ScuutCore.Modules.Patreon;
using ScuutCore.Modules.Patreon.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ScpReplaceCommand : PatreonExclusiveCommand
{
    protected override string Permission { get; } = "scpreplace";
    public override string Command { get; } = "scpreplace";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Replace a player who left the game.";

    protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
    {
        var ply = Player.Get(sender);
        var item = ScpReplaceModule.ReplaceInfos.FirstOrDefault();
        if (item == null)
        {
            response = "No players to replace.";
            return false;
        }

        item.Apply(ply);
        ScpReplaceModule.ReplaceInfos.Remove(item);
        response = $"Replaced {item.Role}.";
        return true;
    }
}