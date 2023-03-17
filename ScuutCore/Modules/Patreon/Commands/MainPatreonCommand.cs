namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using CommandSystem;
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class MainPatreonCommand : ParentCommand
    {
        public MainPatreonCommand()
        {
            LoadGeneratedCommands();
        }

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SetBadgeIndex());
            RegisterCommand(new SetCustomBadge());
            RegisterCommand(new SetCustomColor());
            // TODO
        }

        public override string Command => "patreon";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Commands for Patreon supporters.";

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Patreon commands:\n" + string.Join("\n", Commands.Keys);
            return true;
        }
    }
}