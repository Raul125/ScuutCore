namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using System.Linq;
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
            RegisterCommand(new FlyingRagdollSelf());
            RegisterCommand(new FlyingRagdollKills());
            RegisterCommand(new ToggleSpectatorList());
        }

        public override string Command => "patreon";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Commands for Patreon supporters.";

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Patreon commands:\n" + string.Join("\n", Commands.Values.Select(e => $"{e.Command} - {e.Description}"));
            return true;
        }
    }
}