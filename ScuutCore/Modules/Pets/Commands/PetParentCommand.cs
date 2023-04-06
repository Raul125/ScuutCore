using CommandSystem;
using PluginAPI.Core;
using System;

namespace ScuutCore.Modules.Pets
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PetParentCommand : ParentCommand
    {
        public PetParentCommand() => LoadGeneratedCommands();

        public override string Command => "pet";

        public override string[] Aliases { get; } = { };

        public override string Description => "Pets main command.";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Hide());
            RegisterCommand(new Show());
            RegisterCommand(new Reset());
            RegisterCommand(new Name());
            RegisterCommand(new Item());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(sender);
            response = "Use a subcommand: hide/show/reset/name/item";
            return false;
        }
    }
}
