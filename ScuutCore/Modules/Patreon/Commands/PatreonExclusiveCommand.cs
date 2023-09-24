namespace ScuutCore.Modules.Patreon.Commands;

using System;
using CommandSystem;
using NWAPIPermissionSystem;
using RemoteAdmin;
public abstract class PatreonExclusiveCommand : ICommand
{
    protected abstract string Permission { get; }

    public abstract string Command { get; }
    public abstract string[] Aliases { get; }
    public abstract string Description { get; }

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not PlayerCommandSender ps)
        {
            response = "You must be a player to use this command.";
            return false;
        }

        var data = PatreonData.Get(ps.ReferenceHub);
        if (!data.Rank.IsValid)
        {
            response = "You don't have a valid Patreon rank.";
            return false;
        }

        if (sender.CheckPermission(Permission))
            return ExecuteInternal(arguments, ps.ReferenceHub, data, out response);

        response = "You don't have permissions to use this command.";
        return false;
    }

    protected abstract bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response);
}