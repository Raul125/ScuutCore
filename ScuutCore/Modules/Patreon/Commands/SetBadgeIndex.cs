namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using CommandSystem;
    using RemoteAdmin;
    using Types;
    public sealed class SetBadgeIndex : PatreonExclusiveCommand
    {

        public override string Command => "selectBadge";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Selects the badge at the given index.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender ps)
            {
                response = "You must be a player to use this command.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Usage: patreon selectBadge <index>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int index))
            {
                response = "Invalid index.";
                return false;
            }

            PatreonData.Get(ps.ReferenceHub).SetIndex(index);
            response = index switch
            {
                Badge.Cycle => "Your badge will now be cycled through.",
                Badge.Custom => "Your badge will now be set to your custom badge.",
                _ => $"Selected badge at index {index + 1}."
            };
            return true;
        }
    }
}