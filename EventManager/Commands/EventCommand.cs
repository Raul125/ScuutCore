using System;
using CommandSystem;

namespace EventManager.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EventCommand : ParentCommand
    {
        public EventCommand() => LoadGeneratedCommands();
         
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new UseCommand());
            RegisterCommand(new CooldownCommand());
            RegisterCommand(new VoteCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Select a valid argument: use, cooldown, vote";
            return false;
        }

        public override string Command { get; } = "donatorevent";
        public override string[] Aliases { get; } = {"donev"};
        public override string Description { get; } = "EventManager main command";
    }
}