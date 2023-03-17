namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using Types;
    public sealed class SetBadgeIndex : PatreonExclusiveCommand
    {

        public override string Command => "selectBadge";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Selects the badge at the given index.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage: patreon selectBadge <index>";
                return false;
            }

            int index = arguments.At(0).ToLower() switch
            {
                "cycle" => Badge.Cycle,
                "custom" => Badge.Custom,
                _ => -2
            };
            if (index == -2 && !int.TryParse(arguments.At(0), out index))
            {
                response = "Invalid index.";
                return false;
            }

            PatreonData.Get(sender).SetIndex(index);
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