namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using NWAPIPermissionSystem;
    using Types;
    public sealed class SetBadgeIndex : PatreonExclusiveCommand
    {

        protected override string Permission => "scuutcore.patreon.selectbadge";
        public override string Command => "selectBadge";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Selects the badge at the given index.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
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
            if (index != -2 || int.TryParse(arguments.At(0), out index))
                return DoSelect(sender, out response, index);
            response = "Invalid index.";
            return false;

        }
        private static bool DoSelect(ReferenceHub sender, out string response, int index)
        {
            var data = PatreonData.Get(sender);
            switch (index)
            {
                case Badge.Cycle:
                    data.SetIndex(index);
                    response = "Your badges will now be cycled through.";
                    break;
                case Badge.Custom:
                    if (!sender.queryProcessor._sender.CheckPermission(SetCustomBadge.BadgePermissions) || data.Prefs.CustomBadge == null)
                    {
                        response = "You don't have a custom badge set.";
                        return false;
                    }
                    data.SetIndex(index);
                    response = "Your badge will now be set to your custom badge.";
                    break;
                default:
                    if (data.Rank.BadgeOptions.Count < index)
                    {
                        response = "You don't have a badge at that index.";
                        return false;
                    }
                    data.SetIndex(index);
                    response = $"Selected badge at index {index}.";
                    break;
            }
            return true;
        }
    }
}