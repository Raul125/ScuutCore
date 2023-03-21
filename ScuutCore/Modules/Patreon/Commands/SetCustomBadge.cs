namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using Types;

    public sealed class SetCustomBadge : PatreonExclusiveCommand
    {
        public const string BadgePermissions = "scuutcore.patreon.custombadge";
        public const string CustomPatreon = " - Custom Patreon";

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
            if (text.Length > 64)
            {
                response = "That badge text is too long.";
                return false;
            }
            var cfg = PatreonPerksModule.Singleton.Config;
            foreach (string phrase in cfg.BlacklistedBadgePhrases)
            {
                if (text.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    response = "That badge text contains a blacklisted phrase.";
                    return false;
                }
            }

            data.Prefs.CustomBadge = text + (cfg.CustomPatreonSuffix ? CustomPatreon : "");
            bool isCustomSelected = data.Prefs.BadgeIndex == Badge.Custom;
            if (isCustomSelected)
                data.UpdateBadge();
            response = $"Your badge text has been set to \"{text}\".{(isCustomSelected ? "" : "Use the \"patreon selectBadge custom\" command to select it.")}";
            return true;
        }
    }
}