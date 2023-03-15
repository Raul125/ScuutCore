namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using CommandSystem;
    using RemoteAdmin;
    public abstract class PatreonExclusiveCommand : ICommand
    {
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

            if (PatreonExtensions.IsSupporter(ps.ReferenceHub))
                return ExecuteInternal(arguments, sender, out response);

            response = "You are not a Patreon supporter.";
            return false;
        }


        protected abstract bool ExecuteInternal(ArraySegment<string> arguments, ICommandSender sender, out string response);
    }
}