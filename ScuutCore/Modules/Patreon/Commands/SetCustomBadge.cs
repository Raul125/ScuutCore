namespace ScuutCore.Modules.Patreon.Commands
{
    using System;

    public sealed class SetCustomBadge : PatreonExclusiveCommand
    {
        public const string BadgePermissions = "scuutcore.patreon.custombadge";

        protected override string Permission => BadgePermissions;
        public override string Command => "customBadge";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Sets your custom badge text.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, PatreonData data, out string response)
        {
            if (arguments.Count < 1)
            {
                data.Prefs.CustomBadge = null;
                response = "Removed your custom badge.";
                data.UpdateBadge();
                return false;
            }

            string text = string.Join(" ", arguments).Trim();
            foreach (string phrase in PatreonPerksModule.Singleton.Config.BlacklistedBadgePhrases)
            {
                if (text.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    response = "That badge text contains a blacklisted phrase.";
                    return false;
                }
            }

            data.Prefs.CustomBadge = text;
            response = $"Your badge text has been set to \"{text}\". Use the \"patreon selectBadge custom\" command to select it.";
            return true;
        }
    }
}