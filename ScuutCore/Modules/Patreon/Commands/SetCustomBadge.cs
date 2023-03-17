namespace ScuutCore.Modules.Patreon.Commands
{
    using System;
    using Types;

    public sealed class SetCustomBadge : PatreonExclusiveCommand
    {

        public override string Command => "customBadge";
        public override string[] Aliases { get; } =
        {
        };
        public override string Description => "Sets your custom badge text.";
        protected override bool ExecuteInternal(ArraySegment<string> arguments, ReferenceHub sender, out string response)
        {
            var data = PatreonData.Get(sender);
            if (!data.Rank.Perks.Contains(PatreonPerk.CustomBadge))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: patreon customBadge <text>";
                return false;
            }

            string text = string.Join(" ", arguments).Trim();
            foreach (string phrase in PatreonPerksModule.Singleton.Config.BlacklistedBadgePhrases)
            {
                if (text.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    response = "Your badge text contains a blacklisted phrase.";
                    return false;
                }
            }

            data.Custom.CustomBadge = text;
            response = $"Your badge text has been set to \"{text}\". Use the \"patreon selectBadge custom\" command to select it.";
            return true;
        }
    }
}